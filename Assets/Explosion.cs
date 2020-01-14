using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	public ContactFilter2D filter;
	Collider2D[] results;
	Collider2D selfCollider;
	// Start is called before the first frame update
	void Start()
    {
		filter.layerMask = LayerMask.GetMask("Barrier");
		filter.useLayerMask = true;
		selfCollider = GetComponent<Collider2D>();
		results = new Collider2D[20];
    }

	// Update is called once per frame
	private void Update()
	{
		selfCollider.OverlapCollider(filter, results);
		foreach (Collider2D col in results)
		{
			if (col != null && col.tag == "Barrier") Destroy(col.gameObject);
			else if (col == null) break;
		}
	}
}
