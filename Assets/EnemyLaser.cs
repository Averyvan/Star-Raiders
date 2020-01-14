using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
	public Sprite[] sprites;
	public GameObject explosion;
	SpriteRenderer sr;
	int currentSprite = 0;
	float scaledSpeed = .05f;
	float swapTime;
	float currentTime = 0;
	Collider2D selfCollider;
	ContactFilter2D filter;
	Collider2D[] results;

	// Start is called before the first frame update
	void Start()
    {
		sr = GetComponent<SpriteRenderer>();
		selfCollider = GetComponent<Collider2D>();
		filter.NoFilter();
		results = new Collider2D[1];
	}

    // Update is called once per frame
    void Update()
    {
		currentTime = Time.time;
		if (currentTime - swapTime > .005 / scaledSpeed)
		{
			if (currentSprite == 0) currentSprite = 1;
			else currentSprite = 0;
			sr.sprite = sprites[currentSprite];
			swapTime = Time.time;
		}
		transform.Translate(0, -16f * scaledSpeed * Time.deltaTime, 0);

		selfCollider.OverlapCollider(filter, results);
		Vector3 move = Vector3.zero;
		foreach (Collider2D col in results)
		{
			if (col != null)
			{
				if (col.tag == "Player") col.GetComponentInParent<GameMaster>().PlayerDied();
				else if (col.tag == "Barrier") move.y = -0.02f;
				if (col.tag != "Enemy")
				{
					Instantiate(explosion, transform.position + move, Quaternion.identity, transform.parent);
					Destroy(gameObject);
				}
			}
		}
	}
	/*
	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Player")
		{
			col.GetComponentInParent<GameMaster>().PlayerDied();
		}
		Instantiate(explosion, transform.position, Quaternion.identity, transform.parent);
		Destroy(gameObject);
	}
	*/
	void OnBecameInvisible() //backup removal
	{
		Destroy(gameObject);
	} //taken from https://answers.unity.com/questions/1230388/how-to-destroy-object-after-it-moves-out-of-screen.html

}
