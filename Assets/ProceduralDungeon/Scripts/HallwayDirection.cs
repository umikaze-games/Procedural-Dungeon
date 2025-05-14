using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum HallwayDirection { Undefined, Left, Right, Top, Bottom };

public static class HallwayDirectionExtension
{
	private static Color yellow = new Color(1, 1, 0, 1);

	public static readonly Dictionary<HallwayDirection, Color> DirectionToColorMap = new Dictionary<HallwayDirection, Color>
	{
		{ HallwayDirection.Left, yellow },
		{ HallwayDirection.Right, Color.magenta },
		{ HallwayDirection.Top, Color.cyan },
		{ HallwayDirection.Bottom, Color.green }
	};

	public static Color GetColor(this HallwayDirection direction)
	{
		return DirectionToColorMap.TryGetValue(direction, out Color color) ? color : Color.gray;
	}
	public static HallwayDirection GetOppositeDirection(this HallwayDirection direction)
	{
		Dictionary<HallwayDirection, HallwayDirection> oppositeDirectionMap = new Dictionary<HallwayDirection, HallwayDirection>
		{
			{ HallwayDirection.Left, HallwayDirection.Right },
			{ HallwayDirection.Right, HallwayDirection.Left },
			{ HallwayDirection.Top, HallwayDirection.Bottom },
			{ HallwayDirection.Bottom, HallwayDirection.Top }
		};

		return oppositeDirectionMap.TryGetValue(direction, out HallwayDirection oppositeDirection) ? oppositeDirection : HallwayDirection.Undefined;
	}

	public static Dictionary<Color, HallwayDirection> GetColorToDirectionMap()
	{
		return DirectionToColorMap.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
	}

}
