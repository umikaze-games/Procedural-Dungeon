using UnityEngine;

public abstract class BaseDecoratorRule : ScriptableObject
{

	internal abstract bool CanBeApplied(TileType[,] levelDecorated, Room room);
	internal abstract void Apply(TileType[,] levelDecorated, Room room, Transform parent);

}
