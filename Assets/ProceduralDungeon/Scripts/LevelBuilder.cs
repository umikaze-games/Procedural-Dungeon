using UnityEngine;
using Unity.AI.Navigation;

public class LevelBuilder : MonoBehaviour
{
	[SerializeField] LayoutGeneratorRooms layoutGeneratorRooms;
	[SerializeField] MarchingSquares marchingSquares;
	[SerializeField] NavMeshSurface navMeshSurface;

	void Start()
	{
		GenerateRandom();
	}

	[ContextMenu("Generate Random")]
	public void GenerateRandom()
	{
		SharedLevelData.Instance.GenerateSeed();
		Generate();
	}

	[ContextMenu("Generate")]
	public void Generate()
	{
		layoutGeneratorRooms.GenerateLevel();
		marchingSquares.CreateLevelGeometry();
		navMeshSurface.BuildNavMesh();
	}
}
