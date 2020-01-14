using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
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
    void Update()
    {
		transform.Translate(0, -.8f * Time.deltaTime, 0);

		selfCollider.OverlapCollider(filter, results);
		Vector3 move = Vector3.zero;
		foreach (Collider2D col in results)
		{
			if (col != null)
			{
				if (col.tag == "Player") col.GetComponentInParent<GameMaster>().PlayerDied();
				else if (col.tag == "Barrier") move.y = -0.05f;
				if (col.tag != "Enemy")
				{
					Instantiate(explosion, transform.position + move, Quaternion.identity, transform.parent);
					Destroy(gameObject);
				}
			}
		}
	}
}
