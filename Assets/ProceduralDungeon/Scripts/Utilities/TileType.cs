using System.Collections.Generic;
using UnityEngine;

public enum TileType { Noop, Empty, Floor, BlockedFloor, Wall, DecoratedWall, Item }

public static class TileTypeExtension
{
    private static readonly Dictionary<TileType, Color> DirectionToColorMap = new Dictionary<TileType, Color>
    {
        { TileType.Wall, Color.black },
        { TileType.Floor, Color.white },
        { TileType.Item, Color.blue },
        { TileType.BlockedFloor, Color.red },
        { TileType.DecoratedWall, Color.yellow }
    };

    public static Color GetColor(this TileType tileType)
    {
        return DirectionToColorMap.TryGetValue(tileType, out Color color) ? color : Color.gray;
    }

}
