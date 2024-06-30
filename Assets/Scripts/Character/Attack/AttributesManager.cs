using System;
using System.Collections;
using DefaultNamespace;
using Invector.vCharacterController;
using Minimalist.Bar.Quantity;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class AttributesManager : MonoBehaviour
{
    public vThirdPersonController moveController;
    public vThirdPersonCamera cameraController;
    public GameObject deathScreen;
    public QuantityBhv health;

    public int Attack
    {
        get => attackManager.baseAttack;
        set => attackManager.baseAttack = value;
    }

    private bool _invulnerable;
    private bool _dead;
    private bool _canBeKnockedUp = true;
    
    public KeyCode keyToRecenterCamera;
    public WeaponAndAttackManager attackManager;

    public Action OnDie;
    public Action OnDefeatLastWave;
    public Action OnBossMeet;
    public Action OnDefeatBoss;
    
    public Animator anim;
    
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Die = Animator.StringToHash("Die");
    private static readonly int Knockup1 = Animator.StringToHash("Knockup");
    private static readonly int OpenOrClose = Animator.StringToHash("OpenOrClose");

    [Title("Warehouse Objects")] 
    public GameObject[] warehouseObjects;
    public GameObject triggeredWall;

    [Title("Final Cutscene")] 
    public CanvasGroup fadeInGroup;
    public TMP_Text subtitle;
    //public AudioClip clip;

    private AudioSource _source;
    private SaveLoadJsonFormatter _saveSystem;

    private void Start()
    {
        OnDefeatBoss += OnWin;
        OnBossMeet += OnLastWave;
        _source = GetComponent<AudioSource>();
        
        _saveSystem = FindObjectOfType<SaveLoadJsonFormatter>();

        LoadGame();
    }

    void LoadGame()
    {
        _saveSystem.LoadGame(out var att, out var h, out var sp);

        if (att != 0)
        {
            Attack = att;
        }

        if (h != 0)
        {
            health.MaximumAmount = h;
        }

        if (sp != 0)
        {
            moveController.freeSpeed.walkSpeed = sp;
        }
    }

    private void OnWin()
    {
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        yield return StartCoroutine(FadeCanvasGroup(fadeInGroup, 0, 1, 2f));
        yield return new WaitForSeconds(1f);
        //_source.PlayOneShot(clip);

        yield return new WaitForSeconds(3f);
        subtitle.text = "Thank you for playing.";
        yield return new WaitForSeconds(3f);

        MythUIElement.IsInUI = true;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }
    
    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, (Time.time - startTime) / duration);
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
    }

    private IEnumerator FadeTMPText(TMP_Text text, float startAlpha, float endAlpha, float duration)
    {
        float startTime = Time.time;
        Color color = text.color;
        while (Time.time < startTime + duration)
        {
            color.a = Mathf.Lerp(startAlpha, endAlpha, (Time.time - startTime) / duration);
            text.color = color;
            yield return null;
        }
        color.a = endAlpha;
        text.color = color;
    }

    private void OnLastWave()
    {
        foreach (var go in warehouseObjects)
        {
            Animator tAnim = go.GetComponent<Animator>();

            if (tAnim != null)
            {
                tAnim.SetTrigger(OpenOrClose);
            }
        }

        if (triggeredWall != null)
        {
            triggeredWall.SetActive(true);
        }
    }

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
            if (_dead) return; 
            Death();
        }
    }

    private void Death()
    {
        OnDie?.Invoke();
        moveController.lockMovement = true;
        moveController.lockRotation = true; 
        cameraController.lockCamera = true;
        anim.SetTrigger(Die);
        _dead = true;
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
        
        anim.SetBool("Dead", _dead);
        
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

    public void SetKnockUp(bool val)
    {
        _canBeKnockedUp = val;
    }

    public bool IsInvulnerable() => _invulnerable;
    public bool CanStomp() => _canBeKnockedUp;

    public void AddTemporaryBuff(int quantity, float time)
    {
        var tpc = GetComponent<vThirdPersonController>();
        tpc.jumpHeight += quantity;
        
        StartCoroutine(JumpBuff());

        IEnumerator JumpBuff()
        {
            yield return new WaitForSeconds(time);
            tpc.jumpHeight -= quantity;
        }
    }

    public void Knockup()
    {
        anim.SetTrigger(Knockup1);
        StartCoroutine(KnockupCc());

        IEnumerator KnockupCc()
        {
            moveController.lockMovement = true;
            attackManager.SetControllable(false);
            yield return new WaitForSeconds(2f);
            moveController.lockMovement = false;
            attackManager.SetControllable(true);
        }
        
    }
}