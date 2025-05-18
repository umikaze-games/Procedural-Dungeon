using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
	private int width;
	private int length;

	private List<Room> rooms;
	private List<Hallway> hallways;

	public int Width => width;
	public int Length => length;
	public Room[] Rooms => rooms.ToArray();
	public Hallway[] Hallways => hallways.ToArray();
	public Room PlayerStartRoom { get; set; }

	public Level(int width,int length)
	{
		this.width = width;
		this.length = length;
		rooms = new List<Room>();
		hallways = new List<Hallway>();
	}

	public void AddRoom(Room newRoom) => rooms.Add(newRoom);
	public void AddHallway(Hallway newHallway) => hallways.Add(newHallway);
}
