using UnityEngine;
using Random = System.Random;

public class RoomDecorator : MonoBehaviour
{
	[SerializeField] GameObject parent;
	[SerializeField] LayoutGeneratorRooms layoutGenerator;

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
		GameObject testGameObject = new GameObject("Test Child");
		testGameObject.transform.SetParent(decorationsTransform);
	}

}
