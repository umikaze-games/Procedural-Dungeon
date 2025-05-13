using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName ="Room Level Layout",menuName ="Custom/Procedual Generation/RoomLevelLayoutConfig")]
public class RoomLayoutConfig : ScriptableObject
{
	[SerializeField] int width = 64;
	[SerializeField] int length = 64;

	[SerializeField] int doorDistanceFromEdge = 1;
	[SerializeField] int minHallwayLength = 3;
	[SerializeField] int maxHallwayLength = 5;
	[SerializeField] int maxRoomCount = 10;
	[SerializeField] int minRoomDistance = 1;

	[SerializeField] RoomTemplate[] roomTemplates;

	public int Width => width;
	public int Length => length;
	public int DoorDistanceFromEdge => doorDistanceFromEdge;
	public int MinHallwayLength => minHallwayLength;
	public int MaxHallwayLength => maxHallwayLength;
	public int MaxRoomCount => maxRoomCount;
	public int MinRoomDistance => minRoomDistance;

	public RoomTemplate[] RoomTemplates => roomTemplates;

	public Dictionary<RoomTemplate, int> GetAvailableRooms()
	{
		var availableRooms = new Dictionary<RoomTemplate, int>();
		for (int i = 0; i < RoomTemplates.Length; i++)
		{
			availableRooms.Add(RoomTemplates[i], RoomTemplates[i].NumberOfRooms);
		}
		// Remove all availableRooms entries where the count of available rooms of that type is 0
		availableRooms = availableRooms.Where(kvp => kvp.Value > 0).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
		return availableRooms;
	}


}
