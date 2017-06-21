using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
	/*
	 * Command
	 * Activate - White Light: 4
	 * Activate - Blue Light: 3
	 * Activate - Green Light: 2
	 * Activate - Red Light: 1
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

	Team team;

	int id;

	Vector3 screenPosition;

	protected float attackInterval;
	protected float attackTimer;

	float respawnInterval;
	float respawnTimer;

	bool isStarSpawned;
	bool isStarSpawnable;
	float starSpawnInterval = 1.0f;
	float starSpawnTimer;

	public enum Team
	{
		None,
		Blue,
		Red
	}

	public void Setup (int v_id, Vector3 pos, float interval)
	{
		team = Team.None;

		id = v_id;

		screenPosition = pos;
		attackInterval = interval;

		Reset ();
	}

	void Update ()
	{
		if (GameManager.GameModeManager.IsGameRunning) {
			//StarSpawnHandler ();

			if (IsAlive && IsActive) {
				Attack ();
			}
		}
	}

	public void UpdateStatus (int stat)
	{
		if (!currStatus.Equals (stat)) {
			prevStatus = currStatus;
			currStatus = stat;

			if (prevStatus > 0 && currStatus <= 0) {
				command = 9;

				GameManager.GameModeManager.AddScore (team, 10);

				isStarSpawned = false;
			}

			GameManager.UIManager.TargetConnectivityScreen.UpdateUI (this);
		}
	}

	protected virtual void Attack ()
	{
		if (IsAttackAllowed) {
			if (IsPerformAttack) {
				attackTimer = 0.0f;
				return;
			}

			attackTimer += Time.deltaTime;
		}
	}

	protected virtual void StarSpawnHandler ()
	{
		if (!isStarSpawned && Label.Equals ("Snap_Target_3")) {
			starSpawnTimer += Time.deltaTime;

			if (starSpawnTimer >= starSpawnInterval) {
				starSpawnTimer = 0.0f;
				isStarSpawnable = true;
			}
		}
	}

	public void SpawnSequence (Team t, int comm)
	{
		Reset ();
		//StarSpawnCheck ();

		command = comm;
		team = t;
	}

	public void StarSpawnCheck ()
	{
		if (GameManager.GameModeManager.CurrentGameMode != GameModeManager.Mode.PvP) {
			if (isStarSpawnable) {
				float rand = Random.value;

				if (rand <= 0.15f) {
					isStarSpawned = true;
					isStarSpawnable = false;

					GameManager.UIManager.TargetConnectivityScreen.UpdateUI (this);
				}
			}
		}
	}

	public void Reset ()
	{
		team = Team.None;

		prevStatus = -1;
		currStatus = -1;

		command = 9;

		attackTimer = 0.0f;
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
		get { return DataManager.TargetStats [id].Label; }
	}

	bool IsActive {
		get { return DataManager.TargetStats [id].IsActive; }
	}

	public int StatusIndex {
		get { return DataManager.TargetStats [id].StatusIndex; }
	}

	public int CommandIndex {
		get { return DataManager.TargetStats [id].CommandIndex; }
	}

	public int PositionIndex {
		get { return DataManager.TargetStats [id].PositionIndex; }
	}

	public int GetCommand {
		get {
			int comm = command;

			// Reset command once used
			command = 9;

			return comm;
		}
	}

	public int SetCommand {
		set { command = value; }
	}

	public bool IsConnected {
		get {
			if (currStatus >= 0 && IsActive) {
				return true;
			}

			return false;
		}
	}

	public bool IsAlive {
		get {
			if (currStatus >= 1 && IsActive) {
				return true;
			}

			return false;
		}
	}

	bool IsAttackAllowed {
		get {
			if (GameManager.GameModeManager.CurrentGameMode != GameModeManager.Mode.PvP) {
				return true;
			}

			return false;
		}
	}

	protected bool IsPerformAttack {
		get {
			if (attackTimer >= attackInterval) {
				return true;
			}

			return false;
		}
	}

	public bool IsStarSpawned {
		get { return isStarSpawned; }
	}

	public Team CurrentTeam {
		get { return team; }
	}
}