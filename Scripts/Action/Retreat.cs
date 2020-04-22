using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

namespace NeoFPS_BehaviorDesigner
{
	[TaskCategory("Neo FPS")]
	[TaskDescription("Retreat from a target object using the NavMesh. The NPC will seek to defend itself while retreating, that is it will not turn and run.")]
	[TaskName("Retreat")]
	public class Retreat : Action
	{
		[BehaviorDesigner.Runtime.Tasks.Tooltip("The target to retreat from.")]
		public SharedGameObject m_Target;
		[BehaviorDesigner.Runtime.Tasks.Tooltip("The distance the agent must be before it considers itself safe.")]
		public SharedFloat m_SafeDistance = 10;

		NavMeshAgent m_Agent;


		public override void OnStart()
		{
			m_Agent = GetComponent<NavMeshAgent>();
		}

		public override void OnEnd()
		{
			base.OnEnd();
		}

		public override TaskStatus OnUpdate()
		{
			Vector3 direction = transform.position - m_Target.Value.transform.position;
			direction.Normalize();

			Vector3 point = transform.position + (direction * m_SafeDistance.Value);

			NavMeshHit hit;
			if (NavMesh.SamplePosition(point, out hit, 1.0f, NavMesh.AllAreas))
			{
				m_Agent.updateRotation = false;
				m_Agent.SetDestination(hit.position);
			} else
			{
				Debug.LogWarning("Agent could not retreat to safe distance, need to add logic to find a different point.");
			}

			return TaskStatus.Success;
		}
	}
}