using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LayoutGeneratorRooms : MonoBehaviour
{
	[SerializeField] int width = 64;
	[SerializeField] int length = 64;

	[SerializeField] int roomWidthMin = 3;
	[SerializeField] int roomWidthMax = 5;
	[SerializeField] int roomLengthMin = 3;
	[SerializeField] int roomLengthMax = 5;

	[SerializeField] GameObject levelLayoutDisplay;
	[SerializeField] List<Hallway> openDoorways;

	System.Random random;
	private Level level;

	[ContextMenu("Generate Level Layout")]
	public void GenerateLevel()
	{
		random = new System.Random();
		openDoorways = new List<Hallway>();
		level = new Level(width, length);
		RectInt roomRect = GetStartRoomRect();
		Debug.Log(roomRect);
		Room room = new Room(roomRect);
		List<Hallway> hallways = room.CalculateAllPossibleDoorways(room.Area.width, room.Area.height, 1);
		foreach (Hallway h in hallways)
		{
			h.StartRoom = room;
		}
		hallways.ForEach(h => openDoorways.Add(h));

		level.AddRoom(room);

		//Room testRoom1 = new Room(new RectInt(3, 6, 6, 10));
		//Room testRoom2 = new Room(new RectInt(15, 4, 10, 12));
		//Hallway testHallway = new Hallway(HallwayDirection.Right, new Vector2Int(6, 3), testRoom1);
		//testHallway.EndPosition = new Vector2Int(0, 5);
		//testHallway.EndRoom = testRoom2;
		//level.AddRoom(testRoom1);
		//level.AddRoom(testRoom2);
		//level.AddHallway(testHallway);
		Hallway selectedEntryway = openDoorways[random.Next(0, openDoorways.Count)];
		Hallway selectedExit = SelectHallwayCandidate(new RectInt(0, 0, 5, 7), selectedEntryway);
		Debug.Log(selectedExit.StartPosition);
		Debug.Log(selectedExit.StartDirection);

		DrawLayout(selectedEntryway, roomRect);
	}

	RectInt GetStartRoomRect()
	{
		int roomWidth = random.Next(roomWidthMin, roomWidthMax);
		int availableWidthX = width / 2 - roomWidth;
		int randomX = random.Next(0, availableWidthX);
		int roomX = randomX + width / 4;

		int roomLength = random.Next(roomLengthMin, roomLengthMax);
		int availableLengthY = length / 2 - roomLength;
		int randomY = random.Next(0, availableLengthY);
		int roomY = randomY + length / 4;

		return new RectInt(roomX, roomY, roomWidth, roomLength);
	}


	void DrawLayout(Hallway selectedEntryway = null, RectInt roomCandidateRect = new RectInt())
	{
		Renderer renderer = levelLayoutDisplay.GetComponent<Renderer>();

		Texture2D layoutTexture = (Texture2D)renderer.sharedMaterial.mainTexture;

		layoutTexture.Reinitialize(width, length);
		levelLayoutDisplay.transform.localScale = new Vector3(width, length, 1);
		layoutTexture.FillWithColor(Color.black);

		Array.ForEach(level.Rooms, room => layoutTexture.DrawRectangle(room.Area, Color.white));
		Array.ForEach(level.Hallways, hallway => layoutTexture.DrawLine(hallway.StartPositionAbsolute, hallway.EndPositionAbsolute, Color.white));

		layoutTexture.DrawRectangle(roomCandidateRect, Color.blue);
		foreach (Hallway hallway in openDoorways)
		{
			layoutTexture.SetPixel(
				hallway.StartPositionAbsolute.x,
				hallway.StartPositionAbsolute.y,
				hallway.StartDirection.GetColor()
			);
		}
		if (selectedEntryway != null)
		{
			layoutTexture.SetPixel(selectedEntryway.StartPositionAbsolute.x, selectedEntryway.StartPositionAbsolute.y, Color.red);
		}

		layoutTexture.SaveAsset();
	}

	Hallway SelectHallwayCandidate(RectInt roomCandidateRect, Hallway entryway)
	{
		Room room = new Room(roomCandidateRect);
		List<Hallway> candidates = room.CalculateAllPossibleDoorways(room.Area.width, room.Area.height, 1);
		HallwayDirection requiredDirection = entryway.StartDirection.GetOppositeDirection();
		List<Hallway> filteredHallwayCandidates = candidates.Where(hc => hc.StartDirection == requiredDirection).ToList();
		return filteredHallwayCandidates.Count > 0 ? filteredHallwayCandidates[random.Next(filteredHallwayCandidates.Count)] : null;
	}

}
