using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class AccountCreation : MonoBehaviour {
	public GameObject name;
	public GameObject specialty;
	public GameObject hackathons;
	private Location l;
	// Use this for initialization
	IEnumerator Start () {
		string strName = name.GetComponent<Text> ().text;
		string strSpecialty = specialty.GetComponent<Text> ().text;
		string strHackathons = hackathons.GetComponent<Text> ().text;
		if (Input.location.isEnabledByUser) {
			Input.location.Start ();
			int maxWait = 20;
			while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
				yield return new WaitForSeconds (1);
				maxWait--;
			}
			if (maxWait < 1 || Input.location.status == LocationServiceStatus.Failed)
				l = new Location (0, 0);
			else {
				l = new Location (Input.location.lastData.latitude, Input.location.lastData.longitude);
			}
		} else {
			l = new Location (0, 0);
		}
		List<string> specialties = new List<string> ();
		specialties.Add (strSpecialty);
		List<string> hackathonList = new List<string> ();
		hackathonList.Add (strHackathons);
		UserManagement.currentUser = new User (strName, "", specialties, hackathonList, l, "");


	}
	
	// Update is called once per frame
	void Update () {
		if (Input.location.isEnabledByUser) {
			if (Input.location.status == LocationServiceStatus.Running)
				gameObject.transform.position = new Vector3 ((float)(Input.location.lastData.latitude - l.latitude), 0, (float)(Input.location.lastData.longitude - l.longitude));
		}
	}
}
