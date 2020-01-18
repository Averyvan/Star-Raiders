using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public float playerSpeed = 0.5f;
	public GameObject bullet;
	public GameObject gameMaster;
	public bool isMovementEnabled = false;
	public bool isFiringEnabled = false;
	Vector3 offset = new Vector3(0, 0.06f, 0);
	GameObject existingBullet = null;
	public bool isAlive = true;

	SpriteRenderer sr;
	public Sprite[] sprites;
	int currentSprite = 0;
	float timeOfSwap;

	private Joystick joystick;
	private bool isMobile = false;
	
	// Start is called before the first frame update
	void Start()
    {
		sr = GetComponent<SpriteRenderer>();
		joystick = FindObjectOfType<Joystick>();
		if (Application.platform == RuntimePlatform.Android) isMobile = true;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		if (isMovementEnabled && !isMobile)
		{
			if (Input.GetButton("left") && transform.position.x > -1.0594f)
			{
				transform.Translate(-Time.deltaTime * playerSpeed,0,0);
			}
			if (Input.GetButton("right") && transform.position.x < 1.0594f)
			{
				transform.Translate(Time.deltaTime * playerSpeed, 0, 0);
			}
		}
		else if (isMovementEnabled && isMobile)
		{
			transform.Translate(joystick.Horizontal * Time.deltaTime * playerSpeed, 0, 0);
		}
		if (Input.GetButton("Jump"))
		{
			Fire();
		}
		if (isAlive && currentSprite != 0)
		{
			currentSprite = 0;
			sr.sprite = sprites[0];
		}
		else if (!isAlive)
		{
			if (Time.time - timeOfSwap > 0.1f)
			{
				if (currentSprite == 1)
				{
					currentSprite = 2;
					sr.sprite = sprites[2];
				}
				else
				{
					currentSprite = 1;
					sr.sprite = sprites[1];
				}
				timeOfSwap = Time.time;
			}
		}
	}

	public void Fire()
	{
		if (isFiringEnabled && existingBullet == null)
		{
			existingBullet = Instantiate(bullet, transform.position + offset, Quaternion.identity);
		}
	}

	public void Revive(bool isNewGame = false)
	{
		isAlive = true;
		currentSprite = 0;
		sr.sprite = sprites[0];
		if (!isNewGame)
		{
			transform.position = new Vector3(-0.85f, -1.2f);
		}
		else transform.position = new Vector3(0, -1.2f);
	}
}
