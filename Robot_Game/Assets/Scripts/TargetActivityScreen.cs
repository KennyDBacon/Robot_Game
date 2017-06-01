using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetActivityScreen : MonoBehaviour
{
	public GameObject TargetInfobox;

	Transform content;

	void Start ()
	{
		content = this.transform.Find ("Scroll View").GetComponent<ScrollRect> ().content.transform;
	}

	void Update ()
	{
		UpdateInfobox ();
	}

	void AddInfoboxes ()
	{
		int count = 0;
		foreach (Target target in GameManager.IREController.Targets) {
			GameObject infobox = Instantiate (TargetInfobox);
			infobox.transform.parent = content;

			infobox.transform.localScale = Vector3.one;
			//infobox.transform.localPosition = new Vector2 (0, count * TargetInfobox.GetComponent<RectTransform> ().rect.height);

			infobox.GetComponent<RectTransform> ().offsetMin = new Vector2 (0, (count + 1) * -100.0f);
			infobox.GetComponent<RectTransform> ().offsetMax = new Vector2 (0, count * TargetInfobox.GetComponent<RectTransform> ().rect.height);

			infobox.transform.Find ("Name").GetComponent<Text> ().text = target.Label;
			infobox.transform.Find ("Status").GetComponent<Text> ().text = target.GetConnectivity;

			count++;
		}

		content.GetComponent<RectTransform> ().offsetMin = new Vector2 (0, count * TargetInfobox.GetComponent<RectTransform> ().rect.height);
	}

	void UpdateInfobox ()
	{
		if (content.childCount == 0 && GameManager.IREController.Targets.Count > 0) {
			AddInfoboxes ();
		} else {
			foreach (Target target in GameManager.IREController.Targets) {
				foreach (Transform child in content) {
					if (target.Label.Equals (child.transform.Find ("Name").GetComponent<Text> ().text)) {
						child.transform.Find ("Status").GetComponent<Text> ().text = target.GetConnectivity;
						break;
					}
				}
			}
		}
	}
}
