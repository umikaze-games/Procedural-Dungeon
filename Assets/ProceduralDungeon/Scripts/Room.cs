using System.Collections.Generic;
using UnityEngine;

public class Room 
{
    private RectInt area;
	//RectInt(Vector2Int position, Vector2Int size);width height
	public RectInt Area  { get { return area; } }

    public Room( RectInt area) { this.area = area; }

	public List<Hallway> CalculateAllPossibleDoorways(int width, int length, int minDistanceFromEdge)
	{
		List<Hallway> hallwayCandidates = new List<Hallway>();

		int bottom = length - 1;
		int minX = minDistanceFromEdge;
		int maxX = width - minDistanceFromEdge;


		for (int x = minX; x < maxX; x++)
		{
			hallwayCandidates.Add(new Hallway(HallwayDirection.Bottom,new Vector2Int(x, 0)));
			hallwayCandidates.Add(new Hallway(HallwayDirection.Top,new Vector2Int(x, bottom)));
		}

		int right = width - 1;
		int minY = minDistanceFromEdge;
		int maxY = length - minDistanceFromEdge;

		for (int y = minY; y < maxY; y++)
		{
			hallwayCandidates.Add(new Hallway(HallwayDirection.Left,new Vector2Int(0, y)));
			hallwayCandidates.Add(new Hallway(HallwayDirection.Right,new Vector2Int(right, y)));
		}
		return hallwayCandidates;
	}
}
