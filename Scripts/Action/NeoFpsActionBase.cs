using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace NeoFPS.BehaviourDesigner
{
    [TaskCategory("Neo FPS")]
    public class NeoFpsActionBase : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The agent that this action operates against.")]
        public SharedGameObject m_AiAgent;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The target object for this action.")]
        public SharedGameObject m_Target;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The maximum time between the agent reconsiders this action? The time between evaluations will typically reduce as the agent gets nearer to an enemy. If this time is 0 then it will be evaluated every time the tree comes to it. Otherwise it will return failure whenever this time has not elapsed since the last evaluation.")]
        public SharedFloat m_MinEvaluationFrequency = 3;

        protected GameObject currentAiAgent;
        protected GameObject prevAiAgent;
        protected GameObject currentTarget;
        protected GameObject prevTarget;
        protected float m_NextEvaluationTime = 0;

        /// <summary>
        /// Indicates if the action confiugration is considered dirty.
        /// When it is all components and game object values should
        /// be updated before use.
        /// </summary>
        protected bool IsDirty { get; set; }

        /// <summary>
        /// Executed every time the action is started. By default
        /// this will check to see if if the AI Agent or the Target
        /// have changed since the last execution. If they have then
        /// SetDirty will be set to true. Implementing classes should
        /// then re-initialize any component references.
        /// </summary>
        public override void OnStart()
        {
            currentAiAgent = GetDefaultGameObject(m_AiAgent.Value);
            if (currentAiAgent != prevAiAgent)
            {
                prevAiAgent = currentAiAgent;
                m_NextEvaluationTime = 0;
                IsDirty = true;
            }

            currentTarget = GetDefaultGameObject(m_Target.Value);
            if (currentTarget != prevTarget)
            {
                prevTarget = currentTarget;
                m_NextEvaluationTime = 0;
                IsDirty = true;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (!IsEvaluationDue())
            {
                return TaskStatus.Failure;
            }

            return TaskStatus.Success;
        }

        internal bool IsEvaluationDue()
        {
            if (Time.realtimeSinceStartup < m_NextEvaluationTime)
            {
                return false;
            }

            m_NextEvaluationTime = Time.realtimeSinceStartup + m_MinEvaluationFrequency.Value;
            return true;
        }
    }
}