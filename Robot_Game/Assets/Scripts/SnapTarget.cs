using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTarget : Target
{
	public GameObject Projectile;

	void Start ()
	{
		Projectile = Resources.Load ("Grenade") as GameObject;
	}

	protected override void Attack ()
	{
		if (IsPerformAttack) {
			Instantiate (Projectile, this.transform.position, Quaternion.identity);
		}

		base.Attack ();
	}
}
