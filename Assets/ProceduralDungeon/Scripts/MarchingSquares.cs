using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingSquares : MonoBehaviour
{
	[SerializeField] Texture2D levelTexture;
	[SerializeField] GameObject generatedLevel;
	[SerializeField] Tileset tileset;
	[SerializeField] int scale = 1;

	[ContextMenu("Create Level Geometry")]
	public void CreateLevelGeometry()
	{
		generatedLevel.transform.DestroyAllChildren();
		Vector3 scaleVector = new Vector3(scale, scale, scale);
		TextureBasedLevel level = new TextureBasedLevel(levelTexture);
		for (int y = 0; y < level.Length - 1; y++)
		{
			for (int x = 0; x < level.Width - 1; x++)
			{
				int tileIndex = CalculateTileIndex(level, x, y);
				GameObject tile = tileset.GetTile(tileIndex);
				if (tile == null) continue;
				GameObject.Instantiate(tile, generatedLevel.transform);
				tile.transform.localScale = scaleVector;
				tile.transform.position = new Vector3(x * scale, 0, y * scale);
				string name = "x" + x + "y" + y + "tileIndex" + tileIndex;
				tile.name = name;
			}
		}
	}
	int CalculateTileIndex(ILevel level, int x, int y)
	{
		int topLeft = level.IsBlocked(x, y + 1) ? 1 : 0;
		int topRight = level.IsBlocked(x + 1, y + 1) ? 1 : 0;
		int bottomLeft = level.IsBlocked(x, y) ? 1 : 0;
		int bottomRight = level.IsBlocked(x + 1, y) ? 1 : 0;
		int tileIndex = topLeft + topRight * 2 + bottomLeft * 4 + bottomRight * 8;
		return tileIndex;
	}

}
