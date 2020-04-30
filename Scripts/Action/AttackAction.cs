using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;

namespace NeoFPS.BehaviourDesigner
{
    [TaskCategory("Neo FPS")]
    [TaskDescription("Attack another character with the currently equipped weapon.")]
    [TaskName("Attack")]
    public class AttackAction : NeoFpsActionBase, IDamageSource
    {
        private IHealthManager m_TargetHealthManager;
        private IAiCharacter m_AiCharacter;
        private Animator m_AiAnimator;

        private TaskStatus m_CurrentStatus;

        public override void OnStart()
        {
            base.OnStart();
            m_CurrentStatus = TaskStatus.Inactive;

            if (IsDirty)
            {
                m_TargetHealthManager = currentTarget.GetComponent<IHealthManager>();
                m_AiCharacter = currentAiAgent.GetComponent<IAiCharacter>();
                m_AiAnimator = currentAiAgent.GetComponent<Animator>();
                IsDirty = false;
            }
        }
        
        public override TaskStatus OnUpdate()
        {
            TaskStatus baseStatus = base.OnUpdate();
            if (baseStatus != TaskStatus.Success)
            {
                return baseStatus;
            }

            if (m_CurrentStatus == TaskStatus.Inactive)
            {
                StartCoroutine(DamageTarget());
            }

            return m_CurrentStatus;
        }

        /// <summary>
        /// Attempt to damage the Target.
        /// </summary>
        /// <returns>Return true if an attack was made (whether damage was done or not)</returns>
        IEnumerator DamageTarget()
        {
            IAiWeapon weapon = m_AiCharacter.quickSlots.selected.GetComponent<IAiWeapon>();
            if (weapon == null) yield return null;

            var character = currentTarget.GetComponent<NeoFPS.ICharacter>();
            if (character == null) yield return null;

            var kicker = character.headTransformHandler.GetComponent<NeoFPS.AdditiveKicker>();
            if (kicker == null) yield return null;

            m_AiAnimator.SetTrigger("Melee Attack");
            m_CurrentStatus = TaskStatus.Running;
            yield return new WaitForSeconds(weapon.timeToImpact);

            bool isCritical = false;
            m_TargetHealthManager.AddDamage(weapon.damageAmount, isCritical, this);

            // Get direction of attack
            var direction = m_Target.Value.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            // Kick the camera position & rotation
            kicker.KickPosition(direction * weapon.kickDistance, weapon.kickDuration);
            kicker.KickRotation(Quaternion.AngleAxis(weapon.kickRotation, Vector3.Cross(direction, Vector3.up)), weapon.kickDuration);

            yield return new WaitForSeconds(weapon.recoveryTime);

            m_CurrentStatus = TaskStatus.Success;
        }

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
    }
}