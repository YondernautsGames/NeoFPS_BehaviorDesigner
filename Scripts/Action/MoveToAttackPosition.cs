using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using UnityEngine.AI;
using System;

namespace NeoFPS.BehaviourDesigner
{
	[TaskCategory("Neo FPS")]
	[TaskDescription("Move to a suitable position to attack the player. By default the agent will attempt to find a suitable location 75% of the way between the min and max attack distances.")]
	[TaskName("Move to Attack Position")]
	public class MoveToAttackPosition : BehaviorDesigner.Runtime.Tasks.Action
	{
		[BehaviorDesigner.Runtime.Tasks.Tooltip("The object to move, if null then the owning object will be used.")]
		public SharedGameObject m_ActionTarget;
		[BehaviorDesigner.Runtime.Tasks.Tooltip("The target to attack.")]
		public SharedGameObject m_AttackTarget;
		[BehaviorDesigner.Runtime.Tasks.Tooltip("The minimum distance from which to attack.")]
		public SharedFloat m_MinimumAttackDistance = 5;
		[BehaviorDesigner.Runtime.Tasks.Tooltip("The maximum distance from which to attack.")]
		public SharedFloat m_MaximumAttackDistance = 25;
		[BehaviorDesigner.Runtime.Tasks.Tooltip("How often will the agent reconsider its current movement plan?")]
		public SharedFloat m_EvaluationFrequency = 2;

		private GameObject prevGameObject;
		private NavMeshAgent m_Agent;
		private float m_NextEvaluationTime = 0;
		private Vector3 m_GoalPosition;

		public override void OnStart()
		{
			var currentGameObject = GetDefaultGameObject(m_ActionTarget.Value);
			if (currentGameObject != prevGameObject)
			{
				m_Agent = currentGameObject.GetComponent<NavMeshAgent>();
				prevGameObject = currentGameObject;
				m_NextEvaluationTime = 0;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (Time.realtimeSinceStartup < m_NextEvaluationTime) return TaskStatus.Failure;

			m_GoalPosition = ScanForOptimalPosition();
			Debug.Log("Optimal position from which to attck is " + m_GoalPosition);
			m_NextEvaluationTime = Time.realtimeSinceStartup + m_EvaluationFrequency.Value;
			return m_Agent.SetDestination(m_GoalPosition) ? TaskStatus.Success : TaskStatus.Failure;
		}

		/// <summary>
		/// Scans the environmet for an optimal position from which to attack.
		/// </summary>
		/// <returns>The position that the agent should try to reach.</returns>
		private Vector3 ScanForOptimalPosition()
		{
			Debug.LogWarning("ScanForOptimalPosition is just finding a point on a straight line between AI and player. This is too simplistic.");
			Vector3 targetPos = m_AttackTarget.Value.transform.position;
			Vector3 direction = (targetPos - transform.position).normalized;
			float distance = Vector3.Distance(targetPos, transform.position);
			float optimalDistance = m_MinimumAttackDistance.Value;
			optimalDistance += distance > m_MaximumAttackDistance.Value ? (m_MaximumAttackDistance.Value - m_MinimumAttackDistance.Value) * 0.75f : (distance - m_MinimumAttackDistance.Value) * 0.75f;

			return transform.position + (direction * optimalDistance);
		}

		public override void OnDrawGizmos()
		{
			base.OnDrawGizmos();

			Gizmos.color = Color.black;
			Gizmos.DrawWireSphere(m_GoalPosition, 0.3f);
		}
	}
}