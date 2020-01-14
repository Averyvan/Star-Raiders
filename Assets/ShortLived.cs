using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortLived : MonoBehaviour
{
	float begin;
	public float timeToLive = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
		begin = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
		if (Time.time - begin > timeToLive) Destroy(gameObject);
    }
}
