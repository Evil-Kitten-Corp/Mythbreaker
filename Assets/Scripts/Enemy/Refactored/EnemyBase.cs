using System;
using Minimalist.Bar.Quantity;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[DefaultExecutionOrder(-1)]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBase : MonoBehaviour
{
    public bool interpolateTurning;
    public bool applyAnimationRotation;

    public Animator Animator => Anim;
    public Vector3 ExternalForce => ExtForce;
    public NavMeshAgent NavmeshAgent => NavMeshAgt;
    public bool FollowNavmeshAgent => FollowNavmeshAgt;
    public bool Grounded => Ground;

    protected NavMeshAgent NavMeshAgt;
    protected bool FollowNavmeshAgt;
    protected Animator Anim;
    protected bool UnderExtForce;
    protected bool ExtForceAddGravity = true;
    protected Vector3 ExtForce;
    protected bool Ground;

    protected Rigidbody Rigidbody;

    const float GroundedRayDistance = .8f;

    public QuantityBhv healthPoints;
    public float invulnerabiltyTime;
    
    [Range(0.0f, 360.0f)] public float hitAngle = 360.0f;
    [Range(0.0f, 360.0f)] public float hitForwardRotation = 360.0f;

    public bool IsInvulnerable { get; set; }
    
    public float CurrentHitPoints 
    { 
        get => healthPoints.Amount;
        private set => healthPoints.Amount = value;
    }

    public UnityEvent OnDeath, OnReceiveDamage, OnHitWhileInvulnerable, OnBecomeVulnerable, OnResetDamage;

    protected float _timeSinceLastHit = 0.0f;
    protected Collider Collider;
    
    Action _schedule;
    
    void OnEnable()
    {
        NavMeshAgt = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
        Anim.updateMode = AnimatorUpdateMode.AnimatePhysics;

        NavMeshAgt.updatePosition = false;

        Rigidbody = GetComponentInChildren<Rigidbody>();
        if (Rigidbody == null)
            Rigidbody = gameObject.AddComponent<Rigidbody>();

        Rigidbody.isKinematic = true;
        Rigidbody.useGravity = false;
        Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

        FollowNavmeshAgt = true;
    }

    private void Start()
    {
        ResetDamage();
        Collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (IsInvulnerable)
        {
            _timeSinceLastHit += Time.deltaTime;
            if (_timeSinceLastHit > invulnerabiltyTime)
            {
                _timeSinceLastHit = 0.0f;
                IsInvulnerable = false;
                OnBecomeVulnerable.Invoke();
            }
        }
    }

    private void FixedUpdate()
    {
        CheckGrounded();

        if (UnderExtForce)
        {
            ForceMovement();
        }
        
        if (_schedule != null)
        {
            _schedule();
            _schedule = null;
        }
    }
    
    public void ResetDamage()
    {
        healthPoints.Amount = healthPoints.MaximumAmount;
        IsInvulnerable = false;
        _timeSinceLastHit = 0.0f;
        OnResetDamage.Invoke();
    }

    public void SetColliderState(bool enabled)
    {
        Collider.enabled = enabled;
    }

    public void ApplyDamage(Transform damageSource, float dmg)
    {
        if (CurrentHitPoints <= 0)
        {
            return;
        }

        if (IsInvulnerable)
        {
            OnHitWhileInvulnerable.Invoke();
            return;
        }

        Vector3 forward = transform.forward;
        forward = Quaternion.AngleAxis(hitForwardRotation, transform.up) * forward;

        //we project the direction to damager to the plane formed by the direction of damage
        Vector3 positionToDamager = damageSource.position - transform.position;
        positionToDamager -= transform.up * Vector3.Dot(transform.up, positionToDamager);

        if (Vector3.Angle(forward, positionToDamager) > hitAngle * 0.5f)
            return;

        IsInvulnerable = true;
        CurrentHitPoints -= dmg;

        if (CurrentHitPoints <= 0)
            _schedule += OnDeath.Invoke;
        else
            OnReceiveDamage.Invoke();
    }

    void CheckGrounded()
    {
        Ray ray = new Ray(transform.position + Vector3.up * GroundedRayDistance * 0.5f, -Vector3.up);
        Ground = Physics.Raycast(ray, out _, GroundedRayDistance, Physics.AllLayers,
            QueryTriggerInteraction.Ignore);
    }

    void ForceMovement()
    {
        if (ExtForceAddGravity)
        {
            ExtForce += Physics.gravity * Time.deltaTime;
        }

        Vector3 movement = ExtForce * Time.deltaTime;
        
        if (!Rigidbody.SweepTest(movement.normalized, out _, movement.sqrMagnitude))
        {
            Rigidbody.MovePosition(Rigidbody.position + movement);
        }

        NavMeshAgt.Warp(Rigidbody.position);
    }

    private void OnAnimatorMove()
    {
        if (UnderExtForce)
            return;

        if (FollowNavmeshAgt)
        {
            NavMeshAgt.speed = (Anim.deltaPosition / Time.deltaTime).magnitude;
            transform.position = NavMeshAgt.nextPosition;
        }
        else
        {
            if (!Rigidbody.SweepTest(Anim.deltaPosition.normalized, out _, Anim.deltaPosition.sqrMagnitude))
            {
                Rigidbody.MovePosition(Rigidbody.position + Anim.deltaPosition);
            }
        }

        if (applyAnimationRotation)
        {
            transform.forward = Anim.deltaRotation * transform.forward;
        }
    }

    public void SetFollowNavmeshAgent(bool follow)
    {
        if (!follow && NavMeshAgt.enabled)
        {
            NavMeshAgt.ResetPath();
        }
        else if (follow && !NavMeshAgt.enabled)
        {
            NavMeshAgt.Warp(transform.position);
        }

        FollowNavmeshAgt = follow;
        NavMeshAgt.enabled = follow;
    }

    public void AddForce(Vector3 force, bool useGravity = true)
    {
        if (NavMeshAgt.enabled)
            NavMeshAgt.ResetPath();

        ExtForce = force;
        NavMeshAgt.enabled = false;
        UnderExtForce = true;
        ExtForceAddGravity = useGravity;
    }

    public void ClearForce()
    {
        UnderExtForce = false;
        NavMeshAgt.enabled = true;
    }

    public void SetForward(Vector3 forward)
    {
        Quaternion targetRotation = Quaternion.LookRotation(forward);

        if (interpolateTurning)
        {
            targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                NavMeshAgt.angularSpeed * Time.deltaTime);
        }

        transform.rotation = targetRotation;
    }

    public bool SetTarget(Vector3 position)
    {
        return NavMeshAgt.SetDestination(position);
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 forward = transform.forward;
        forward = Quaternion.AngleAxis(hitForwardRotation, transform.up) * forward;

        if (Event.current.type == EventType.Repaint)
        {
            UnityEditor.Handles.color = Color.blue;
            UnityEditor.Handles.ArrowHandleCap(0, transform.position, Quaternion.LookRotation(forward), 1.0f,
                EventType.Repaint);
        }


        UnityEditor.Handles.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        forward = Quaternion.AngleAxis(-hitAngle * 0.5f, transform.up) * forward;
        UnityEditor.Handles.DrawSolidArc(transform.position, transform.up, forward, hitAngle, 1.0f);
    }
#endif
}
