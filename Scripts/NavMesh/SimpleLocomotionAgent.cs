using UnityEngine;
using UnityEngine.AI;

namespace NeoFPS.BehaviourDesigner
{
    /// <summary>
    /// Converts NavMesh driven motion to XVelocity, YVelocity and Move parameters
    /// in the Animtor. This allows NavMeshAgents to drive animation through a 
    /// blend tree or similar.
    /// 
    /// Original code from: https://docs.unity3d.com/Manual/nav-CouplingAnimationAndNavigation.html
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class SimpleLocomotionAgent : MonoBehaviour
    {
        [Header("Animator Parameters")]
        [SerializeField, Tooltip("The name of the boolean parameter in the aninmator that indicates whether the character is moving under it's own power or not.")]
        string m_MoveParameterName = "Move";
        [SerializeField, Tooltip("The animator parameter representing the velovity of movement in the X axis (from 0 to 1)")]
        string m_VelocityXParameterName = "XVelocity";
        [SerializeField, Tooltip("The animator parameter representing the velovity of movement in the Y axis (from 0 to 1)")]
        string m_VelocityYParameterName = "YVelocity";

        Animator anim;
        NavMeshAgent agent;
        Vector2 smoothDeltaPosition = Vector2.zero;
        Vector2 velocity = Vector2.zero;

        void Start()
        {
            anim = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            // Don’t update position automatically
            agent.updatePosition = false;
        }

        void Update()
        {
            Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

            // Map 'worldDeltaPosition' to local space
            float dx = Vector3.Dot(transform.right, worldDeltaPosition);
            float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
            Vector2 deltaPosition = new Vector2(dx, dy);

            // Low-pass filter the deltaMove
            float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
            smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

            // Update velocity if time advances
            if (Time.deltaTime > 1e-5f)
                velocity = smoothDeltaPosition / Time.deltaTime;

            bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;

            // Update animation parameters
            anim.SetBool(m_MoveParameterName, shouldMove);
            anim.SetFloat(m_VelocityXParameterName, velocity.x);
            anim.SetFloat(m_VelocityXParameterName, velocity.y);

            //GetComponent<LookAt>().lookAtTargetPosition = agent.steeringTarget + transform.forward;
        }

        void OnAnimatorMove()
        {
                transform.position = agent.nextPosition;
        }
    }
}