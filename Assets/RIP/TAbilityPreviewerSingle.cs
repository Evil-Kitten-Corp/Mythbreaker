using System;
using System.Collections;
using System.Collections.Generic;
using BrunoMikoski.ServicesLocation;
using DG.Tweening;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

public class TAbilityPreviewerSingle : MonoBehaviour
{
    [ReadOnly] public TAbility currentAbility;
    [ReadOnly] public List<Transform> screenTargets = new();
    [ReadOnly] public Transform target;
    public bool isLocked;
    
    [Header("Canvas")]
    public Image aim;
    public Image lockAim;
    public Vector2 uiOffset;

    private bool _isUsing;
    private event Action<TAbility, TEnemy> onSendTarget;

    public event Action<string> onCancelAbility;
    
    public void AddDelegate(TAbility ab, Action<TAbility, TEnemy> abilitySender)
    {
        onSendTarget += abilitySender;
        currentAbility = ab;
    }

    private void Update()
    {
        if (_isUsing)
        {
            UserInterface();

            if (screenTargets.Count < 1)
            {
                onCancelAbility?.Invoke("There aren't any enemies available to use this ability on!");
                LockInterface(false);
                return;
            }

            if (!isLocked)
            {
                target = screenTargets[TargetIndex()];
            }
        }
    }
    
    public void Activate()
    {
        _isUsing = true;
        StartCoroutine(AbilityCast());
    }
    
    private IEnumerator AbilityCast()
    {
        LockInterface(true);
        isLocked = true;
        yield return new WaitUntil(() => Input.GetKeyUp(ServiceLocator.Instance.
            GetInstance<AbilityManager>().GetAbilityKey(currentAbility)));
        LockInterface(false);
        isLocked = false;
        onSendTarget?.Invoke(currentAbility, target.GetComponent<TEnemy>());
        yield return new WaitForSeconds(1f);
        _isUsing = false;
    }
    
    private void UserInterface()
    {
        if (target == null)
        {
            target = screenTargets[TargetIndex()];
        }
        
        if (Camera.main != null)
            aim.transform.position = Camera.main.WorldToScreenPoint(target.position + (Vector3)uiOffset);

        Color c = screenTargets.Count < 1 ? Color.clear : Color.white;
        aim.color = c;
    }

    private void LockInterface(bool state)
    {
        float size = state ? 1 : 2;
        float fade = state ? 1 : 0;
        lockAim.DOFade(fade, .15f);
        lockAim.transform.DOScale(size, .15f).SetEase(Ease.OutBack);
        lockAim.transform.DORotate(Vector3.forward * 180, .15f, RotateMode.FastBeyond360).From();
        aim.DOFade(fade, .15f);
        aim.transform.DORotate(Vector3.forward * 90, .15f, RotateMode.LocalAxisAdd);
    }

    private int TargetIndex()
    {
        float[] distances = new float[screenTargets.Count];

        for (int i = 0; i < screenTargets.Count; i++)
        {
            if (Camera.main != null)
                distances[i] = Vector2.Distance(Camera.main.WorldToScreenPoint(screenTargets[i].position),
                    new Vector2(Screen.width / 2, Screen.height / 2));
        }

        float minDistance = Mathf.Min(distances);
        int index = 0;

        for (int i = 0; i < distances.Length; i++)
        {
            if (minDistance == distances[i])
                index = i;
        }

        return index;
    }
}