using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public GameObject deathsprite;
	public Sprite[] sprites;
	public int pointValue = 10;
	public SpriteRenderer sr;
	int currentSprite = 0;
	public ContactFilter2D filter;
	Collider2D[] results;
	Collider2D selfCollider;
	bool endedGame = false;

	// Start is called before the first frame update
	void Start()
    {
		selfCollider = GetComponent<Collider2D>();
		results = new Collider2D[20];
		filter.NoFilter();
		sr = GetComponent<SpriteRenderer>();
	}

    // Update is called once per frame
    public void SwapSprites()
    {
		if (currentSprite == 0) currentSprite = 1;
		else currentSprite = 0;
		sr.sprite = sprites[currentSprite];
    }

	public void Die()
	{
		transform.root.GetComponent<GameMaster>().AddScore(pointValue);
		Instantiate(deathsprite, transform.position, Quaternion.identity, transform.parent);
		Destroy(gameObject);
	}

	private void Update()
	{
		selfCollider.OverlapCollider(filter, results);
		
		foreach(Collider2D col in results)
		{
			if (col != null && !endedGame)
			{
				if (col.tag == "Barrier") Destroy(col.gameObject);
				else if (col.tag == "Player") col.GetComponentInParent<GameMaster>().PlayerDied();
				else if (col.tag == "Environment")
				{
					col.GetComponentInParent<GameMaster>().GameOver();
					endedGame = true;
				}
			}
		}
	}
}
