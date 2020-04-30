using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using NeoFPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.BehaviourDesigner
{
    [TaskCategory("Neo FPS")]
    [TaskDescription("Test to see if it is possible to use a melee attack against a character.")]
    [TaskName("Can Melee Attack")]
    public class CanMeleeAttack : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The agent that we are testing whether it can see a target object, if null then the owning object will be used.")]
        public SharedGameObject m_Agent;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The target we want to attack. If set to null then the task owner will be used.")]
        public SharedGameObject target;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The minimum distance from the target before the attack can be taken.")]
        public SharedFloat minDistance = 2;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The maximum distance from the target before the attack can be taken.")]
        public SharedFloat maxDistance = 2;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("How long to cooldown between attacks.")]
        public float attackCooldown = 2;

        float nextAttackTime;
        private float m_SqrDistanceRange;
        private GameObject m_CurrentAgent;
        private GameObject m_PrevAgent;
        private IAiWeapon m_Weapon;

        public override void OnStart()
        {
            m_CurrentAgent = GetDefaultGameObject(m_Agent.Value);
            if (m_CurrentAgent != m_PrevAgent)
            {
                FpsInventoryBase inventory = m_CurrentAgent.GetComponent<FpsInventoryBase>();
                if (inventory.selected != null)
                {
                    m_Weapon = inventory.selected.gameObject.GetComponent<IAiWeapon>();
                    inventory.onSelectionChanged += WieldableSelectionChanged;
                } else
                {
                    m_Weapon = null;
                }

                if (m_PrevAgent != null)
                {
                    inventory = m_PrevAgent.GetComponent<FpsInventoryBase>();
                    inventory.onSelectionChanged -= WieldableSelectionChanged;
                }

                m_PrevAgent = m_CurrentAgent;
            }
        }

        private void WieldableSelectionChanged(IQuickSlotItem item)
        {
            m_Weapon = item.gameObject.GetComponent<IAiWeapon>();
        }

        public override TaskStatus OnUpdate()
        {
            if (Time.time < nextAttackTime)
            {
                return TaskStatus.Failure;
            }

            if (m_Weapon == null)
            {
                return TaskStatus.Failure;
            }

            float dist = Vector3.Distance(target.Value.transform.position, transform.position);
            if (dist >= minDistance.Value && dist <= maxDistance.Value)
            {
                nextAttackTime = Time.time + attackCooldown;
                return base.OnUpdate();
            }
            else
            {
                return TaskStatus.Failure;
            }
        }

    }
}