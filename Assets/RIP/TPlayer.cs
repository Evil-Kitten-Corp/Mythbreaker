using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Invector.vCharacterController;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TPlayer : TCombatEntity
{
    public event Action OnPerformAttack;

    private int _combo;
    public float comboResetTime = 1.5f;
    private float lastClickTime = 0f;

    public override void Start()
    {
        base.Start();
        //OnPerformAttack += PerformAttack;
    }

    private void PerformAttack()
    {
        anim.SetTrigger(Attack);
        _canAttack = false;
        StartCoroutine(MAttack());
    }

    IEnumerator MAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
        _canAttack = true;
    }

    public override void Update()
    {
        base.Update();
        
        //if (Input.GetMouseButton(0) && _canAttack)
        //{
        //    OnPerformAttack?.Invoke();
        //}
        
        /*if (Input.GetMouseButtonDown(0)) 
        {
            anim.SetInteger("LightAttack", _combo);
            anim.SetTrigger("LATrigger");
            
            _combo++;
            Debug.Log(_combo);
            lastClickTime = Time.time;
            OnPerformAttack?.Invoke();
        }
        
        if (Input.GetMouseButtonDown(1)) 
        {
            anim.SetInteger("HeavyAttack", _combo);
            anim.SetTrigger("HATrigger");
            
            _combo++;
            Debug.Log(_combo);
            lastClickTime = Time.time;
            OnPerformAttack?.Invoke();
        }
        
        if (Time.time - lastClickTime > comboResetTime)
        {
            _combo = 0; // Reset combo
            Debug.Log(_combo);
        }*/
    }

    public void AddForceForward()
    {
        Debug.Log("Called force add!");
        //vThirdPersonController tpc = GetComponent<vThirdPersonController>();
        //transform.DOMove(transform.position + new Vector3(5,0,.3f), .5f).SetEase(Ease.InExpo);
        //tpc.MoveCharacter(Vector3.forward * 2);

    }

}

