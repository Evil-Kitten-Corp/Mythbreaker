using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackControl : MonoBehaviour
{
    [Header("Debug")] public float blastForce = 1;
    public Collider weaponCollider;
    
    private Animator anim;
    internal event Action OnAttackSuccessful;
    internal event Action LightAttackAllowed;
    internal event Action HeavyAttackAllowed;
    
    [Header("Debug++")]
    private int attackCount = 0;
    private float lastAttackTime = 0f;
    public float attackWindow = 1.0f;
    public float clickThreshold = 0.3f; // Adjust this threshold as needed
    public float cooldownDuration = 1.0f; // Adjust this cooldown duration as needed
    private bool isCoolingDown = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetTrigger("Attack");
            EnableWeapon();
            LightAttackAllowed?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            anim.SetTrigger("HeavyAttack");
            EnableWeapon();
            HeavyAttackAllowed?.Invoke();
        }
    }

    public void EnableWeapon()
    {
        weaponCollider.enabled = true;
    }

    public void DisableWeapon()
    {
        weaponCollider.enabled = false;
    }
    
    private void AltAltUpdate()
    {
        // Check for attack input
        if (Input.GetMouseButtonDown(0))
        {
            // Calculate time since last attack
            float timeSinceLastAttack = Time.time - lastAttackTime;

            // Check if the player is in an attackable state
            if (CanAttack())
            {
                // Reset attack count if the attack window has passed
                if (timeSinceLastAttack > attackWindow)
                {
                    attackCount = 0;
                }

                // Increment attack count
                attackCount++;

                // Perform attack based on attack count
                anim.SetTrigger("Attack");

                // Update last attack time
                lastAttackTime = Time.time;
            }
        }
    }

    private bool CanAttack()
    {
        // Check if the player is currently in an attackable state
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        return !stateInfo.IsTag("Attack"); // Assuming attack animations are tagged with "Attack"
    }
    
    void XUpdate()
    {
        if (!isCoolingDown && Input.GetKeyDown(KeyCode.Mouse0))
        {
            float currentTime = Time.time;
            if (currentTime - lastAttackTime <= clickThreshold)
            {
                // Button smashing detected, ignore this click
                return;
            }

            lastAttackTime = currentTime;
            attackCount++;

            
                anim.SetTrigger("Attack");
                LightAttackAllowed?.Invoke();
            
        }
        
        if (!isCoolingDown && Input.GetKeyDown(KeyCode.Mouse1))
        {
            float currentTime = Time.time;
            if (currentTime - lastAttackTime <= clickThreshold)
            {
                // Button smashing detected, ignore this click
                return;
            }

            lastAttackTime = currentTime;
            attackCount++;

                anim.SetTrigger("HeavyAttack");
                HeavyAttackAllowed?.Invoke();
            
        }
    }

    IEnumerator StartCooldown()
    {
        isCoolingDown = true;
        yield return new WaitForSeconds(cooldownDuration);
        attackCount = 0;
        isCoolingDown = false;
    }

    public void SetEndCombo()
    {
        attackCount = 4;
        StartCoroutine(StartCooldown());
    }

    public void Blast()
    {
        Collider[] objsHit = Physics.OverlapSphere(transform.position, 50);

        foreach (var t in objsHit)
        {
            Rigidbody rb = t.GetComponent<Rigidbody>();

            if (!rb)
            {
                continue;
            }

            Vector3 direction = (t.transform.position - transform.position).normalized;
            Animator anim = rb.GetComponent<Animator>();
            
            if (anim != null)
            {
               anim.SetTrigger("KnockUp");  
            }
            
            rb.AddForce(direction * blastForce, ForceMode.Impulse);
        }
    }

    protected internal virtual void AttackSuccessful()
    {
        OnAttackSuccessful?.Invoke();
    }
}
