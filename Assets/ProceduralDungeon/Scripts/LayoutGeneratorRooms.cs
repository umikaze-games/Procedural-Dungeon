using System.Collections;
using System.Collections.Generic;
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

	[ContextMenu("Generate Level Layout")]
	public void GenerateLevel()
	{
		random = new System.Random();
		openDoorways = new List<Hallway>();

		RectInt roomRect = GetStartRoomRect();
		Debug.Log(roomRect);
		Room room = new Room(roomRect);
		List<Hallway> hallways = room.CalculateAllPossibleDoorways(room.Area.width, room.Area.height, 1);
		foreach (Hallway h in hallways)
		{
			h.StartRoom = room;
		}
		hallways.ForEach(h => openDoorways.Add(h));
		DrawLayout(roomRect);
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


	void DrawLayout(RectInt roomCandidateRect = new RectInt())
	{
		Renderer renderer = levelLayoutDisplay.GetComponent<Renderer>();

		Texture2D layoutTexture = (Texture2D)renderer.sharedMaterial.mainTexture;

		layoutTexture.Reinitialize(width, length);
		levelLayoutDisplay.transform.localScale = new Vector3(width, length, 1);
		layoutTexture.FillWithColor(Color.black);
		layoutTexture.DrawRectangle(roomCandidateRect, Color.blue);
		foreach (Hallway hallway in openDoorways)
		{
			layoutTexture.SetPixel(
				hallway.StartPositionAbsolute.x,
				hallway.StartPositionAbsolute.y,
				hallway.StartDirection.GetColor()
			);
		}
		layoutTexture.SaveAsset();
	}
}
