using BehaviorDesigner.Runtime;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace NeoFPS.BehaviourDesigner
{
    public class AiBaseCharacter : MonoBehaviour, IAiCharacter
    {
        [Header("Animation Settings")]
        [SerializeField, Tooltip("The name of an animation parameter that will trigger the death animation transition")]
        string m_DeathParameterName = "Die";

        [Header("Ragdoll Settings")]
        [SerializeField, Tooltip("Use ragdoll (set to true) or use animations (set to false) for death.")]
        bool m_UseRagdoll = false;
        [SerializeField, Tooltip("Characters main collider that is used when not a ragdoll.")]
        Collider m_MainCollider;
        [SerializeField, Tooltip("The time until the character gets back up after becoming a ragdoll.")]
        float m_GetBackUpTime = 5f;

        Rigidbody[] m_Rigidbodies;
        bool m_IsRagDoll = false;
        NavMeshAgent m_agent;
        Animator m_animator;
        BehaviorTree m_BehaviourTree;

        public IInventory inventory
        {
            get;
            private set;
        }

        public IQuickSlots quickSlots
        {
            get;
            private set;
        }
        
        void Awake()
        {
            m_agent = GetComponent<NavMeshAgent>();
            m_Rigidbodies = GetComponentsInChildren<Rigidbody>();
            m_animator = GetComponent<Animator>();
            m_BehaviourTree = GetComponent<BehaviorTree>();
            inventory = GetComponent<IInventory>();
            quickSlots = GetComponent<IQuickSlots>();
            ToggleRagdoll(true);
        }

        private void OnEnable()
        {
            GetComponent<IHealthManager>().onIsAliveChanged += OnIsAliveChanged;
        }
        private void OnDisable()
        {
            GetComponent<IHealthManager>().onIsAliveChanged -= OnIsAliveChanged;
        }

        protected virtual void OnIsAliveChanged(bool isAlive)
        {
            if (!isAlive)
            {
                if (m_UseRagdoll)
                {
                    ToggleRagdoll(false);
                } else
                {
                    m_animator.SetTrigger(m_DeathParameterName);
                }
                m_agent.isStopped = true;
                m_BehaviourTree.DisableBehavior();
                StartCoroutine(ReturnFromDeath());
            } else
            {
                if (m_UseRagdoll)
                {
                    ToggleRagdoll(true);
                }

                m_BehaviourTree.EnableBehavior();
                m_animator.Play("Idle");
                m_agent.isStopped = false;
            }
        }

        void ToggleRagdoll(bool isAnimating)
        {
            m_IsRagDoll = !isAnimating;
            if (m_MainCollider != null)
                m_MainCollider.enabled = isAnimating;
            for (int i = 0; i < m_Rigidbodies.Length; i++)
            {
                m_Rigidbodies[i].isKinematic = isAnimating;
            }
            
            m_animator.enabled = isAnimating;
        }

        IEnumerator ReturnFromDeath()
        {
            yield return new WaitForSeconds(m_GetBackUpTime);
            GetComponent<IHealthManager>().AddHealth(GetComponent<IHealthManager>().healthMax);
        }
    }
}