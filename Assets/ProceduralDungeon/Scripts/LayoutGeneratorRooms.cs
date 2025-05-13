using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Random=System.Random;

public class LayoutGeneratorRooms : MonoBehaviour
{
    [SerializeField] RoomLayoutConfig levelConfig;
    [SerializeField] int seed = Environment.TickCount;
	[SerializeField] GameObject levelLayoutDisplay;
    [SerializeField] List<Hallway> openDoorways;

	Random random;
    Level level;
	
    Dictionary<RoomTemplate, int> availableRooms;

	[ContextMenu("Generate Level Layout")]
    public void GenerateLevel() {
        random = new Random(seed);
		availableRooms = levelConfig.GetAvailableRooms();

		openDoorways = new List<Hallway>();
        level = new Level(levelConfig.Width, levelConfig.Length);
        //RectInt roomRect = GetStartRoomRect();
		
        RoomTemplate startRoomTemplate = availableRooms.Keys.ElementAt(random.Next(0, availableRooms.Count));
		RectInt roomRect = GetStartRoomRect(startRoomTemplate);

		Debug.Log(roomRect);
        Room room = new Room(roomRect);
        List<Hallway> hallways = room.CalculateAllPossibleDoorways(room.Area.width, room.Area.height, levelConfig.DoorDistanceFromEdge);
        hallways.ForEach (h => h.StartRoom = room);
        hallways.ForEach (h => openDoorways.Add(h));
        level.AddRoom(room);

        Hallway selectedEntryway = openDoorways[random.Next(0, openDoorways.Count)];
        AddRooms();
        DrawLayout(selectedEntryway, roomRect);
    }

	[ContextMenu("Generate new Seed")]
	public void GenerateNewSeed()
	{
		seed = Environment.TickCount;
	}

	[ContextMenu("Generate new Seed and Level")]
	public void GenerateNewSeedAndLevel()
	{
		GenerateNewSeed();
		GenerateLevel();
	}

	RectInt GetStartRoomRect(RoomTemplate roomTemplate)
	{
		int roomWidth = random.Next(roomTemplate.RoomWidthMin, roomTemplate.RoomWidthMax);
        int availableWidthX = levelConfig.Width / 2 - roomWidth;
        int randomX = random.Next(0, availableWidthX);
        int roomX = randomX + levelConfig.Width / 4;

        int roomLength = random.Next(roomTemplate.RoomLengthMin, roomTemplate.RoomLengthMax);
        int availableLengthY = levelConfig.Length / 2 - roomLength;
        int randomY = random.Next(0, availableLengthY);
		int roomY = randomY + levelConfig.Length / 4;

		return new RectInt(roomX, roomY, roomWidth, roomLength);
    }

    
    void DrawLayout(Hallway selectedEntryway = null, RectInt roomCandidateRect = new RectInt() ,bool isDebug = false) {
        Renderer renderer = levelLayoutDisplay.GetComponent<Renderer>();

        Texture2D layoutTexture = (Texture2D) renderer.sharedMaterial.mainTexture;

        layoutTexture.Reinitialize(levelConfig.Width, levelConfig.Length);
        levelLayoutDisplay.transform.localScale = new Vector3(levelConfig.Width, levelConfig.Length, 1);
        layoutTexture.FillWithColor(Color.black);

        Array.ForEach(level.Rooms, room => layoutTexture.DrawRectangle(room.Area, Color.white));
        Array.ForEach(level.Hallways, hallway => layoutTexture.DrawLine(hallway.StartPositionAbsolute, hallway.EndPositionAbsolute, Color.white));
        if (isDebug)
        {
            layoutTexture.DrawRectangle(roomCandidateRect, Color.blue);

            openDoorways.ForEach(hallway => layoutTexture.SetPixel(hallway.StartPositionAbsolute.x, hallway.StartPositionAbsolute.y, hallway.StartDirection.GetColor()));
        }

        if (isDebug && selectedEntryway != null)
        {
            layoutTexture.SetPixel(selectedEntryway.StartPositionAbsolute.x, selectedEntryway.StartPositionAbsolute.y, Color.red);
        }

        layoutTexture.SaveAsset();
    }

    Hallway SelectHallwayCandidate(RectInt roomCandidateRect, Hallway entryway)
    {
        Room room = new Room(roomCandidateRect);
        List<Hallway> candidates = room.CalculateAllPossibleDoorways(room.Area.width, room.Area.height, levelConfig.DoorDistanceFromEdge);
        HallwayDirection requiredDirection = entryway.StartDirection.GetOppositeDirection();
        List<Hallway> filteredHallwayCandidates = candidates.Where(hc => hc.StartDirection == requiredDirection).ToList();
        return filteredHallwayCandidates.Count > 0 ? filteredHallwayCandidates[random.Next(filteredHallwayCandidates.Count)] : null;
    }

