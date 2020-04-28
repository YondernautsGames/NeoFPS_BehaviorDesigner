using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeoFPS.BehaviourDesigner
{
    /// <summary>
    /// Attaches a game object to a specific point on the model.
    /// Attach a Game Object that you want to attach to.
    /// </summary>
    [ExecuteInEditMode]
    public class MountPoint : MonoBehaviour
    {
        [SerializeField, Tooltip("The object currently attached to this mountpoint. If this is a prefab then it will be instantiated.")]
        GameObject m_AttachedObject;

        GameObject m_PreviousGameObject;

        void Update()
        {
            if (m_PreviousGameObject != m_AttachedObject)
            {
                if (m_AttachedObject != null)
                {
                    if (!m_AttachedObject.scene.isLoaded)
                    {
                        m_AttachedObject = Instantiate(m_AttachedObject, transform, false);
                    }
                    else
                    {
                        m_AttachedObject.transform.SetParent(transform, false);
                    }
                }
                m_PreviousGameObject = m_AttachedObject;
            }
        }

    }
}