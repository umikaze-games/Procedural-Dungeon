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

	System.Random random;

	[ContextMenu("Generate Level Layout")]
	public void GenerateLevel()
	{
		random = new System.Random();
		RectInt roomRect = GetStartRoomRect();
		Debug.Log(roomRect);
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
}
