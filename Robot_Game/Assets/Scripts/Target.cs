using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
	public bool IsUpdateIndicator;

	/*
	 * Command
	 * Activate: 1
	 * Deactivate: 0
	 */
	int command;

	/*
	 * Status
	 * Activated: 1
	 * Deactivate: 0
	 * Not connected: -1
	 * Drone taking off: -2
	 */
	int currStatus;
	int prevStatus;

	string label;
	IREController.TargetIndex index;
	Category type;
	bool isActive;

	Vector3 screenPosition;

	bool isOffsetTimer;
	protected float attackInterval;
	protected float attackTimer;
	protected float attackTimerOffset;

	float respawnInterval;
	float respawnTimer;

	public enum Category
	{
		SnapTarget,
		RoboticArm
	}

	public void Setup (IREController.TargetIndex targetIndex, string lbl, Category t, bool isAct, Vector3 pos, float interval, bool isTimeOffset)
	{
		index = targetIndex;
		label = lbl;
		type = t;
		isActive = isAct;

		screenPosition = pos;

		isOffsetTimer = isTimeOffset;
		attackInterval = interval;

		Reset ();
	}

	void Update ()
	{
		if (IsAlive && isActive) {
			Attack ();
		}
	}

	protected virtual void Attack ()
	{
		if (GameManager.GameModeManager.IsGameRunning) {
			if (IsPerformAttack) {
				attackTimer = 0.0f;

				if (isOffsetTimer) {
					attackTimerOffset = Random.Range (-0.1f, 0.1f);
				} else {
					attackTimerOffset = 0.0f;
				}
				return;
			}

			attackTimer += Time.deltaTime;
		}
	}


	bool isReset;

	public bool UpdateStatus (int stat)
	{
		if (!currStatus.Equals (stat)) {
			IsUpdateIndicator = true;
			prevStatus = currStatus;
			currStatus = stat;

			if (prevStatus > 0 && currStatus <= 0) {
				command = 9;

				GameManager.GameModeManager.KillTarget ();
			}

			return true;
		}

		return false;
	}

	public void Reset ()
	{
		prevStatus = -1;
		currStatus = -1;

		command = 9;

		attackTimer = 0.0f;
		attackTimerOffset = Random.Range (-0.1f, 0.1f);
	}

	public string GetConnectivity {
		get {
			if (IsConnected) {
				return "Connected";
			} else {
				return "Not Connected";
			}
		}
	}

	public string Label {
		get { return label; }
	}

	public IREController.TargetIndex TargetIndex {
		get { return index; }
	}

	public int GetCommand {
		get {
			int comm = command;
			command = 9;
			return comm;
		}
	}

	public int SetCommand {
		set { command = value; }
	}

	public bool IsConnected {
		get {
			if (currStatus >= 0 && isActive) {
				return true;
			}

			return false;
		}
	}

	public bool IsAlive {
		get {
			if (currStatus >= 1 && isActive) {
				return true;
			}

			return false;
		}
	}

	protected bool IsPerformAttack {
		get {
			if (attackTimer >= attackInterval + attackTimerOffset) {
				return true;
			}

			return false;
		}
	}
}