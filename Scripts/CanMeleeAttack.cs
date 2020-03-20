using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner
{
    [TaskCategory("Neo FPS")]
    [TaskDescription("Test to see if it is possible to use a melee attack against a character.")]
    [TaskName("Can Melee Attack")]
    public class CanMeleeAttack : Conditional
    {
        [Runtime.Tasks.Tooltip("The object that we want to test for an alive status. Must have an IHealthManager attached. If set to null then the task owner will be used.")]
        public SharedGameObject target;
        [Runtime.Tasks.Tooltip("The minimum distance from the target before the attack can be taken.")]
        public SharedFloat minDistance = 2;
        [Runtime.Tasks.Tooltip("The maximum distance from the target before the attack can be taken.")]
        public SharedFloat maxDistance = 2;
        [Runtime.Tasks.Tooltip("How long to cooldown between attacks.")]
        public float attackCooldown = 2;

        float nextAttackTime;

        public override TaskStatus OnUpdate()
        {
            if (Time.time < nextAttackTime)
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