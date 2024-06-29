using UnityEngine;

namespace FinalScripts.Refactored
{
    public class RagdollReplacer : MonoBehaviour
    {
        public GameObject ragdollPrefab;

        public void Replace()
        {
            GameObject ragdollInstance = Instantiate(ragdollPrefab, transform.position, transform.rotation);
            ragdollInstance.SetActive(true); 
            Destroy(gameObject);
            
            return;
            
            ragdollInstance.SetActive(false);

            //EnemyBase baseController = GetComponent<EnemyBase>();
            //RigidbodyDelayedForce t = ragdollInstance.AddComponent<RigidbodyDelayedForce>();
            //t.forceToAdd = baseController.ExternalForce;

            Transform ragdollCurrent = ragdollInstance.transform;
            Transform current = transform;
            bool first = true;

            while (current != null && ragdollCurrent != null)
            {
                if (first || ragdollCurrent.name == current.name)
                {
                    ragdollCurrent.rotation = current.rotation;
                    ragdollCurrent.position = current.position;
                    first = false;
                }

                if (current.childCount > 0)
                {
                    current = current.GetChild(0);
                    ragdollCurrent = ragdollCurrent.GetChild(0);
                }
                else
                {
                    while (current != null)
                    {
                        if (current.parent == null || ragdollCurrent.parent == null)
                        {
                            current = null;
                            ragdollCurrent = null;
                        }
                        else if (current.GetSiblingIndex() == current.parent.childCount - 1 ||
                                 current.GetSiblingIndex() + 1 >= ragdollCurrent.parent.childCount)
                        {
                            current = current.parent;
                            ragdollCurrent = ragdollCurrent.parent;
                        }
                        else
                        {
                            current = current.parent.GetChild(current.GetSiblingIndex() + 1);
                            ragdollCurrent = ragdollCurrent.parent.GetChild(ragdollCurrent.GetSiblingIndex() + 1);
                            break;
                        }
                    }
                }
            }

            ragdollInstance.SetActive(true);
            Destroy(gameObject);
        }
    }
}