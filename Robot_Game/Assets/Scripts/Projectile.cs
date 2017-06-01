using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	void Start ()
	{
		GameManager.IREController.AddToProjectiles (this.transform.root.gameObject);
		this.transform.eulerAngles = new Vector3 (Random.Range (0.0f, 180.0f), Random.Range (0.0f, 180.0f), Random.Range (0.0f, 180.0f));
	}

	void Update ()
	{
		if (!GameManager.UIManager.IsPaused) {
			this.transform.root.position = Vector3.MoveTowards (this.transform.root.position, GameManager.Center.position, Time.deltaTime * 8.0f);
			this.transform.Rotate (Vector3.one * 4.0f);
		}
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name.Equals ("PlayerCenter")) {
			GameManager.GameModeManager.TakeDamage ();
			GameManager.IREController.RemoveFromProjectiles (this.transform.root.gameObject);
		}
	}
}
