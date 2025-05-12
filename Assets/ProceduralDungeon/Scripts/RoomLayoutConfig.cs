using UnityEngine;

[CreateAssetMenu(fileName ="Room Level Layout",menuName ="Custom/Procedual Generation/RoomLevelLayoutConfig")]
public class RoomLayoutConfig : ScriptableObject
{
	[SerializeField] int width = 64;
	[SerializeField] int length = 64;

	[SerializeField] int roomWidthMin = 3;
	[SerializeField] int roomWidthMax = 5;
	[SerializeField] int roomLengthMin = 3;
	[SerializeField] int roomLengthMax = 5;
	[SerializeField] int doorDistanceFromEdge = 1;
	[SerializeField] int minHallwayLength = 3;
	[SerializeField] int maxHallwayLength = 5;
	[SerializeField] int maxRoomCount = 10;
	[SerializeField] int minRoomDistance = 1;

	public int Width => width;
	public int Length => length;
	public int RoomWidthMin => roomWidthMin;
	public int RoomWidthMax => roomWidthMax;
	public int RoomLengthMin => roomLengthMin;
	public int RoomLengthMax => roomLengthMax;
	public int DoorDistanceFromEdge => doorDistanceFromEdge;
	public int MinHallwayLength => minHallwayLength;
	public int MaxHallwayLength => maxHallwayLength;
	public int MaxRoomCount => maxRoomCount;
	public int MinRoomDistance => minRoomDistance;

}
