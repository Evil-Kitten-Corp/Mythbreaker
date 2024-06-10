using System;
using System.Collections;
using DefaultNamespace;
using Invector.vCharacterController;
using Minimalist.Bar.Quantity;
using UnityEngine;
using Random = UnityEngine.Random;

public class AttributesManager : MonoBehaviour
{
    public vThirdPersonController moveController;
    public vThirdPersonCamera cameraController;
    public GameObject deathScreen;
    public QuantityBhv health;
    public int attack;

    private bool _invulnerable;
    
    public KeyCode keyToRecenterCamera;
    public WeaponAndAttackManager attackManager;

    public Animator anim;
    
    private static readonly int Hit = Animator.StringToHash("Hit");

    public Action OnDie;
    private static readonly int Die = Animator.StringToHash("Die");

    public void TakeDamage(int amount, bool crit = false)
    {
        if (_invulnerable) return;

        health.Amount -= amount;
        
        Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), 
            Random.Range(0f, 0.25f), Random.Range(0f, 0.25f));
        
        switch (crit)
        {
            case false:
                DamagePopUpGenerator.Current.CreatePopUp(transform.position + randomness, 
                    amount.ToString(), Color.yellow);
                break;
            
            case true:
                DamagePopUpGenerator.Current.CreatePopUp(transform.position + randomness, 
                    amount.ToString(), Color.cyan);
                break;
        }

        if (anim != null)
        {
            anim.SetTrigger(Hit);
        }

        if (health.Amount <= 0)
        {
            StartCoroutine(Death());
        }
    }

    private IEnumerator Death()
    {
        OnDie?.Invoke();
        moveController.lockMovement = true;
        
        anim.SetTrigger(Die);
        yield return new WaitForSeconds(6f);
        Time.timeScale = 0;
        deathScreen.SetActive(true);
    }

    public void DealDamage(GameObject target)
    {
        var atm = target.GetComponent<EnemyBehaviour>();
        
        if (atm != null)
        {
            atm.TakeDamage.Invoke(attack);
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(keyToRecenterCamera))
        {
            Cursor.visible = true;
            cameraController.lockCamera = true;
            attackManager.enabled = false;
        }
        else if (Input.GetKeyUp(keyToRecenterCamera))
        {
            Cursor.visible = false;
            cameraController.lockCamera = false;
            attackManager.enabled = true; 
        }
        
        if (MythUIElement.IsInUI)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    
    private void OnApplicationFocus(bool hasFocus)
    {
        if (MythUIElement.IsInUI)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void SetInvulnerable(bool val)
    {
        _invulnerable = val;
    }

    public bool IsInvulnerable() => _invulnerable;
}