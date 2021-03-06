﻿using System.Collections;
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
			GameObject laser = Instantiate (LaserModel, this.transform.localPosition, Quaternion.identity);
			laser.GetComponent<LineRenderer> ().SetPosition (0, this.transform.localPosition);
			laser.GetComponent<LineRenderer> ().SetPosition (1, this.transform.localPosition);
		}

		base.Attack ();
	}
}
