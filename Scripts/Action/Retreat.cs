using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

namespace NeoFPS.BehaviourDesigner
{
	[TaskCategory("Neo FPS")]
	[TaskDescription("Retreat from a target object using the NavMesh. The NPC will seek to defend itself while retreating, that is it will not turn and run.")]
	[TaskName("Retreat")]
	public class Retreat : MovementActionBase
	{
		[BehaviorDesigner.Runtime.Tasks.Tooltip("The distance the agent must be before it considers itself safe.")]
		public SharedFloat m_SafeDistance = 10;
		
		internal override bool SetOptimalNextPosition()
		{
			Vector3 direction = transform.position - m_Target.Value.transform.position;
			direction.Normalize();

			Vector3 point = transform.position + (direction * m_SafeDistance.Value);

			Vector3 newPosition;
			NavMeshHit hit;
			if (NavMesh.SamplePosition(point, out hit, 1.0f, NavMesh.AllAreas))
			{	
				newPosition = hit.position;
			} else
			{
				Debug.LogWarning("Agent could not retreat to safe distance, need to add logic to find a different point.");
				return false;
			}

			if (GoalPosition != newPosition)
			{
				GoalPosition = newPosition;
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}