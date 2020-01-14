using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float bulletSpeed = 1f;
	public GameObject explosion;
	Collider2D selfCollider;
	ContactFilter2D filter;
	Collider2D[] results;

	// Start is called before the first frame update
	void Start()
	{
		selfCollider = GetComponent<Collider2D>();
		filter.NoFilter();
		results = new Collider2D[1];
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		transform.Translate(0, bulletSpeed * Time.deltaTime, 0);
		selfCollider.OverlapCollider(filter, results);
		
		foreach (Collider2D col in results)
		{
			if (col != null)
			{
				if (col.tag == "Enemy") col.gameObject.GetComponent<Enemy>().Die();
				else if (col.tag == "Barrier") Instantiate(explosion, transform.position, Quaternion.identity, transform.parent);
				else if (col.tag == "Environment") Instantiate(explosion, transform.position, Quaternion.identity, transform.parent);
				if (col.tag != "Player") Destroy(gameObject);
			}
		}
	}

	void OnBecameInvisible() //backup removal
	{
		Destroy(gameObject);
	} //taken from https://answers.unity.com/questions/1230388/how-to-destroy-object-after-it-moves-out-of-screen.html
}
