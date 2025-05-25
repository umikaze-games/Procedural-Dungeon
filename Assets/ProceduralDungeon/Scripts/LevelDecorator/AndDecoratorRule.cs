using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "AndDecoratorRule", menuName = "Custom/Procedural Generation/And Decorator Rule")]
public class AndDecoratorRule : BaseDecoratorRule
{

	[SerializeField] BaseDecoratorRule[] childRules;

	internal override bool CanBeApplied(TileType[,] levelDecorated, Room room)
	{
		foreach (BaseDecoratorRule rule in childRules)
		{
			if (!rule.CanBeApplied(levelDecorated, room))
			{
				return false;
			}
		}
		return true;
	}

	internal override void Apply(TileType[,] levelDecorated, Room room, Transform parent)
	{
		foreach (BaseDecoratorRule rule in childRules)
		{
			rule.Apply(levelDecorated, room, parent);
		}
	}

}
