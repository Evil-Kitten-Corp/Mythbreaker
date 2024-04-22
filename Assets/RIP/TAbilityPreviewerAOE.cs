using System;
using System.Collections;
using BrunoMikoski.ServicesLocation;
using TriInspector;
using UnityEngine;

public class TAbilityPreviewerAOE : MonoBehaviour
{
    [ReadOnly] public TAbility currentlyPreviewing;
    
    public GameObject vfxMarkerPrefab;

    private bool _canUse;
    private bool _isAiming;
    private GameObject _currentVfxMarker;
    private KeyCode _key;
    
    private event Action<TAbility, Vector3> onSendPoint;
    
    private void Start()
    {
        _currentVfxMarker = Instantiate(vfxMarkerPrefab, transform);
        _currentVfxMarker.SetActive(false);
    }

    public void AddDelegate(TAbility ab, Action<TAbility, Vector3> abilitySender)
    {
        onSendPoint += abilitySender;
        currentlyPreviewing = ab;
    }

    private void Update()
    {
        if (_canUse && _isAiming)
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
            {
                _currentVfxMarker.SetActive(true);
                _currentVfxMarker.transform.position = hit.point;
            }
            else
            {
                _currentVfxMarker.SetActive(false);
            }
        }
        else
        {
            _currentVfxMarker.SetActive(false);
        }
    }

    public void Activate()
    {
        _canUse = true;
        StartCoroutine(AbilityCast());
    }
    
    private IEnumerator AbilityCast()
    {
        _isAiming = true;
        _currentVfxMarker.SetActive(true);
        yield return new WaitUntil(() => Input.GetKeyUp(ServiceLocator.Instance.
            GetInstance<AbilityManager>().GetAbilityKey(currentlyPreviewing)));
        onSendPoint?.Invoke(currentlyPreviewing, _currentVfxMarker.transform.position);
        yield return new WaitForSeconds(1f);
        _isAiming = false;
    }
}