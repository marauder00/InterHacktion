using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("Users")]
public class User {
	[DynamoDBHashKey]
	public string Name { get; set; }
	[DynamoDBProperty]
	public string Team { get; set; }
	[DynamoDBProperty("Specialty")]
	public List<string> Specialty { get; set; }
	[DynamoDBProperty("Hackathons")]
	public List<string> Hackathons { get; set; }
	[DynamoDBProperty]
	public Location Location {get; set; }
	[DynamoDBProperty]
	public string Idea { get; set; }
	public User(string name, string team, List<string> specialty,
		List<string> hackathons, Location location, string idea) {
		this.Name = name;
		this.Team = team;
		this.Specialty = specialty;
		this.Hackathons = hackathons;
		this.Location = location;
		this.Idea = idea;
	}
}

public enum Specialty {
	UX,
	BE,
	FE,
	FS
}

public class Location {
	public double latitude { get; set; }
	public double longitude { get; set; }
	public Location(double x, double y) {
		latitude = x; 
		longitude = y;
	}
}