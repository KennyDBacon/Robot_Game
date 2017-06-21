using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningProjectile : Projectile
{
	protected override void Start ()
	{
		base.Start ();

		speed = 12.0f;
		this.transform.LookAt (GameManager.Center);
	}

	protected override void Update ()
	{
		base.Update ();

		if (GameManager.GameModeManager.IsGameRunning) {
			this.transform.Rotate (Vector3.up * 12.0f);
		}
	}
}
