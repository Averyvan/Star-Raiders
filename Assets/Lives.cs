using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour
{
	public GameObject lifeSprite;
	public GameObject lifeCounter;
	TextMesh lifeCount;
	GameObject life1;
	GameObject life2;
	int lives = 3;

    // Start is called before the first frame update
    void Start()
    {
		lifeCount = lifeCounter.GetComponent<TextMesh>();
		ResetLives();
    }

	public void ResetLives()
	{
		Destroy(life1);
		Destroy(life2);
		life1 = Instantiate(lifeSprite, transform);
		life1.transform.Translate(.475f,0,0);
		life2 = Instantiate(lifeSprite, transform);
		life2.transform.Translate(.625f, 0, 0);
		lives = 3;
		lifeCount.text = "LIVES: 3";
	}
	
	public void RemoveLife()
	{
		if (lives == 3)
		{
			Destroy(life2);
			--lives;
			lifeCount.text = "LIVES: 2";
		}
		else if(lives == 2)
		{
			Destroy(life1);
			--lives;
			lifeCount.text = "LIVES: 1";
		}
		else if(lives == 1)
		{
			--lives;
			lifeCount.text = "LIVES: 0";
		}
	}
}