    Vector2Int CalculateRoomPosition(Hallway entryway, int roomWidth, int roomLength, int distance, Vector2Int endPosition) {
        Vector2Int roomPosition = entryway.StartPositionAbsolute;
        switch (entryway.StartDirection) {
            case HallwayDirection.Left:
                roomPosition.x -= distance + roomWidth;
                roomPosition.y -= endPosition.y;
                break;
            case HallwayDirection.Top:
                roomPosition.x -= endPosition.x;
                roomPosition.y += distance + 1;
                break;
            case HallwayDirection.Right:
                roomPosition.x += distance + 1;
                roomPosition.y -= endPosition.y;
                break;
            case HallwayDirection.Bottom:
                roomPosition.x -= endPosition.x;
                roomPosition.y -= distance + roomLength;
                break;
        }
        return roomPosition;
    }

    Room ConstructAdjacentRoom(Hallway selectedEntryway)
    {
		RoomTemplate roomTemplate = availableRooms.Keys.ElementAt(random.Next(0, availableRooms.Count));
		RectInt roomCandidateRect = new RectInt {
			width = random.Next(roomTemplate.RoomWidthMin, roomTemplate.RoomWidthMax),
			height = random.Next(roomTemplate.RoomLengthMin, roomTemplate.RoomLengthMax)
		};

        Hallway selectedExit = SelectHallwayCandidate(roomCandidateRect, selectedEntryway);
        if (selectedExit == null) { return null; }
        int distance = random.Next(levelConfig.MinHallwayLength, levelConfig.MaxHallwayLength + 1);
        Vector2Int roomCandidatePosition = CalculateRoomPosition(selectedEntryway, roomCandidateRect.width, roomCandidateRect.height, distance, selectedExit.StartPosition);
        roomCandidateRect.position = roomCandidatePosition;

        if (!IsRoomCandidateValid(roomCandidateRect))
        {
            return null;
        }

        Room newRoom = new Room(roomCandidateRect);
        selectedEntryway.EndRoom = newRoom;
        selectedEntryway.EndPosition = selectedExit.StartPosition;
        return newRoom;
    }

	bool IsRoomCandidateValid(RectInt roomCandidateRect)
	{
		RectInt levelRect = new RectInt(1, 1, levelConfig.Width - 2, levelConfig.Length - 2);
		bool fullyInside =
			roomCandidateRect.xMin >= levelRect.xMin &&
			roomCandidateRect.yMin >= levelRect.yMin &&
			roomCandidateRect.xMax <= levelRect.xMax &&
			roomCandidateRect.yMax <= levelRect.yMax;

		return fullyInside && !CheckRoomOverlap(roomCandidateRect, level.Rooms, level.Hallways, levelConfig.MinRoomDistance);
	}

	bool CheckRoomOverlap(RectInt roomCandidateRect, Room[] rooms, Hallway[] hallways, int minRoomDistance)
    {
        RectInt paddedRoomRect = new RectInt {
            x = roomCandidateRect.x - minRoomDistance, 
            y = roomCandidateRect.y - minRoomDistance, 
            width = roomCandidateRect.width + 2 * minRoomDistance, 
            height = roomCandidateRect.height + 2 * minRoomDistance
        };
        foreach (Room room in rooms)
        {
            if (paddedRoomRect.Overlaps(room.Area))
            {
                return true;
            }
        }
        foreach (Hallway hallway in hallways)
        {
            if (paddedRoomRect.Overlaps(hallway.Area))
            {
                return true;
            }
        }
        return false;
    }

    void AddRooms()
    {
        while (openDoorways.Count > 0 && level.Rooms.Length < levelConfig.MaxRoomCount)
        {
            Hallway selectedEntryway = openDoorways[random.Next(0, openDoorways.Count)];
            Room newRoom = ConstructAdjacentRoom(selectedEntryway);

            if (newRoom == null)
            {
                openDoorways.Remove(selectedEntryway);
                continue;
            }

            level.AddRoom(newRoom);
            level.AddHallway(selectedEntryway);

            selectedEntryway.EndRoom = newRoom;
            List<Hallway> newOpenHallways = newRoom.CalculateAllPossibleDoorways(newRoom.Area.width, newRoom.Area.height, 1);
            newOpenHallways.ForEach(hallway => hallway.StartRoom = newRoom);

            openDoorways.Remove(selectedEntryway);
            openDoorways.AddRange(newOpenHallways);
        }
    }

}
