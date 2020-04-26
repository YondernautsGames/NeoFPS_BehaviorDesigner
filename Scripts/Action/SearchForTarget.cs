using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

namespace NeoFPS.BehaviourDesigner
{
	[TaskCategory("Neo FPS")]
	[TaskDescription("Search for a target by using a random search algorithm.")]
	[TaskName("Search For Target")]
	public class SearchForTarget : AbstractMovementAction
	{
		internal override bool SetOptimalNextPosition()
		{
			Debug.LogWarning("SearchForTarget.SetOptimalNextPosition is a simplistic forward wander behaviour at this point. Need a real search strategy.");

			Vector3 targetPos = m_EnemyTarget.Value.transform.position;
			Vector3 direction = transform.forward * Random.Range(-35, 35);
			float optimalDistance = 5f;
			Vector3 position = transform.position + (direction * optimalDistance);

            int tries = 0;
            int maxTries = 5;
			NavMeshHit hit;
			while (!NavMesh.SamplePosition(position, out hit, 1, NavMesh.AllAreas) && tries < maxTries)
			{
				direction = transform.forward * Random.Range(165, 195);
				position = transform.position + (direction * optimalDistance);
                tries++;
			}

            if (tries < maxTries)
            {
                GoalPosition = position;
                return true;
            } else
            {
                return false;
            }
		}
	}
}