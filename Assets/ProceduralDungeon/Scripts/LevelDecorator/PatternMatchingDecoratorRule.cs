using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[Serializable]
[CreateAssetMenu(fileName = "DecoratorRule", menuName = "Custom/Procedural Generation/Pattern Decorator Rule")]
public class PatternMatchingDecoratorRule : BaseDecoratorRule
{
	[SerializeField] GameObject prefab;
	[SerializeField] Array2DWrapper<TileType> placement;
	[SerializeField] Array2DWrapper<TileType> fill;

	internal override bool CanBeApplied(TileType[,] levelDecorated, Room room)
	{
		if (FindOccurrences(levelDecorated, room).Length > 0)
		{
			return true;
		}
		return false;
	}

	internal override void Apply(TileType[,] levelDecorated, Room room, Transform parent)
	{
		Vector2Int[] occurrences = FindOccurrences(levelDecorated, room);
		if (occurrences.Length == 0) { return; }
		Random random = SharedLevelData.Instance.Rand;
		int occurrenceIndex = random.Next(0, occurrences.Length);
		Vector2Int occurrence = occurrences[occurrenceIndex];
		for (int y = 0; y < placement.Height; y++)
		{
			for (int x = 0; x < placement.Width; x++)
			{
				TileType tileType = fill[x, y];
				levelDecorated[occurrence.x + x, occurrence.y + y] = tileType;
			}
		}

	}

	private Vector2Int[] FindOccurrences(TileType[,] levelDecorated, Room room)
	{
		List<Vector2Int> occurrences = new List<Vector2Int>();
		for (int y = room.Area.position.y - 1; y < room.Area.position.y + room.Area.height + 2 - placement.Height; y++)
		{
			for (int x = room.Area.position.x - 1; x < room.Area.position.x + room.Area.width + 2 - placement.Width; x++)
			{
				if (IsPatternAtPosition(levelDecorated, placement, x, y))
				{
					occurrences.Add(new Vector2Int(x, y));
				}
			}
		}
		return occurrences.ToArray();
	}

	bool IsPatternAtPosition(TileType[,] levelDecorated, Array2DWrapper<TileType> pattern, int startX, int startY)
	{
		for (int y = 0; y < pattern.Height; y++)
		{
			for (int x = 0; x < pattern.Width; x++)
			{
				if (levelDecorated[startX + x, startY + y] != pattern[x, y])
				{
					return false;
				}
			}
		}
		return true;
	}

}
