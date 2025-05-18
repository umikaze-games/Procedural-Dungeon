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
		Level level = layoutGeneratorRooms.GenerateLevel();
		marchingSquares.CreateLevelGeometry();
		navMeshSurface.BuildNavMesh();
		Room startRoom = level.PlayerStartRoom;
		Vector2 roomCenter = startRoom.Area.center;
		Vector3 playerPosition = LevelPositionToWorldPosition(roomCenter);

		GameObject player = GameObject.FindGameObjectWithTag("Player");
		player.transform.position = playerPosition;

	}
	Vector3 LevelPositionToWorldPosition(Vector2 levelPosition)
	{
		int scale = SharedLevelData.Instance.Scale;
		return new Vector3((levelPosition.x - 1) * scale, 0, (levelPosition.y - 1) * scale);
	}

}
