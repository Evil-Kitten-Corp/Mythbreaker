using System;
using System.Collections;
using Base;
using UnityEngine;
using BrunoMikoski.ServicesLocation;

namespace Abilities
{
    public class AbilityPreviewer : MonoBehaviour
    {
        public GameObject vfxMarkerPrefab;

        private bool _isAiming;
        private GameObject _currentVfxMarker;
        private KeyCode _key;
        private AltAbilityBase _ability;

        private event Action<Transform> OnAbilityCast;

        private void Start()
        {
            _currentVfxMarker = Instantiate(vfxMarkerPrefab);
            _currentVfxMarker.SetActive(false);
        }

        public void SubscribeToAbilityCast(Action<Transform> method, AltAbilityBase ability)
        {
            OnAbilityCast += method;
            _ability = ability;
        }

        private void Update()
        {
            if (_isAiming)
            {
                RaycastHit hit;
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    _currentVfxMarker.SetActive(true);
                    _currentVfxMarker.transform.position = hit.point;
                }
                else
                {
                    _currentVfxMarker.SetActive(false);
                }
            }

            if (Input.GetKeyDown(_ability.GetKeyCode(ServiceLocator.Instance.GetInstance<InputManager>())))
            {
                StartCoroutine(AbilityCast());
            }

            if (Input.GetKeyUp(_ability.GetKeyCode(ServiceLocator.Instance.GetInstance<InputManager>())))
            {
                _isAiming = false;
            }
        }

        private IEnumerator AbilityCast()
        {
            _ability.isCasting = true;
            _isAiming = true;
            _currentVfxMarker.SetActive(true);
            yield return new WaitUntil(() => Input.GetKeyUp(_ability.GetKeyCode(ServiceLocator.Instance
                .GetInstance<InputManager>())));
            OnAbilityCast?.Invoke(_currentVfxMarker.transform);
            yield return new WaitForSeconds(5f);
            _isAiming = false;
            _ability.isCasting = false;
        }
    }
}