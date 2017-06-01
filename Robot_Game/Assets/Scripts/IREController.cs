using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class IREController : MonoBehaviour
{
	public GameObject TargetGameObject;

	bool isConnected = false;
	bool isReady = false;
	string controllerID = "";

	List<Target> targets;
	List<GameObject> projectiles;

	float distanceFromCamera = 25.0f;
	float widthOffset = 20.0f;
	float heightOffset = 20.0f;

	float snapTargetAttackInterval = 2.5f;
	float roboticArmAttackInterval = 1.0f;

	SerialPort port;
	Thread thread;

	float spawnInterval = 3.5f;
	float spawnTimer;

	enum DataIndex
	{
		ID = 1,
		Drone = 2,
		DroneStat = 3,
		ST1 = 4,
		ST2 = 5,
		Arm1 = 6,
		Arm2 = 7,
		Vest = 8,
		Mist = 9,
		MistStat = 10,
		ST3 = 11,
		ST4 = 12,
		ST5 = 13,
		ST6 = 14,
		ST7 = 15,
		ST8 = 16
	}

	public enum TargetIndex
	{
		SnapTarget_1 = 7,
		SnapTarget_2 = 1,
		SnapTarget_3 = 2,
		SnapTarget_4 = 3,
		SnapTarget_5 = 0,
		SnapTarget_6 = 4,
		SnapTarget_7 = 5,
		SnapTarget_8 = 9,
		RoboticArm_1 = 6,
		RoboticArm_2 = 8
	}

	void Awake ()
	{
		Vector2 screenSize = new Vector2 (Screen.width, Screen.height);

		targets = new List<Target> ();
		projectiles = new List<GameObject> ();

		TargetStatContainer tsc = TargetStatContainer.Load (Application.dataPath + "/StreamingAssets/EnemyStats.xml");

		// Top 1
		SetupTarget (TargetIndex.SnapTarget_5, "SnapTarget_5", Target.Category.SnapTarget, tsc.TargetStats [4].IsActive, new Vector3 (-widthOffset, screenSize.y + heightOffset, distanceFromCamera), snapTargetAttackInterval);
		// Top 2
		SetupTarget (TargetIndex.SnapTarget_2, "SnapTarget_2", Target.Category.SnapTarget, tsc.TargetStats [1].IsActive, new Vector3 (screenSize.x / 4, screenSize.y + heightOffset, distanceFromCamera), snapTargetAttackInterval);
		// Top 3
		SetupTarget (TargetIndex.SnapTarget_3, "SnapTarget_3", Target.Category.SnapTarget, tsc.TargetStats [2].IsActive, new Vector3 (screenSize.x / 2, screenSize.y + heightOffset, distanceFromCamera), snapTargetAttackInterval);
		// Top 4
		SetupTarget (TargetIndex.SnapTarget_4, "SnapTarget_4", Target.Category.SnapTarget, tsc.TargetStats [3].IsActive, new Vector3 (screenSize.x / 4 * 3, screenSize.y + heightOffset, distanceFromCamera), snapTargetAttackInterval);
		// Top 5
		SetupTarget (TargetIndex.SnapTarget_6, "SnapTarget_6", Target.Category.SnapTarget, tsc.TargetStats [5].IsActive, new Vector3 (screenSize.x + widthOffset, screenSize.y + heightOffset, distanceFromCamera), snapTargetAttackInterval);
		// Bottom 1
		SetupTarget (TargetIndex.SnapTarget_7, "SnapTarget_7", Target.Category.SnapTarget, tsc.TargetStats [6].IsActive, new Vector3 (-widthOffset, -heightOffset, distanceFromCamera), snapTargetAttackInterval);
		// Bottom 2
		SetupTarget (TargetIndex.RoboticArm_1, "RoboticArm_1", Target.Category.RoboticArm, tsc.TargetStats [8].IsActive, new Vector3 (screenSize.x / 4, -heightOffset, distanceFromCamera), roboticArmAttackInterval, false);
		// Bottom 3
		SetupTarget (TargetIndex.SnapTarget_1, "SnapTarget_1", Target.Category.SnapTarget, tsc.TargetStats [0].IsActive, new Vector3 (screenSize.x / 2, -heightOffset, distanceFromCamera), snapTargetAttackInterval);
		// Bottom 4
		SetupTarget (TargetIndex.RoboticArm_2, "RoboticArm_2", Target.Category.RoboticArm, tsc.TargetStats [9].IsActive, new Vector3 (screenSize.x / 4 * 3, -heightOffset, distanceFromCamera), roboticArmAttackInterval, false);
		// Bottom 5
		SetupTarget (TargetIndex.SnapTarget_8, "SnapTarget_8", Target.Category.SnapTarget, tsc.TargetStats [7].IsActive, new Vector3 (screenSize.x + widthOffset, -heightOffset, distanceFromCamera), snapTargetAttackInterval);

		CommandConstructor ();
	}

	void Start ()
	{
		try {
			port = new SerialPort ("\\\\.\\COM1", 115200);
			port.ReadTimeout = 20;
			port.WriteTimeout = 400;
			port.Open ();
		} catch {
			print ("IRE not connected to COM 1");
		}

		thread = new Thread (new ThreadStart (PortConnector));
		thread.Start ();

		spawnTimer = spawnInterval;
	}

	void Update ()
	{
		if (isConnected & GameManager.GameModeManager.IsGameRunning) {
			SpawnHandler ();
		}
	}

	void SetupTarget (TargetIndex index, string label, Target.Category type, bool isActive, Vector3 position, float attackInterval, bool isOffsetTimer = true)
	{
		GameObject target = Instantiate (TargetGameObject, Camera.main.ScreenToWorldPoint (position), Quaternion.identity);
		target.name = label;
		target.transform.parent = this.transform;

		if (type == Target.Category.SnapTarget) {
			target.AddComponent<SnapTarget> ();
		} else if (type == Target.Category.RoboticArm) {
			target.AddComponent<RoboticArm> ();
		}

		targets.Add (target.GetComponent<Target> ());

		target.GetComponent<Target> ().Setup (index, label, type, isActive, position, attackInterval, isOffsetTimer);

	}

	public bool IsUpdateConnectivity;

	void PortConnector ()
	{
		while (true) {
			try {
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
								controllerID = data [(int)DataIndex.ID];

								ResetCommand ();
								SendCommand ();
							}

							targets [(int)TargetIndex.SnapTarget_1].UpdateStatus (int.Parse (data [(int)DataIndex.ST1]));
							targets [(int)TargetIndex.SnapTarget_2].UpdateStatus (int.Parse (data [(int)DataIndex.ST2]));
							targets [(int)TargetIndex.RoboticArm_1].UpdateStatus (int.Parse (data [(int)DataIndex.Arm1]));
							targets [(int)TargetIndex.RoboticArm_2].UpdateStatus (int.Parse (data [(int)DataIndex.Arm2]));
							targets [(int)TargetIndex.SnapTarget_3].UpdateStatus (int.Parse (data [(int)DataIndex.ST3]));
							targets [(int)TargetIndex.SnapTarget_4].UpdateStatus (int.Parse (data [(int)DataIndex.ST4]));
							targets [(int)TargetIndex.SnapTarget_5].UpdateStatus (int.Parse (data [(int)DataIndex.ST5]));
							targets [(int)TargetIndex.SnapTarget_6].UpdateStatus (int.Parse (data [(int)DataIndex.ST6]));
							targets [(int)TargetIndex.SnapTarget_7].UpdateStatus (int.Parse (data [(int)DataIndex.ST7]));
							targets [(int)TargetIndex.SnapTarget_8].UpdateStatus (int.Parse (data [(int)DataIndex.ST8]));
						} catch {

						}
					}
				}
			} catch (System.Exception e) {
				print ("[Ignore TimeOutException] " + e);
			}

			Thread.Sleep (400);
		}
	}

	void SpawnHandler ()
	{
		spawnTimer += Time.deltaTime;

		if (spawnTimer >= spawnInterval) {
			spawnTimer = 0;

			List<Target> tempTargets = AvailableConnectedTargets;
			while (GameManager.GameModeManager.SpawnCount < tempTargets.Count) {
				int index = Random.Range (0, tempTargets.Count);
				tempTargets.RemoveAt (index);
			}

			foreach (Target target in tempTargets) {
				target.Reset ();
				//GameManager.UIManager.TargetConnectivityScreen.SpawnIndication (target);
				SetCommand (target.TargetIndex, 1);
			}

			SendCommand ();
		}
	}

	void SetCommand (TargetIndex targetIndex, int comm)
	{
		int index = (int)targetIndex;
		targets [index].SetCommand = comm;
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
		string comm = string.Format ("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}",
			              "6",
			              controllerID,
			              "0",
			              targets [(int)TargetIndex.SnapTarget_1].GetCommand.ToString (),
			              targets [(int)TargetIndex.SnapTarget_2].GetCommand.ToString (),
			              targets [(int)TargetIndex.RoboticArm_1].GetCommand.ToString (),
			              targets [(int)TargetIndex.RoboticArm_2].GetCommand.ToString (),
			              "0",
			              "0",
			              targets [(int)TargetIndex.SnapTarget_3].GetCommand.ToString (),
			              targets [(int)TargetIndex.SnapTarget_4].GetCommand.ToString (),
			              targets [(int)TargetIndex.SnapTarget_5].GetCommand.ToString (),
			              targets [(int)TargetIndex.SnapTarget_6].GetCommand.ToString (),
			              targets [(int)TargetIndex.SnapTarget_7].GetCommand.ToString (),
			              targets [(int)TargetIndex.SnapTarget_8].GetCommand.ToString ());

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

	void OnApplicationQuit ()
	{
		if (port != null) {
			if (port.IsOpen) {
				ResetCommand ();
				SendCommand ();

				port.Close ();
			}
		}

		if (thread != null) {
			if (thread.IsAlive) {
				thread.Abort ();
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
}