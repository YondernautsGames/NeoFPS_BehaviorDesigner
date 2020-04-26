using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using UnityEngine.AI;
using System;

namespace NeoFPS.BehaviourDesigner
{
	public abstract class AbstractMovementAction : BehaviorDesigner.Runtime.Tasks.Action
	{
		[BehaviorDesigner.Runtime.Tasks.Tooltip("The object to move, if null then the owning object will be used.")]
		public SharedGameObject m_ActionTarget;
		[BehaviorDesigner.Runtime.Tasks.Tooltip("The enemy target the agent is engaged with.")]
		public SharedGameObject m_EnemyTarget;
		[BehaviorDesigner.Runtime.Tasks.Tooltip("The maximum time between the agent reconsidering its current movement plan? The time between evaluations will typically reduce as the agent gets nearer to an enemy.")]
		public SharedFloat m_MinEvaluationFrequency = 3;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The minimum distance between the current goal position and the proposed next goal position required to trigger a recalculation of the path.")]
        public float m_MinDisitanceForNewPath = 1;


        private GameObject prevGameObject;
		private NavMeshAgent m_Agent;
		protected float m_NextEvaluationTime = 0;
		private Vector3 m_GoalPosition;

        public Vector3 GoalPosition
        {
            get { return m_GoalPosition; }
            set
            {
                if (Vector3.Distance(m_GoalPosition, value) > m_MinDisitanceForNewPath)
                {
                    if (m_Agent.SetDestination(value))
                    {
                        m_Agent.isStopped = false;
                        m_GoalPosition = value;
                    }
                }
            }
        }

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
            if (Time.realtimeSinceStartup < m_NextEvaluationTime)
            {
                // No need to reevaluate and thus our existing movement plan is still appropriate and thus this is a success from a Behaviour tree perspective.
                return TaskStatus.Success;
            }

            if (SetOptimalNextPosition())
            {
                Debug.Log("Optimal position from which to attack updated to " + GoalPosition);
                if (Time.realtimeSinceStartup > m_NextEvaluationTime) // if the implementation of this abstract class has not updated the evaluation time do it here.
                {
                    m_NextEvaluationTime = Time.realtimeSinceStartup + m_MinEvaluationFrequency.Value;
                }
                return TaskStatus.Success;
            } else
            {
                Debug.Log("Optimal position from which to attack not updated.");
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