using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboticArm : Target
{
	public GameObject LaserModel;

	void Start ()
	{
		LaserModel = Resources.Load ("Laser") as GameObject;
	}

	protected override void Attack ()
	{
		if (IsPerformAttack) {
			GameObject laser = Instantiate (LaserModel);
			laser.GetComponent<LineRenderer> ().SetPosition (0, this.transform.position);
			laser.GetComponent<LineRenderer> ().SetPosition (1, GameManager.Center.position);

			GameManager.GameModeManager.TakeDamage ();
		}

		base.Attack ();
	}
}
