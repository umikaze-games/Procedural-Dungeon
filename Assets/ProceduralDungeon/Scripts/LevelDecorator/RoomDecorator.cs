using UnityEngine;
using Random = System.Random;
using System;
using System.Linq;
using System.Collections.Generic;

[Serializable]
public class RuleAvailability
{
	public BaseDecoratorRule rule;
	public int maxAvailability;

	public RuleAvailability(RuleAvailability other)
	{
		rule = other.rule;
		maxAvailability = other.maxAvailability;
	}
}

public class RoomDecorator : MonoBehaviour
{
	[SerializeField] GameObject parent;
	[SerializeField] LayoutGeneratorRooms layoutGenerator;
	[SerializeField] Texture2D levelTexture;
	[SerializeField] Texture2D decoratedTexture;

	[SerializeField] RuleAvailability[] availableRules;

	Random random;

	[ContextMenu("Place Items")]
	public void PlaceItemsFromMenu()
	{
		SharedLevelData.Instance.ResetRandom();
		Level level = layoutGenerator.GenerateLevel();
		PlaceItems(level);
	}

	public void PlaceItems(Level level)
	{
		random = SharedLevelData.Instance.Rand;
		Transform decorationsTransform = parent.transform.Find("Decorations");

		if (decorationsTransform == null)
		{
			GameObject decorationsGameObject = new GameObject("Decorations");
			decorationsTransform = decorationsGameObject.transform;
			decorationsTransform.SetParent(parent.transform);
		}
		else
		{
			decorationsTransform.DestroyAllChildren();
		}
		TileType[,] levelDecorated = InitializeDecoratorArray(level);

		foreach (var room in level.Rooms)
		{
			DecorateRoom(levelDecorated, room, decorationsTransform);
		}

		GenerateTextureFromTileType(levelDecorated);
	}

	void DecorateRoom(TileType[,] levelDecorated, Room room, Transform decorationTransform)
	{
		int currentTries = 0;
		int maxTries = 50;

		int currentNumberOfDecorations = 0;
		int maxNumberOfDecorations = room.Area.width * room.Area.height * 4;
		List<RuleAvailability> availableRulesForRoom = CopyRuleAvailability();

		while (currentNumberOfDecorations < maxNumberOfDecorations && currentTries < maxTries && availableRulesForRoom.Count > 0)
		{
			int selectedRuleIndex = random.Next(0, availableRulesForRoom.Count);
			RuleAvailability selectedRuleAvailability = availableRulesForRoom[selectedRuleIndex];
			BaseDecoratorRule selectedRule = selectedRuleAvailability.rule;

			if (selectedRule.CanBeApplied(levelDecorated, room))
			{
				selectedRule.Apply(levelDecorated, room, decorationTransform);
				currentNumberOfDecorations++;
				if (selectedRuleAvailability.maxAvailability > 0)
				{
					selectedRuleAvailability.maxAvailability--;
				}
				if (selectedRuleAvailability.maxAvailability == 0)
				{
					availableRulesForRoom.Remove(selectedRuleAvailability);
				}
			}
			currentTries++;
		}
	}

	private List<RuleAvailability> CopyRuleAvailability()
	{
		List<RuleAvailability> availableRulesForRoom = new List<RuleAvailability>();
		availableRules.ToList().ForEach(original => availableRulesForRoom.Add(new RuleAvailability(original)));
		return availableRulesForRoom;
	}

	private TileType[,] InitializeDecoratorArray(Level level)
	{
		TextureBasedLevel textureBasedLevel = new TextureBasedLevel(levelTexture);
		TileType[,] levelDecorated = new TileType[level.Width, level.Length];
		for (int y = 0; y < level.Length; y++)
		{
			for (int x = 0; x < level.Width; x++)
			{
				bool isBlocked = textureBasedLevel.IsBlocked(x, y);
				if (isBlocked)
				{
					levelDecorated[x, y] = TileType.Wall;
				}
				else
				{
					levelDecorated[x, y] = TileType.Floor;
				}
			}
		}
		return levelDecorated;
	}

	private void GenerateTextureFromTileType(TileType[,] tileTypes)
	{
		int width = tileTypes.GetLength(0);
		int length = tileTypes.GetLength(1);

		Color32[] pixels = new Color32[width * length];

		for (int y = 0; y < length; y++)
		{
			for (int x = 0; x < width; x++)
			{
				pixels[x + y * width] = tileTypes[x, y].GetColor();
			}
		}

		decoratedTexture.Reinitialize(width, length);
		decoratedTexture.SetPixels32(pixels);
		decoratedTexture.Apply();
		decoratedTexture.SaveAsset();
	}

}
