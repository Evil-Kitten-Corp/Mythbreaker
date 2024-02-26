using UnityEngine;

namespace Ability_Behaviours.UI
{
    public class UIBillboard : MonoBehaviour
    {
        private Camera _cam;

        private void Awake()
        {
            _cam = Camera.main;
        }

        private void Update()
        {
            transform.forward = _cam.transform.forward;
        }
    }
}