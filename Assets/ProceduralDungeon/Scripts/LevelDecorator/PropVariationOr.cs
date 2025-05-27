using UnityEngine;
using Random = System.Random;
public class PropVariationOr : MonoBehaviour
{
	[SerializeField] GameObject[] gameObjects;
	[SerializeField] double probability;

	public void GenerateVariation()
	{
		Random random = SharedLevelData.Instance.Rand;
		foreach (var gameObject in gameObjects)
		{
			gameObject.SetActive(random.NextDouble() <= probability);
		}
	}
}
