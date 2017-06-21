using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetConnectivityScreen : MonoBehaviour
{
	List<Indicator> indicators;

	void Awake ()
	{
		indicators = new List<Indicator> ();

		foreach (Transform child in this.transform) {
			if (child.GetComponent<Indicator> () != null) {
				indicators.Add (child.GetComponent<Indicator> ());
			}
		}
	}

	public void UpdateUI (Target target)
	{
		int index = (int)target.PositionIndex;

		indicators [index].ToggleStar (target.IsStarSpawned);

		if (target.IsAlive) {
			indicators [index].ShowIndicator (Color.green);
		} else if (target.IsConnected) {
			indicators [index].ShowIndicator (Color.yellow);
		} else {
			indicators [index].ShowIndicator (Color.red);
		}
	}
}