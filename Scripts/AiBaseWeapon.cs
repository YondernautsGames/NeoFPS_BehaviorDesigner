using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NeoFPS.BehaviourDesigner
{
    public class AiBaseWeapon : MonoBehaviour, IAiWeapon
    {
		[SerializeField, Tooltip("The damage the weapon does.")]
		private float m_Damage = 50f;
		[SerializeField, Tooltip("The minimum range that the melee weapon can reach.")]
		private float m_MinimumRange = 1f;
		[SerializeField, Tooltip("The maximum range that the melee weapon can reach.")]
		private float m_MaximumRange = 1.2f;
		[SerializeField, Tooltip("The delay from starting the attack to checking for an impact. Should be synced with the striking point in the animation.")]
		private float m_TimeToImpact = 0.6f;
		[SerializeField, Tooltip("The recovery time after a hit.")]
		private float m_RecoveryTime = 1f;
		
		[Header("Kicker")]
		public float m_KickDistance = 0.02f;
		public float m_KickRotation = 5f;
		public float m_KickDuration = 0.5f;

		public float damageAmount
		{
			get { return m_Damage; }
		}

		public float recoveryTime
		{
			get { return m_RecoveryTime; }
		}

		public float timeToImpact
		{
			get { return m_TimeToImpact; }
		}

		public float kickDuration
		{
			get { return m_KickDistance; }
		}

		public float kickDistance
		{
			get { return m_KickDistance; }
		}

		public float kickRotation
		{
			get { return m_KickRotation; }
		}

	}
}
