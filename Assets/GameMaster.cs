using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
	public GameObject scoreBox;
	public GameObject highScoreBox;
	public GameObject levelBox;
	public GameObject playerLivesCounter;
	public GameObject announcement;
	public GameObject player;
	public GameObject enemy1;
	public GameObject enemy2;
	public GameObject enemy3;
	public GameObject enemyLaser;
	public GameObject enemyMissile;
	public GameObject enemyBox;
	public GameObject barrierPrefab;
	public float speed = 0.75f;
	public float fireChance = 0.05f;
	
	int lives = 3;
	int level = 1;
	bool isGameActive = false;
	bool isRespawning = false;
	bool isNextLevel = false;
	bool isGameOver = false;
	float timeOfPause;
	int score = 0;
	int highScore = 0;
	TextMesh scoreDisplay;
	TextMesh highScoreDisplay;
	TextMesh announcementText;
	Player playerScript;
	float swapTime;
	float fireTime;
	float currentTime = 0;
	float scaledSpeed;
	float yOffset = -0.15f;
	float xOffset = 0.15f;
	float leftBound;
	float rightBound;
	int leftColumnAlive = 0;
	int rightColumnAlive = 10;
	int direction = 1;
	const float angle = Mathf.PI / 20;
	Vector3 move;
	GameObject[,] enemies;
	GameObject[] barriers;
	GameObject bulletBox;
	int enemiesAlive = 55;

    // Start is called before the first frame update
    void Start()
    {
		playerScript = player.GetComponent<Player>();
		enemies = new GameObject[5, 11]; //row, column
		barriers = new GameObject[4];
		scoreDisplay = scoreBox.GetComponent<TextMesh>();
		highScoreDisplay = highScoreBox.GetComponent<TextMesh>();
		announcementText = announcement.GetComponent<TextMesh>();
		bulletBox = new GameObject("Bullet Box");
		BuildEnemies();
		BuildBarriers();
		Play();
	}

	void BuildEnemies()
	{
		swapTime = Time.time;
		scaledSpeed = speed;
		enemyBox.transform.position = new Vector3(-1, 1);
		leftBound = -.075f;
		rightBound = xOffset * 10 + .075f;
		enemiesAlive = 55;
		direction = 1;
		leftColumnAlive = 0;
		rightColumnAlive = 10;
		Vector3 newpos = new Vector3(0, 0, 1);
		for (int j = 0; j < 11; j++)
		{
			enemies[0, j] = Instantiate(enemy3, enemyBox.transform, false);
			enemies[0, j].transform.localPosition = newpos;
			newpos.x += xOffset;
		}
		for (int i = 1; i < 3; i++)
		{
			newpos.y += yOffset;
			newpos.x = 0f;
			for (int j = 0; j < 11; j++)
			{
				enemies[i, j] = Instantiate(enemy2, enemyBox.transform, false);
				enemies[i, j].transform.localPosition = newpos;
				newpos.x += xOffset;
			}
		}
		for (int i = 3; i < 5; i++)
		{
			newpos.y += yOffset;
			newpos.x = 0f;
			for (int j = 0; j < 11; j++)
			{
				enemies[i, j] = Instantiate(enemy1, enemyBox.transform, false);
				enemies[i, j].transform.localPosition = newpos;
				newpos.x += xOffset;
			}
		}
	}

	void ClearEnemies()
	{
		foreach (Transform enemy in enemyBox.transform)
		{
			Destroy(enemy.gameObject);
		}
		foreach (Transform bullet in bulletBox.transform)
		{
			Destroy(bullet.gameObject);
		}
	}

	public void CountEnemies()
	{
		enemiesAlive = 0;
		int oldLeftColumnAlive = leftColumnAlive;
		int oldRightColumnAlive = rightColumnAlive;
		leftColumnAlive = 11;
		rightColumnAlive = -1;
		for(int j = oldLeftColumnAlive; j < oldRightColumnAlive+1; j++)
		{
			bool isAlive = false;
			for(int i = 0; i < 5; i++)
			{
				if (enemies[i, j] != null)
				{
					isAlive = true;
					++enemiesAlive;
				}
			}
			if (isAlive && (leftColumnAlive > j)) leftColumnAlive = j;
			if (isAlive && (rightColumnAlive < j)) rightColumnAlive = j;
		}
		leftBound = xOffset * leftColumnAlive - .075f;
		rightBound = xOffset * rightColumnAlive + .075f;
		scaledSpeed = speed + speed * (55-enemiesAlive)/8;
		if (enemiesAlive == 0)
		{
			isNextLevel = true;
			Pause(true);
			announcementText.text = "LEVEL PASSED";
			ClearEnemies();
		}
	}

	void BuildBarriers()
	{
		for (int i = 0; i < 4; ++i)
		{
			barriers[i] = Instantiate(barrierPrefab, transform);
			barriers[i].transform.Translate(-0.75f + (0.5f * i), -.95f, 0);
		}
	}

	void ClearBarriers()
	{
		for (int i = 0; i < 4; ++i)
		{
			Destroy(barriers[i]);
		}
	}

	void NextLevel()
	{
		isNextLevel = false;
		announcementText.text = "";
		++level;
		levelBox.GetComponent<TextMesh>().text = "LEVEL: " + level.ToString();
		ClearEnemies(); //just in case
		ClearBarriers();
		BuildEnemies();
		if (level <= 5)
		{
			BuildBarriers();
			enemyBox.transform.Translate(0, -.1f * (level - 1), 0);
		}
		else enemyBox.transform.Translate(0, -.4f, 0); //max lowering at level 5+
		Play();
	}

	void FireWeapons()
	{
		bool fire = false;
		bool missile = false;
		for (int j = leftColumnAlive; j < rightColumnAlive+1; j++)
		{
			if (Random.value < fireChance) fire = true;
			if (fire)
			{
				if (Random.value < fireChance / 0.33f) missile = true;
				int lowestAlive = -1;
				for (int i = 0; i < 5; i++)
				{
					if (enemies[i, j] != null) lowestAlive = i;
				}
				if (lowestAlive != -1)
				{
					Vector3 fireLocation = Vector3.zero;
					fireLocation.x = xOffset * j;
					fireLocation.y = yOffset * lowestAlive + yOffset;
					if (!missile) Instantiate(enemyLaser, enemyBox.transform.TransformPoint(fireLocation), Quaternion.identity).transform.SetParent(bulletBox.transform);
					else Instantiate(enemyMissile, enemyBox.transform.TransformPoint(fireLocation), Quaternion.identity).transform.SetParent(bulletBox.transform);
					fire = false;
				}
			}
		}
	}

	public void AddScore(int earned)
	{
		score += earned;
		scoreDisplay.text = "Score: " + score.ToString();
		if (score > highScore)
		{
			highScore = score;
			highScoreDisplay.text = "HI-SCORE: " + highScore.ToString();
		}
	}

	public void Pause(bool isMovementAllowed = false)
	{
		isGameActive = false;
		playerScript.isMovementEnabled = isMovementAllowed;
		playerScript.isFiringEnabled = false;
		timeOfPause = Time.time;
	}

	public void Play()
	{
		isGameActive = true;
		playerScript.isMovementEnabled = true;
		playerScript.isFiringEnabled = true;
	}

	public void PlayerDied()
	{
		if (isGameActive)
		{
			playerScript.isAlive = false;
			lives--;
			if (lives == 0)
			{
				GameOver();
				return;
			}
			isRespawning = true;
			Pause();
		}
	}

	public void GameOver()
	{
		Pause();
		isGameOver = true;
		announcementText.text = "GAME OVER";
	}

	void ResetGame()
	{
		ClearEnemies();
		ClearBarriers();
		score = 0;
		scoreDisplay.text = "Score: 0";
		announcementText.text = "";
		lives = 3;
		playerLivesCounter.GetComponent<Lives>().ResetLives();
		level = 1;
		levelBox.GetComponent<TextMesh>().text = "LEVEL: 1";
		BuildBarriers();
		BuildEnemies();
		playerScript.Revive(true);
		isRespawning = false;
		isGameOver = false;
		Play();
	}

	//NOTE: Sides are x = +/- 1.125
	// Update is called once per frame
	void Update()
    {
		if (isGameOver && (Time.time - timeOfPause) > 5)
		{
			ResetGame();
		}
		else if (isRespawning && (Time.time - timeOfPause) > 2)
		{
			playerScript.Revive();
			isRespawning = false;
			playerLivesCounter.GetComponent<Lives>().RemoveLife();
			Play();
		}
		else if (isNextLevel && (Time.time - timeOfPause) > 3)
		{
			NextLevel();
		}
		else if (isGameActive)
		{
			move.x = direction * scaledSpeed * Mathf.Cos(angle) * Time.deltaTime;
			move.y = -scaledSpeed * Mathf.Sin(angle) * Time.deltaTime;
			if (enemyBox.transform.position.x + rightBound + move.x > 1.125f) direction = -1;
			if (enemyBox.transform.position.x + leftBound + move.x < -1.125f) direction = 1;
			enemyBox.transform.Translate(move);
			CountEnemies();

			currentTime = Time.time;
			if (currentTime - swapTime > .15f / scaledSpeed)
			{
				enemyBox.BroadcastMessage("SwapSprites", SendMessageOptions.DontRequireReceiver);
				swapTime = Time.time;
			}
			if (currentTime - fireTime > .05f / scaledSpeed)
			{
				FireWeapons();
				fireTime = Time.time;
			}
		}
	}
}
