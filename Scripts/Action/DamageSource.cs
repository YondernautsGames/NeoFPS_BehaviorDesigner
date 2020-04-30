using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System;
using UnityEngine;

namespace NeoFPS.BehaviourDesigner
{
    [TaskCategory("Neo FPS")]
    [TaskDescription("Do damage to a Neo FPS character.")]
    [TaskName("Damage Source")]
    [Obsolete("Use AttackAction instead.")]
    public class DamageSource : NeoFpsActionBase, IDamageSource
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The amount of damage to be dealt.")]
        public float damageAmount = 5;
        public bool isCritical = false;
        public float kickDistance = 0.02f;
        public float kickRotation = 5f;
        public float kickDuration = 0.5f;

        #region Neo FPS IDamageSource
        public NeoFPS.DamageFilter outDamageFilter
        {
            get { return NeoFPS.DamageFilter.AllDamageAllTeams; }
            set { }
        }

        public NeoFPS.IController controller
        {
            get { return null; }
        }

        public Transform damageSourceTransform
        {
            get { return transform; }
        }

        public string description
        {
            get { return transform.name; }
        }
        #endregion

        public override TaskStatus OnUpdate()
        {
            DamageNeoFpsPlayer();

            return TaskStatus.Success;
        }

        void DamageNeoFpsPlayer()
        {
            // Damage the player health
            var health = m_Target.Value.GetComponent<NeoFPS.IHealthManager>();
            if (health == null)
                return;
            
            health.AddDamage(damageAmount, isCritical, this);

            // Get character head kicker
            var character = m_Target.Value.GetComponent<NeoFPS.ICharacter>();
            if (character == null)
                return;
            var kicker = character.headTransformHandler.GetComponent<NeoFPS.AdditiveKicker>();
            if (kicker == null)
                return;

            // Get direction of attack
            var direction = m_Target.Value.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            // Kick the camera position & rotation
            kicker.KickPosition(direction * kickDistance, kickDuration);
            kicker.KickRotation(Quaternion.AngleAxis(kickRotation, Vector3.Cross(direction, Vector3.up)), kickDuration);
        }
    }
}
