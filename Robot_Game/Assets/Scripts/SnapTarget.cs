using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTarget : Target
{
	Object[] Projectiles;

	void Awake ()
	{
		Projectiles = Resources.LoadAll ("Projectiles", typeof(GameObject));
	}

	protected override void Attack ()
	{
		if (IsPerformAttack) {
			int index = Random.Range (0, Projectiles.Length);
			Instantiate (Projectiles [index], this.transform.position, Quaternion.identity);
		}

		base.Attack ();
	}
}
