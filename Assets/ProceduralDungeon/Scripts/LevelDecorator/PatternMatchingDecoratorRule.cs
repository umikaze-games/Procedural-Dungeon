using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[Serializable]
[CreateAssetMenu(fileName = "DecoratorRule", menuName = "Custom/Procedual Generation/Pattern Decorator Rule")]
public class PatternMatchingDecoratorRule : BaseDecoratorRule
{
	[SerializeField] float prefabRotation = 0;
	[SerializeField] GameObject prefab;
	[SerializeField] Array2DWrapper<TileType> placement;
	[SerializeField] Array2DWrapper<TileType> fill;
	[SerializeField] bool centerHorizontally = false;
	[SerializeField] bool centerVertically = false;

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

		GameObject decoration = Instantiate(prefab, parent.transform);
		Vector3 currentRotation = decoration.transform.eulerAngles;
		decoration.transform.eulerAngles = currentRotation + new Vector3(0, prefabRotation, 0);

		Vector3 center = new Vector3(occurrence.x + placement.Width / 2.0f, 0, occurrence.y + placement.Height / 2.0f);
		int scale = SharedLevelData.Instance.Scale;
		decoration.transform.position = (center + new Vector3(-1, 0, -1)) * scale;
		decoration.transform.localScale = Vector3.one * scale;
	}

	private Vector2Int[] FindOccurrences(TileType[,] levelDecorated, Room room)
	{
		List<Vector2Int> occurrences = new List<Vector2Int>();
		int centerX = room.Area.position.x + room.Area.width / 2 - placement.Width / 2;
		int centerY = room.Area.position.y + room.Area.height / 2 - placement.Height / 2;

		for (int y = room.Area.position.y - 1; y < room.Area.position.y + room.Area.height + 2 - placement.Height; y++)
		{
			for (int x = room.Area.position.x - 1; x < room.Area.position.x + room.Area.width + 2 - placement.Width; x++)
			{
				if (centerHorizontally && x != centerX)
				{
					continue;
				}
				if (centerVertically && y != centerY)
				{
					continue;
				}

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
