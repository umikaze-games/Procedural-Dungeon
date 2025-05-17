using UnityEngine;

[CreateAssetMenu(fileName ="Tileset",menuName ="Custom/Procedual Generation/Tileset")]
public class Tileset : ScriptableObject
{
	[SerializeField]
	Color wallColor;
	[SerializeField]
	TileVariant[] tiles = new TileVariant[16];
	public Color WallColor => wallColor;
	public GameObject GetTile(int tileIndex)
	{
		if (tileIndex>=tiles.Length)
		{
			return null;
		}
		return tiles[tileIndex].GetRandomTile();
	}
}
