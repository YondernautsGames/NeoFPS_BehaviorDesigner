using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace NeoFPS.BehaviourDesigner
{
    [TaskCategory("Neo FPS")]
    [TaskDescription("Test to see if two objects are within a given distance of one another.")]
    [TaskName("Within Distance")]
    public class WithinDistance : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The agent that should be within a distance of the target object, if null then the owning object will be used.")]
        public SharedGameObject m_AgentTarget;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The target we are checking is within a distance or not.")]
        public SharedGameObject m_Target;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The distance that the target needs to be within")]
        public SharedFloat m_Distance = 5;
        
        private float m_SqrMagnitude;
        private GameObject m_CurrentAgent;
        private GameObject m_PrevAgent;

        public override void OnStart()
        {
            m_SqrMagnitude = m_Distance.Value * m_Distance.Value;

            m_CurrentAgent = GetDefaultGameObject(m_AgentTarget.Value);
            if (m_CurrentAgent != m_PrevAgent)
            {
                m_PrevAgent = m_CurrentAgent;
            }
        }

        public override TaskStatus OnUpdate()
        {
            Vector3 distance = m_Target.Value.transform.position - m_CurrentAgent.transform.position;

            if (Vector3.SqrMagnitude(distance) < m_SqrMagnitude)
            {
                return TaskStatus.Success;
            } else
            {
                return TaskStatus.Failure;
            }
        }
    }
}