using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
	public float Lifetime;
	float timer = 0.0f;

	LineRenderer line;

	Vector3 shootDirection;

	float length;
	float speed;

	void Start ()
	{
		line = this.transform.GetComponent<LineRenderer> ();

		shootDirection = GameManager.Center.position - this.transform.position;
		shootDirection.Normalize ();

		length = 6.0f;
		speed = 30.0f;
	}

	void Update ()
	{
		FlyToPlayer ();
		HitDetection ();

		//timer += Time.deltaTime;

		//if (timer >= Lifetime) {
		//	Destroy (this.gameObject);
		//}
	}

	void FlyToPlayer ()
	{
		line.SetPosition (0, line.GetPosition (0) + (shootDirection * Time.deltaTime * speed));

		if (Vector3.Distance (line.GetPosition (0), line.GetPosition (1)) >= length) {
			line.SetPosition (1, line.GetPosition (1) + (shootDirection * Time.deltaTime * speed));
		}
	}

	void HitDetection ()
	{
		RaycastHit hit;
		if (Physics.Raycast (line.GetPosition (0), line.GetPosition (1) - line.GetPosition (0), out hit)) {
			if (hit.collider.tag.Equals ("Player")) {
				GameManager.Player.TakeDamage (1);
				Destroy (this.gameObject);
			}
		}
	}
}
