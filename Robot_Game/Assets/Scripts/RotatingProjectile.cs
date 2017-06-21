using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingProjectile : Projectile
{
	protected override void Start ()
	{
		base.Start ();

		speed = 6.5f;
		this.transform.eulerAngles = new Vector3 (Random.Range (0.0f, 180.0f), Random.Range (0.0f, 180.0f), Random.Range (0.0f, 180.0f));
	}

	protected override void Update ()
	{
		base.Update ();

		if (GameManager.GameModeManager.IsGameRunning) {
			this.transform.Rotate (Vector3.one * 4.0f);
		}
	}
}
