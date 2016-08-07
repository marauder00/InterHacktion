using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
public class UserManagement : MonoBehaviour {

	public User currentUser;
	public List<User> otherUsers = new List<User>();
	public GameObject currentObject;
	public GameObject objectToClone;
	const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
	string[] hackathons = { "HackDuke", "HackPSU", "HackTheNorth", "Hack The Planet" };
	// Use this for initialization
	void Start () {
		var credentials = new CognitoAWSCredentials("arn:aws:dynamodb:us-west-2:456798324465:table/Users, RegionEndpoint.USWest1");
		AmazonDynamoDBClient client = new AmazonDynamoDBClient(credentials);
		DynamoDBContext context = new DynamoDBContext(client);

		var scanRequest = new ScanRequest {
			TableName = "Users"
		};

		var response = client.ScanAsync (scanRequest, (results) => {
			foreach(Dictionary<string, AttributeValue> item in results.Response.Items.
				Where(x=> x["Name"].Equals(currentUser.Name)==false)) {
				var doc = Document.FromAttributeMap(item);
				var myUser = context.FromDocument<User>(doc);
				otherUsers.Add(myUser);
			}
		});
		createCurrentUser ();
		createTestUsers ();
	}
	
	// Update is called once per frame
	void Update () {
		foreach(User user in otherUsers.Where(x=> GameObject.Find(x.Name) == null && x.Hackathons.Intersect(currentUser.Hackathons).Count()!=0)) {
			GameObject g = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			g.name = user.Name;
			g.transform.parent = currentObject.transform;
			var latDiff = (currentUser.Location.latitude - user.Location.latitude);
			var longDiff = (currentUser.Location.longitude - user.Location.longitude);
			var yOffset = latDiff * 111111;
			var xOffset = longDiff * 111111 * Mathf.Cos ((float)latDiff);
			Vector3 position = new Vector3 ((float)xOffset, 0, (float)yOffset);
			position.Scale (new Vector3 (.01f, .01f, .01f));
			g.transform.position = position;
			g.transform.parent = null;
		}
	}

	void createTestUsers() {
		int number = UnityEngine.Random.Range (1, 10);
		var random = new System.Random ();
		var currentUserLocation = currentUser.Location;
		for (int i = 0; i < number; i++) {
			var yChange = UnityEngine.Random.Range (0, .01f);
			var xChange = UnityEngine.Random.Range (0, .01f);
			xChange = UnityEngine.Random.Range (1, 3) == 1 ? xChange : -xChange;
			yChange = UnityEngine.Random.Range (1, 3) == 1 ? yChange : -yChange;
			var newLocation = new Location (currentUserLocation.latitude + xChange, currentUserLocation.longitude + yChange);
			var name = new String(Enumerable.Repeat (chars, random.Next (15)).Select (s => s [random.Next (s.Length)]).ToArray ());
			List<String> specialty = new List<string> ();
			specialty.Add ("BE");
			var hackathon = hackathons [random.Next (hackathons.Length)];
			var hackathonList = new List<String> ();
			hackathonList.Add (hackathon);
			var idea = new String(Enumerable.Repeat (chars, random.Next (15)).Select (s => s [random.Next (s.Length)]).ToArray ());
			otherUsers.Add (new User (name, "", specialty, hackathonList, newLocation, idea));
		}

	}

	void createCurrentUser() {
		var yChange = UnityEngine.Random.Range (0, 100);
		var xChange = UnityEngine.Random.Range (0, 100);
		xChange = UnityEngine.Random.Range (1, 3) == 1 ? xChange : -xChange;
		yChange = UnityEngine.Random.Range (1, 3) == 1 ? yChange : -yChange;
		var newLocation = new Location (xChange, yChange);
		var random = new System.Random ();
		var name = new String(Enumerable.Repeat (chars, random.Next (15)).Select (s => s [random.Next (s.Length)]).ToArray ());
		List<string> specialty = new List<string> ();
		specialty.Add ("FE");
		var hackathon = hackathons [random.Next (hackathons.Length)];
		var hackathonList = new List<String> ();
		hackathonList.Add (hackathon);
		var idea = new String(Enumerable.Repeat (chars, random.Next (15)).Select (s => s [random.Next (s.Length)]).ToArray ());
		currentUser =  (new User (name, "", specialty, hackathonList, newLocation, idea));

	}

	static T RandomEnumValue<T> () {
		var v = Enum.GetValues(typeof(T));
		return (T)v.GetValue (new System.Random ().Next (v.Length));
	}
}
