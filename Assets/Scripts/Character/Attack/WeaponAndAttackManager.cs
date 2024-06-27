using System;
using Invector.vCharacterController;
using System.Collections;
using UnityEngine;

public class WeaponAndAttackManager : MonoBehaviour
{
    public GameObject weapon;
    public CameraShake cameraShaker;
    public vThirdPersonController moveController;
    
    [Header("Settings")] 
    public float blastForce = 1;
    public Collider weaponCollider;
    
    private Animator _anim;
    internal event Action OnAttackSuccessful;
    internal event Action LightAttackAllowed;
    internal event Action HeavyAttackAllowed;
    
    [Header("Debug++")]
    private int attackCount = 0;
    private float lastAttackTime = 0f;
    public float cooldownDuration = 1.0f; // Adjust this cooldown duration as needed
    private bool isCoolingDown = false;
    private bool _controllable = true;
    
    [Header("Attack Data")]
    public AttackData[] lightAttackData;  // Array of light attack data
    public AttackData[] heavyAttackData;  // Array of heavy attack data
    private int lightAttackIndex = 0;
    private int heavyAttackIndex = 0;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_controllable)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                _anim.SetTrigger("Attack");
                SetCurrentAttackData(lightAttackData[lightAttackIndex]);
                EnableWeapon();
                LightAttackAllowed?.Invoke();
                lightAttackIndex = (lightAttackIndex + 1) % lightAttackData.Length;  // Cycle through light attack data
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                _anim.SetTrigger("HeavyAttack");
                SetCurrentAttackData(heavyAttackData[heavyAttackIndex]);
                EnableWeapon();
                HeavyAttackAllowed?.Invoke();
                heavyAttackIndex = (heavyAttackIndex + 1) % heavyAttackData.Length;  // Cycle through heavy attack data
            }
        }
    }
    
    private void SetCurrentAttackData(AttackData data)
    {
        weapon.GetComponent<WeaponAttributes>().currentAttackData = data;
    }

    public void SetControllable(bool b)
    {
        _controllable = b;
    }
    
    public void ShakeCamera()
    {
        cameraShaker.TriggerShake();
    }

    public void EnableWeapon()
    {
        weaponCollider.enabled = true;
    }

    public void DisableWeapon()
    {
        weaponCollider.enabled = false;
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
            EnemyBase bh = rb.GetComponent<EnemyBase>();

            if (bh != null)
            {
                bh.AddForce(direction * blastForce, true);
            }
        }
    }

    protected internal virtual void AttackSuccessful()
    {
        OnAttackSuccessful?.Invoke();
    }

    public void EnableWeaponCollider(int isEnable)
    {
        if (weapon != null)
        {
            var col = weapon.GetComponent<Collider>();

            if (col != null)
            {
                col.enabled = isEnable == 1;
            }
        }
    }

    public void EnableMovement(bool enable)
    {
        if(enable == false)
        {
            moveController.lockMovement = true;
            moveController.lockRotation = true;
        }
        else
        {
            moveController.lockMovement = false;
            moveController.lockRotation = false;
        }
    }
}
