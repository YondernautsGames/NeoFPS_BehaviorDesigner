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
	public class MoveToAttackPosition : AbstractMovementAction
	{
		[BehaviorDesigner.Runtime.Tasks.Tooltip("The minimum distance from which to attack.")]
		public SharedFloat m_MinimumAttackDistance = 5;
		[BehaviorDesigner.Runtime.Tasks.Tooltip("The maximum distance from which to attack.")]
		public SharedFloat m_MaximumAttackDistance = 25;

		internal override bool SetOptimalNextPosition()
		{
			Debug.LogWarning("MoveToAttackPosition.SetOptimalNextPosition is just finding a point on a straight line between AI and player. This is too simplistic.");
			Vector3 targetPos = m_EnemyTarget.Value.transform.position;
			float distance = Vector3.Distance(m_EnemyTarget.Value.transform.position, transform.position);

            if (distance >= m_MinimumAttackDistance.Value && distance <= m_MaximumAttackDistance.Value)
            {
                return false;
            }

            Vector3 direction = (targetPos - transform.position).normalized;
            float optimalDistance = m_MinimumAttackDistance.Value;
			optimalDistance += distance > m_MaximumAttackDistance.Value ? (m_MaximumAttackDistance.Value - m_MinimumAttackDistance.Value) * 0.75f : (distance - m_MinimumAttackDistance.Value) * 0.75f;

            Vector3 newPosition = transform.position + (direction * optimalDistance);
            m_NextEvaluationTime = Time.realtimeSinceStartup + (m_MinEvaluationFrequency.Value * (optimalDistance / distance));

            if (GoalPosition != newPosition)
            {
                GoalPosition = newPosition;
                return true;
            } else
            {
                return false;
            }
		}
	}
}