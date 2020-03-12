using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using NeoFPS;
using UnityEngine;

namespace BehaviorDesigner
{
    [TaskCategory("Neo FPS")]
    [TaskDescription("Test if a character is alive or not. Returning success if alive.")]
    [TaskName("Is Alive")]
    public class IsAlive : Conditional
    {
        [Runtime.Tasks.Tooltip("The object that we want to test for an alive status. Must have an IHealthManager attached. If set to null then the task owner will be used.")]
        public SharedGameObject targetGameObject;

        private IHealthManager healthManager;
        private GameObject prevGameObject;

        public override void OnStart()
        {
            GameObject currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject)
            {
                if (currentGameObject != null)
                {
                    healthManager = currentGameObject.GetComponent<IHealthManager>();
                } else
                {
                    healthManager = null;
                }
            }
        }

        public override TaskStatus OnUpdate()
        {   
            if (healthManager != null && healthManager.isAlive)
            {
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Failure;
            }
        }
    }
}
