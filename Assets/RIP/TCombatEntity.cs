using System;
using System.Collections;
using Ability_Behaviours.UI;
using BrunoMikoski.ServicesLocation;
using TriInspector;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

[DeclareFoldoutGroup("Foldout", Title = "VFX")]
public abstract class TCombatEntity : MonoBehaviour
{
    public Animator anim;
    public float maxHealth;
    protected internal float _health;
    public float attackCooldown;
    
    [GroupNext("Foldout")] 
    [Title("Death", HorizontalLine = true)] 
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public VisualEffect DeathVFX;
    public float dissolveRate = 0.05f;
    public float refreshRate = 0.1f;
    public float dieDelay = 0.25f;
    
    private bool _alive = true;
    private Material[] _dissolveMaterials;
    protected bool _canAttack = true;

    internal static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Death = Animator.StringToHash("Death");
    private static readonly int DissolveAmount = Shader.PropertyToID("DissolveAmount_");
    private static readonly int Hit = Animator.StringToHash("Hit");

    public event Action OnDeath;

    public virtual void Start()
    {
        _health = maxHealth;
        
        if (DeathVFX != null)
        {
            DeathVFX.Stop();
            DeathVFX.gameObject.SetActive(false);
        }

        if (skinnedMeshRenderer != null)
            _dissolveMaterials = skinnedMeshRenderer.materials;

        OnDeath += Die;
    }

    public virtual void Update()
    {
    }
    
    public void TakeDamage(float dmg)
    {
        _health -= dmg;
        Vector3 randomness = new Vector3(Random.Range(0f, 0.55f), Random.Range(0f, 0.55f), Random.Range(0f, 0.55f));
        
        ServiceLocator.Instance.GetInstance<DamagePopUpGenerator>().CreatePopUp(transform.position + 
            randomness, dmg.ToString(), Color.yellow);

        if (anim != null)
        {
            anim.SetTrigger(Hit);
        }

        if (_health <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    public void Die()
    {
        StartCoroutine(DissolveCo());
    }

    IEnumerator DissolveCo()
    {
        _alive = false;

        if (anim != null)
        {
            anim.SetTrigger(Death);
        }
        else
        {
            Debug.Log("No Animator assigned in the inspector.");
            _alive = true;
            yield break;
        }

        yield return new WaitForSeconds(dieDelay);
        
        if (DeathVFX != null)
        {
            DeathVFX.gameObject.SetActive(true);
            DeathVFX.Play();
        }

        float counter = 0; 

        if (_dissolveMaterials.Length > 0)
        {   
            while (_dissolveMaterials[0].GetFloat(DissolveAmount) < 1)
            {
                counter += dissolveRate;
                
                foreach (var t in _dissolveMaterials)
                    t.SetFloat(DissolveAmount, counter);

                yield return new WaitForSeconds(refreshRate);
            }
        }
        
        Destroy(gameObject, 3f);
    }
}
