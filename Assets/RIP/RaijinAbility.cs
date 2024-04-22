using System.Collections;
using BrunoMikoski.ServicesLocation;
using LT.Shi;
using UnityEngine;
using UnityEngine.VFX.Utility;

public class RaijinAbility : TAbility
{
    public GameObject player;
    public GameObject vfx;
    public float range;

    private bool _forceLook = false;
    private GameObject _enemy = null;
    private GameObject _myVfx;

    private void Update()
    {
        if (_forceLook)
        {
            player.transform.LookAt(_enemy != null ? _enemy.transform : null);
        }
    }

    public override void Use(Vector3 v)
    {
        Collider[] enemies = Physics.OverlapSphere(v, range);
        //Physics.OverlapSphereNonAlloc(v, range, enemies);

        foreach (var col in enemies)
        {
            if (col.GetComponent<Target>())
            {
                _enemy = col.gameObject;
                break;
            }
        }

        _myVfx = Instantiate(vfx, transform);
        var visualEffects = GetComponentsInChildren<CustomVFXPositionBinder>();
        
        visualEffects[0].Target = player.transform;
        visualEffects[4].Target = player.transform;
        Debug.Log($"Enemy: {_enemy.name}");
        
        visualEffects[3].Target = _enemy.transform;
        visualEffects[7].Target = _enemy.transform;

        _forceLook = true;
        ServiceLocator.Instance.GetInstance<CinemachineController>().ChangeCam();
        
        base.Use(v);
        
        StartCoroutine(StopLooking());
    }

    IEnumerator StopLooking()
    {
        yield return new WaitForSeconds(5f);
        _forceLook = false;
        ServiceLocator.Instance.GetInstance<CinemachineController>().ChangeCam();
        Destroy(_myVfx);
    }
}