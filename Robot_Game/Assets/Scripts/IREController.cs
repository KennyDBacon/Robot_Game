using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class IREController : MonoBehaviour
{
	public GameObject TargetGameObject;

	bool isConnected = false;
	string controllerID = "";

	List<Target> targets;
	List<GameObject> projectiles;

	float distanceFromCamera = 25.0f;
	float widthOffset = 20.0f;
	float heightOffset = 20.0f;

	float snapTargetAttackInterval;
	float roboticArmAttackInterval;

	SerialPort port;

	float spawnTimer;

	bool isReading;

	void Start ()
	{
		projectiles = new List<GameObject> ();

		SetupTarget ();

		CommandConstructor ();

		try {
			port = new SerialPort ("\\\\.\\COM1", 115200);
			port.ReadTimeout = 20;
			port.WriteTimeout = 400;
			port.Open ();
			print ("Connected to COM1");
		} catch {
			print ("IRE not connected to COM 1");
		}

		if (port.IsOpen) {
			ResetCommand ();
			SendCommand ();

			StartCoroutine (PortReader ());
		}

		spawnTimer = GameManager.GameModeManager.SpawnInterval;
	}

	void Update ()
	{
		if (isConnected & GameManager.GameModeManager.IsGameRunning) {
			SpawnHandler ();
		}

		if (!isReading) {
			if (port != null) {
				if (port.IsOpen) {
					StartCoroutine (PortReader ());
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.S)) {
			if (AttackingTargets.Count > 0) {
				int index = Random.Range (0, AttackingTargets.Count);
				AttackingTargets [index].SetCommand = 0;
				SendCommand ();
			}
		}
	}

	void SetupTarget ()
	{
		targets = new List<Target> ();
		List<Vector3> targetPositions = new List<Vector3> ();

		Vector2 screenSize = new Vector2 (Screen.width, Screen.height);

		targetPositions.Add (new Vector3 (-widthOffset, screenSize.y + heightOffset, distanceFromCamera));
		targetPositions.Add (new Vector3 (-widthOffset, screenSize.y + heightOffset, distanceFromCamera));
		targetPositions.Add (new Vector3 (screenSize.x / 2, screenSize.y + heightOffset, distanceFromCamera));
		targetPositions.Add (new Vector3 (screenSize.x / 4 * 3, screenSize.y + heightOffset, distanceFromCamera));
		targetPositions.Add (new Vector3 (screenSize.x + widthOffset, screenSize.y + heightOffset, distanceFromCamera));
		targetPositions.Add (new Vector3 (-widthOffset, -heightOffset, distanceFromCamera));
		targetPositions.Add (new Vector3 (screenSize.x / 4, -heightOffset, distanceFromCamera));
		targetPositions.Add (new Vector3 (screenSize.x / 2, -heightOffset, distanceFromCamera));
		targetPositions.Add (new Vector3 (screenSize.x / 4 * 3, -heightOffset, distanceFromCamera));
		targetPositions.Add (new Vector3 (screenSize.x + widthOffset, -heightOffset, distanceFromCamera));

		snapTargetAttackInterval = GameManager.GameModeManager.CurrentDifficulty.ST_AttackInterval;
		roboticArmAttackInterval = GameManager.GameModeManager.CurrentDifficulty.RA_AttackInterval;

		int index = 0;
		foreach (Vector3 position in targetPositions) {
			foreach (TargetStat targetStat in DataManager.TargetStats) {
				if (targetStat.PositionIndex == index) {
					GameObject target = Instantiate (TargetGameObject, Camera.main.ScreenToWorldPoint (position), Quaternion.identity);
					target.transform.parent = this.transform;

					// Name of GameObject
					target.name = targetStat.Label;

					// Attach the script of target's type and add to target list
					target.AddComponent (System.Type.GetType (targetStat.Type));
					targets.Add (target.GetComponent<Target> ());

					// Set attack interval based on the target's type
					float attackInterval;
					if (targetStat.Type.Equals ("SnapTarget")) {
						attackInterval = snapTargetAttackInterval;
					} else {
						attackInterval = roboticArmAttackInterval;
					}

					// Setup target
					// Provide ID of Target for data reference
					target.GetComponent<Target> ().Setup (targetStat.ID, position, attackInterval);
					break;
				}
			}

			index++;
		}
	}

	IEnumerator PortReader ()
	{
		try {
			isReading = true;

			if (port.IsOpen) {
				string line = "";

				try {
					line = port.ReadLine ();
				} catch {
					line = "";
				}

				if (!line.Equals ("")) {
					// Update status
					string[] data = line.Split (',');

					try {
						if (!isConnected) {
							isConnected = true;
							controllerID = data [1];

							ResetCommand ();
							SendCommand ();
						}

						foreach (Target target in targets) {
							target.UpdateStatus (int.Parse (data [target.StatusIndex]));
						}
					} catch (System.Exception e) {
						print ("[Error]" + e);
					}
				}
			}
		} catch (System.Exception e) {
			print ("[Ignore TimeOutException] " + e);
		}

		yield return new WaitForSeconds (0.2f);

		isReading = false;
	}

	void SpawnHandler ()
	{
		spawnTimer += Time.deltaTime;

		if (spawnTimer >= GameManager.GameModeManager.SpawnInterval) {
			spawnTimer = 0;

			switch (GameManager.GameModeManager.CurrentGameMode) {
			case GameModeManager.Mode.PvP:
				if (AvailableConnectedTargets.Count > 1) {
					List<Target> blueTeam = SpawnedBlueTargets;
					List<Target> redTeam = SpawnedRedTargets;

					if (blueTeam.Count < 5) {
						SpawnTarget (3);
					}

					if (redTeam.Count < 5) {
						SpawnTarget (1);
					}
				}
				break;
			case GameModeManager.Mode.PvE:
				SpawnTarget (GetTypeToSpawn);
				break;
			}

			SendCommand ();
		}
	}

	void SpawnTarget (int command)
	{
		if (AvailableConnectedTargets.Count > 0) {
			int index = Random.Range (0, AvailableConnectedTargets.Count);
			Target target = AvailableConnectedTargets [index];
			target.SpawnSequence (GetTeam (command), command);
		}
	}

	Target.Team GetTeam (int comm)
	{
		if (comm == 1) {
			return Target.Team.Red;
		} else if (comm == 2) {
			return Target.Team.None;
		} else if (comm == 3) {
			return Target.Team.Blue;
		}

		return Target.Team.None;
	}

	void ResetCommand ()
	{
		foreach (Target target in targets) {
			target.SetCommand = 0;
		}
	}

	void SendCommand ()
	{
		if (isConnected) {
			try {
				byte[] commands = CommandConstructor ();
				port.Write (commands, 0, commands.Length);
			} catch (System.Exception e) {
				print ("[Port Write] " + e);
			}
		}
	}

	byte[] CommandConstructor ()
	{
		string comm = string.Format ("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}",
			              "6",
			              controllerID,
			              "0",
			              FindTargetByCommandIndex (3).GetCommand.ToString (),
			              FindTargetByCommandIndex (4).GetCommand.ToString (),
			              FindTargetByCommandIndex (5).GetCommand.ToString (),
			              FindTargetByCommandIndex (6).GetCommand.ToString (),
			              "0",
			              "0",
			              FindTargetByCommandIndex (9).GetCommand.ToString (),
			              FindTargetByCommandIndex (10).GetCommand.ToString (),
			              FindTargetByCommandIndex (11).GetCommand.ToString (),
			              FindTargetByCommandIndex (12).GetCommand.ToString (),
			              FindTargetByCommandIndex (13).GetCommand.ToString (),
			              FindTargetByCommandIndex (14).GetCommand.ToString (),
			              ((int)GameManager.GameModeManager.CurrentGameMode).ToString ());

		print (comm);
		byte[] commandBytes = System.Text.Encoding.UTF8.GetBytes (comm);
		return commandBytes;
	}

	public void AddToProjectiles (GameObject proj)
	{
		projectiles.Add (proj);
	}

	public void RemoveFromProjectiles (GameObject proj)
	{
		int index = projectiles.IndexOf (proj);
		projectiles.RemoveAt (index);
		Destroy (proj);
	}

	public void Clear ()
	{
		ResetCommand ();
		SendCommand ();

		foreach (Target target in Targets) {
			target.Reset ();
		}

		for (int i = projectiles.Count - 1; i >= 0; --i) {
			Destroy (projectiles [i]);
		}

		projectiles.Clear ();
	}

	public Target FindTargetByCommandIndex (int commandIndex)
	{
		foreach (Target target in targets) {
			if (target.CommandIndex == commandIndex) {
				return target;
			}
		}

		return null;
	}

	void OnApplicationQuit ()
	{
		if (port != null) {
			if (port.IsOpen) {
				ResetCommand ();
				SendCommand ();

				port.Close ();
			}
		}
	}

	public List<Target> Targets {
		get { return targets; }
	}

	public List<Target> AvailableConnectedTargets {
		get {
			List<Target> temp = new List<Target> ();
			foreach (Target target in targets) {
				if (target.IsConnected && !target.IsAlive) {
					temp.Add (target);
				}
			}

			return temp;
		}
	}

	public List<Target> SpawnedBlueTargets {
		get {
			List<Target> temp = new List<Target> ();
			foreach (Target target in targets) {
				if (target.IsConnected && target.IsAlive && target.CurrentTeam == Target.Team.Blue) {
					temp.Add (target);
				}
			}

			return temp;
		}
	}

	public List<Target> SpawnedRedTargets {
		get {
			List<Target> temp = new List<Target> ();
			foreach (Target target in targets) {
				if (target.IsConnected && target.IsAlive && target.CurrentTeam == Target.Team.Red) {
					temp.Add (target);
				}
			}

			return temp;
		}
	}

	public List<Target> AttackingTargets {
		get {
			List<Target> temp = new List<Target> ();
			foreach (Target target in targets) {
				if (target.IsAlive) {
					temp.Add (target);
				}
			}

			return temp;
		}
	}

	public int GetTypeToSpawn {
		get {
			float midThreshold = Mathf.Lerp (0.35f, 0.1f, GameManager.GameModeManager.GetDiversityRate);

			print (midThreshold);

			float value = Random.value;
			if (value <= 0.5f - midThreshold) {
				return 1;
			} else if (value >= 0.5f + midThreshold) {
				return 3;
			} else {
				return 4;
			}
		}
	}
}