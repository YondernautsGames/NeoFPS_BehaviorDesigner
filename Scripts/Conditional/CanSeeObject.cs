using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace NeoFPS.BehaviourDesigner
{
    [TaskCategory("Neo FPS")]
    [TaskDescription("Test to see if one object can see another.")]
    [TaskName("Can See Object")]
    public class CanSeeObject : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The agent that we are testing whether it can see a target object, if null then the owning object will be used.")]
        public SharedGameObject m_AgentTarget;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The target we are checking is within sight or not.")]
        public SharedGameObject m_Target;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The field of view angle of the agent (in degrees)")]
        public SharedFloat m_FieldOfViewAngle = 90;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The distance that the target needs to be within to be in sight.")]
        public SharedFloat m_ViewDistance = 500;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The LayerMask identifying objects to ignore when performing the line of sight check")]
        public LayerMask m_IgnoreLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");

        private float m_SqrMagnitude;
        private GameObject m_CurrentAgent;
        private GameObject m_PrevAgent;

        public override void OnStart()
        {
            m_SqrMagnitude = m_ViewDistance.Value * m_ViewDistance.Value;

            m_CurrentAgent = GetDefaultGameObject(m_AgentTarget.Value);
            if (m_CurrentAgent != m_PrevAgent)
            {
                m_PrevAgent = m_CurrentAgent;
            }
        }

        public override TaskStatus OnUpdate()
        {
            Vector3 direction = m_Target.Value.transform.position - m_CurrentAgent.transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            direction.y = 0;

            if (Vector3.SqrMagnitude(direction) < m_SqrMagnitude && angle < m_FieldOfViewAngle.Value * 0.5f)
            {
                RaycastHit hit;
                if (Physics.Linecast(m_CurrentAgent.transform.position, m_Target.Value.transform.position, out hit, ~m_IgnoreLayerMask, QueryTriggerInteraction.Ignore))
                {
                    if (hit.transform.IsChildOf(m_Target.Value.transform) || m_Target.Value.transform.IsChildOf(hit.transform))
                    {
                        return TaskStatus.Success;
                    }
                }
            }

            return TaskStatus.Failure;
        }
    }
}