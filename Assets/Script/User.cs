using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class User {
	public string name { get; set; }
	public string team { get; set; }
	public List<string> specialty { get; set; }
	public List<string> hackathons { get; set; }
	public Location location {get; set; }
	public string idea { get; set; }
	public User(string name, string team, List<string> specialty,
		List<string> hackathons, Location location, string idea) {
		this.name = name;
		this.team = team;
		this.specialty = specialty;
		this.hackathons = hackathons;
		this.location = location;
		this.idea = idea;
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