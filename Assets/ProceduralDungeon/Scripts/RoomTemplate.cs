using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[Serializable]
public class RoomTemplate
{
	[SerializeField] String name;
	[SerializeField] int numberOfRooms;
	[SerializeField] int roomWidthMin = 3;
	[SerializeField] int roomWidthMax = 5;
	[SerializeField] int roomLengthMin = 3;
	[SerializeField] int roomLengthMax = 5;

	public int NumberOfRooms => numberOfRooms;
	public int RoomWidthMin => roomWidthMin;
	public int RoomWidthMax => roomWidthMax;
	public int RoomLengthMin => roomLengthMin;
	public int RoomLengthMax => roomLengthMax;

}
