using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
	public float Lifetime;
	float timer = 0.0f;

	void Update ()
	{
		timer += Time.deltaTime;

		if (timer >= Lifetime) {
			Destroy (this.gameObject);
		}
	}
}
