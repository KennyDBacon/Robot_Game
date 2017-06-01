using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetConnectivityScreen : MonoBehaviour
{
	List<Indicator> indicators;

	bool isUpdating;

	void Awake ()
	{
		indicators = new List<Indicator> ();

		foreach (Transform child in this.transform) {
			if (child.GetComponent<Indicator> () != null) {
				indicators.Add (child.GetComponent<Indicator> ());
			}
		}
	}

	void Update ()
	{
		if (!isUpdating) {
			StartCoroutine (UpdateIndicator ());
		}
	}

	IEnumerator UpdateIndicator ()
	{
		isUpdating = true;

		int index = 0;
		foreach (Target target in GameManager.IREController.Targets) {
			if (target.IsUpdateIndicator) {
				target.IsUpdateIndicator = false;

				if (target.IsAlive) {
					indicators [index].ShowIndicator (Color.green);
				} else if (target.IsConnected) {
					indicators [index].ShowIndicator (Color.yellow);
				} else {
					indicators [index].ShowIndicator (Color.red);
				}
			}

			index++;
		}

		yield return new WaitForSeconds (0.1f);

		isUpdating = false;
	}

	public void SpawnIndication (Target target)
	{
		int index = GameManager.IREController.Targets.IndexOf (target);
		indicators [index].ArrowBlink ();
	}
}