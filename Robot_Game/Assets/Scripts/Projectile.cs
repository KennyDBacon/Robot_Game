using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	protected float speed;

	protected virtual void Start ()
	{
		GameManager.IREController.AddToProjectiles (this.transform.root.gameObject);
	}

	protected virtual void Update ()
	{
		if (GameManager.GameModeManager.IsGameRunning) {
			this.transform.root.position = Vector3.MoveTowards (this.transform.root.position, GameManager.Center.position, Time.deltaTime * speed);
		}
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name.Equals ("PlayerCenter")) {
			GameManager.Player.TakeDamage (1);
			GameManager.IREController.RemoveFromProjectiles (this.transform.root.gameObject);
		}
	}
}
