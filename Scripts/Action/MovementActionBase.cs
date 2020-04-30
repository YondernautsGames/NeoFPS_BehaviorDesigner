using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using UnityEngine.AI;
using System;

namespace NeoFPS.BehaviourDesigner
{
	public abstract class MovementActionBase : NeoFpsActionBase
	{
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The minimum distance between the current goal position and the proposed next goal position required to trigger a recalculation of the path.")]
        public float m_MinDistanceForNewPath = 1;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Whether the NavMeshAgent should update its rotation during this move.")]
        public bool m_UpdateRotation = true;

        private NavMeshAgent m_NavMeshAgent;
		private Vector3 m_GoalPosition;

        public Vector3 GoalPosition
        {
            get { return m_GoalPosition; }
            set
            {
                if (Vector3.Distance(m_GoalPosition, value) > m_MinDistanceForNewPath)
                {
                    if (m_NavMeshAgent.SetDestination(value))
                    {
                        m_NavMeshAgent.updateRotation = m_UpdateRotation;
                        m_NavMeshAgent.isStopped = false;
                        m_GoalPosition = value;
                    }
                }
            }
        }

		public override void OnStart()
        {
            base.OnStart();
            if (IsDirty)
            {
				m_NavMeshAgent = currentAiAgent.GetComponent<NavMeshAgent>();
                IsDirty = false;
            }
        }

		public override TaskStatus OnUpdate()
        {
            if (!IsEvaluationDue())
            {
                return TaskStatus.Success;
            }

            if (SetOptimalNextPosition())
            {
                if (Time.realtimeSinceStartup > m_NextEvaluationTime) // if the implementation of this abstract class has not updated the evaluation time do it here.
                {
                    m_NextEvaluationTime = Time.realtimeSinceStartup + m_MinEvaluationFrequency.Value;
                }
                return TaskStatus.Success;
            } else
            {
                // optimal position has not changed so assume our current goal is still optimal thus our movement is a success
                return TaskStatus.Success;
            }
		}

		/// <summary>
		/// Scans the environment for an optimal position to move to next.
        /// If a better position than the current one is found then `GoalPosition` is
        /// updated and true is returned.
		/// </summary>
		/// <returns>True if a new position has been set, otherwise fals.</returns>
		internal abstract bool SetOptimalNextPosition();

		public override void OnDrawGizmos()
		{
			base.OnDrawGizmos();

			Gizmos.color = Color.black;
			Gizmos.DrawWireSphere(m_GoalPosition, 0.3f);
		}
	}
}