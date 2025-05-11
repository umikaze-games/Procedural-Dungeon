using UnityEngine;

public class Hallway
{
	private Vector2Int startPosition;
	private Vector2Int endPosition;

	private HallwayDirection startDirection;
	private HallwayDirection endDirection;

	private Room startRoom;//×óÏÂ½Ç
	private Room endRoom;//ÓÒÉÏ½Ç

	public Room StartRoom { get { return startRoom; } set { this.startRoom = value; } }
	public Room EndRoom { get { return endRoom; } set { this.endRoom = value; } }

	public HallwayDirection StartDirection { get { return startDirection; } }
	public HallwayDirection EndDirection { get { return endDirection; } set {endDirection=value; } }

	public Vector2Int StartPositionAbsolute { get { return startPosition + startRoom.Area.position; } }
	public Vector2Int EndPositionAbsolute { get { return endPosition + endRoom.Area.position; } }

	public Vector2Int StartPosition {
		get => startPosition;
		set => startPosition = value;
	}
	public Vector2Int EndPosition
	{
		get => endPosition;
		set => endPosition = value;
	}

	public RectInt Area
	{
		get
		{
			int x = Mathf.Min(StartPositionAbsolute.x, EndPositionAbsolute.x);
			int y = Mathf.Min(StartPositionAbsolute.y, EndPositionAbsolute.y);
			int width = Mathf.Max(1, Mathf.Abs(StartPositionAbsolute.x - EndPositionAbsolute.x));
			int height = Mathf.Max(1, Mathf.Abs(StartPositionAbsolute.y - EndPositionAbsolute.y));
			if (StartPositionAbsolute.x == EndPositionAbsolute.x)
			{
				y++;
				height--;
			}
			if (StartPositionAbsolute.y == EndPositionAbsolute.y)
			{
				x++;
				width--;
			}
			return new RectInt(x, y, width, height);
		}
	}

	public Hallway(HallwayDirection startDirection, Vector2Int startPos,Room startRoom=null)
	{
		this.startDirection = startDirection;
		this.startPosition = startPos;
		this.startRoom = startRoom;
	}
}
