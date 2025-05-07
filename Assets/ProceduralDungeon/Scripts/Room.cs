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
		hallwayCandidates.Add(new Hallway(new Vector2Int(0, 0)));
		hallwayCandidates.Add(new Hallway(new Vector2Int(width, length)));
		return hallwayCandidates;
	}
}
