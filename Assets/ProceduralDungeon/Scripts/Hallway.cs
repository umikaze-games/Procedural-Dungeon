using UnityEngine;

public class Hallway
{
	private Vector2Int startPos;
	private Vector2Int endPos;
	private Room startRoom;//вСоб╫г
	private Room endRoom;//срио╫г

	public Room StartRoom { get { return startRoom; } set { this.startRoom = value; } }
	public Room EndRoom { get { return endRoom; } set { this.endRoom = value; } }

	public Vector2Int StartPositionAbsolute { get { return startPos + startRoom.Area.position; } }
	public Vector2Int EndPositionAbsolute { get { return endPos + endRoom.Area.position; } }

	public Hallway(Vector2Int startPos,Room startRoom=null)
	{
		this.startPos = startPos;
		this.startRoom = startRoom;
	}
}
