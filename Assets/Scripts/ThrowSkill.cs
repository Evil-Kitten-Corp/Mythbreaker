using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ThrowSkill: MonoBehaviour
{ 
    public Animator animator;
    private Rigidbody weaponRb;
    private TWeaponScript weaponScript;
    private float returnTime;

    private Vector3 origLocPos;
    private Vector3 origLocRot;
    private Vector3 pullPosition;

    [Header("Public References")]
    public Transform weapon;
    public Transform hand;
    public Transform spine;
    public Transform curvePoint;
        
    [Space]
        
    [Header("Parameters")]
    public float throwPower = 30;
    public float cameraZoomOffset = .3f;
        
    [Space]
        
    [Header("Bools")]
    public bool walking = true;
    public bool aiming = false;
    public bool hasWeapon = true;
    public bool pulling = false;
        
    [Space]
        
    [Header("Particles and Trails")]
    public ParticleSystem catchParticle;
    public ParticleSystem trailParticle;
    public TrailRenderer trailRenderer;
        
    [Space]
    public CameraShaker cameraShake;

    void Start()
    {
        weaponRb = weapon.GetComponent<Rigidbody>();
        weaponScript = weapon.GetComponent<TWeaponScript>();
            
        origLocPos = weapon.localPosition;
        origLocRot = weapon.localEulerAngles;
    }

    void Update()
    { 
            
        animator.SetBool("Pulling", pulling);

        if (hasWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                animator.SetTrigger("Throw");
                Debug.Log("Triggered throw.");
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                WeaponStartPull();
                animator.SetTrigger("Pull");
            }
        }

        if (pulling)
        {
            if (returnTime < 1)
            {
                weapon.position = GetQuadraticCurvePoint(returnTime, pullPosition, curvePoint.position, 
                    hand.position);
                returnTime += Time.deltaTime * 1.5f;
            }
            else
            {
                WeaponCatch();
            }
        }
    }

    public void WeaponThrow()
    {
        hasWeapon = false;
        weaponScript.activated = true;
        weaponRb.isKinematic = false;
        weaponRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        weapon.parent = null;
        weapon.eulerAngles = new Vector3(0, -90 +transform.eulerAngles.y, 0);
        weapon.transform.position += transform.right/5;
        weaponRb.AddForce(Camera.main.transform.forward * throwPower + transform.up * 2, ForceMode.Impulse);

        //Trail
        trailRenderer.emitting = true;
        trailParticle.Play();
    }

    public void WeaponStartPull()
    {
        pullPosition = weapon.position;
        weaponRb.Sleep();
        weaponRb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        weaponRb.isKinematic = true;
        weapon.DORotate(new Vector3(-90, -90, 0), .2f).SetEase(Ease.InOutSine);
        weapon.DOBlendableLocalRotateBy(Vector3.right * 90, .5f);
        weaponScript.activated = true;
        pulling = true;
    }

    public void WeaponCatch()
    {
        returnTime = 0;
        pulling = false;
        weapon.parent = hand;
        weaponScript.activated = false;
        weapon.localEulerAngles = origLocRot;
        weapon.localPosition = origLocPos; 
        hasWeapon = true;

        //Particle and trail
        catchParticle.Play();
        trailRenderer.emitting = false;
        trailParticle.Stop();

        //Shake
        cameraShake.ShakeCamera();

    }

    public Vector3 GetQuadraticCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t; 
        float uu = u * u;
        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }
}