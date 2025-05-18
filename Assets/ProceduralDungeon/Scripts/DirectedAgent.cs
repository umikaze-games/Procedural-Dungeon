using UnityEngine;
using UnityEngine.AI;

public class DirectedAgent : MonoBehaviour
{
	NavMeshAgent agent;

	void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	public void MoveToLocation(Vector3 targetPoint)
	{
		agent.destination = targetPoint;
		agent.isStopped = false;
	}

	void Update()
	{

	}

}
