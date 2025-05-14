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
	[SerializeField] Texture2D layoutTexture;

	public int NumberOfRooms => numberOfRooms;
	public int RoomWidthMin => roomWidthMin;
	public int RoomWidthMax => roomWidthMax;
	public int RoomLengthMin => roomLengthMin;
	public int RoomLengthMax => roomLengthMax;
	public Texture2D LayoutTexture => layoutTexture;

	public RectInt GenerateRoomCandidateRect(Random random)
	{
		if (layoutTexture != null)
		{
			return new RectInt { width = layoutTexture.width, height = layoutTexture.height };
		}

		RectInt roomCandidateRect = new RectInt
		{
			width = random.Next(roomWidthMin, roomWidthMax),
			height = random.Next(roomLengthMin, roomLengthMax)
		};
		return roomCandidateRect;
	}


}
