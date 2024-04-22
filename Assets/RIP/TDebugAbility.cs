using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.VFX.Utility;

public class TDebugAbility : TAbility
{
    private vThirdPersonController _tpc;
    private bool _locked;
    private TEnemy _t;

    private void Start()
    {
        _tpc = GameObject.FindWithTag("Player").GetComponent<vThirdPersonController>();
    }

    private void Update()
    {
        if (_locked)
        {
            _tpc.transform.LookAt(_t.transform);
        }
    }

    public override void Use(TEnemy t)
    {
        Debug.Log($"Used Debug Ability targeting {t.name}.");

        _t = t;
        _tpc.lockMovement = true;
        _tpc.GetComponent<Animator>().applyRootMotion = true;
        base.Use(t);
        StartCoroutine(RestoreMovement(t.transform));
    }

    private IEnumerator RestoreMovement(Transform t)
    {
        _locked = true;
        yield return new WaitForSeconds(2f);
        _tpc.lockMovement = false;
        _locked = false;
    }
    
    public override void Use(Vector3 v)
    {
        Debug.Log($"Used Debug Ability in {v} location.");
        base.Use(v);
    }

    public override void Use()
    {
        Debug.Log($"Used simple Debug Ability.");
        base.Use();
    }

    public override IEnumerator Use(bool isRoutine)
    {
        Debug.Log($"Used Debug Ability in coroutine");
        return base.Use(isRoutine);
    }
}