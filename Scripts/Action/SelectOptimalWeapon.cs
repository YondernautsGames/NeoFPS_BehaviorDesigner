using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace NeoFPS.BehaviourDesigner
{
	[TaskCategory("Neo FPS")]
	[TaskDescription("Select the optimal weapon available given the current situation.")]
	[TaskName("Select Optimal Weapon")]
	public class SelectOptimalWeapon : NeoFpsActionBase
	{
		private IInventory m_Inventory;
		private IAiCharacter m_Character;

		public override void OnStart()
		{
			base.OnStart();
			if (IsDirty)
			{
				m_Inventory = currentAiAgent.GetComponent<IInventory>();
				m_Character = currentAiAgent.GetComponent<IAiCharacter>();
				IsDirty = false;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (!IsEvaluationDue())
			{
				return TaskStatus.Success;
			}

			Debug.LogWarning("SelectOptimalWeapon action does not currently select a weapon, it just goes with the default.");
			return TaskStatus.Success;
		}
	}
}