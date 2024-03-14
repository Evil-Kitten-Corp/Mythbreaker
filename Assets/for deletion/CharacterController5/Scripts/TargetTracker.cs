using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace CharacterController5.Scripts
{
    public class TargetTracker : MonoBehaviour
    {
        [FormerlySerializedAs("targettingRadius")] [SerializeField]
        private float targetingRadius = 5;
 
        [FormerlySerializedAs("targettingLayer")] [SerializeField] 
        private LayerMask targetingLayer;

        private Collider[] _hits;
        [FormerlySerializedAs("targettingList")] public List<Collider> targetingList = new();
 
        [FormerlySerializedAs("ActiveTarget")] public Collider activeTarget;

        private int _targetIndex;
        public bool lockedOn;

        private GameObject _targetFollow;

        private CinemachineTargetGroup _targetGroup;
 
        // Start is called before the first frame update
        private void Start()
        {
            // create an empty gameobject for the targetgroup that follows the activetarget
            _targetFollow = new GameObject
            {
                name = "TargetFollow"
            };

            _targetGroup = FindObjectOfType<CinemachineStateDrivenCamera>().GetComponentInChildren<CinemachineTargetGroup>();
            _targetGroup.AddMember(_targetFollow.transform, 1, 1);
        }
 
        // Update is called once per frame
        private void Update()
        {
            // get all colliders within the range on the targetting layer, it's set up for colliders, but you can
            // change this to look for triggers too
            _hits = Physics.OverlapSphere(transform.position, targetingRadius, targetingLayer,
                QueryTriggerInteraction.Ignore);
            
            // clear the list
            targetingList.Clear();
            
            // add to the list if it's tagged with enemy
            foreach (var t in _hits)
            {
                // only add 
                if (t.CompareTag($"Enemy"))
                {
                    targetingList.Add(t);
                }
            }
            
            // shift key will toggle to targetting mode
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                SetTargeting();
            }
            
            // switch targets
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SwitchTargetLeft();
            }
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                SwitchTargetRight();
            }
            
            // lerp the follow to the active target
            if (lockedOn)
            {
                _targetFollow.transform.position = Vector3.Lerp(_targetFollow.transform.position, 
                    activeTarget.bounds.center, Time.deltaTime * 4f);
            }
        }

        private void SwitchTargetLeft()
        {
            // switch to the left if locked on, don't go under 0
            if (lockedOn)
            {
                SortList();
                if (_targetIndex > 0)
                {
                    _targetIndex -= 1;
                }
                else
                {
                    _targetIndex = 0;
                }
                activeTarget = targetingList[_targetIndex];
            }
        }

        private void SwitchTargetRight()
        {
            // switch to the left if locked on, don't go over max length of targets
            if (lockedOn)
            {
                SortList();
                if (_targetIndex < (targetingList.Count - 1))
                {
                    _targetIndex += 1;
                }
                else
                {
                    _targetIndex = (targetingList.Count - 1);
                }
                activeTarget = targetingList[_targetIndex];
            }
        }

        private void SortList()
        {
            // sort list horizontally on screenposition
            targetingList = targetingList.OrderBy(x => Camera.main!.WorldToScreenPoint(x.transform.position).x).ToList();
        }

        private void SetTargeting()
        {
            // if there is no target, return
            if (!IsThereATarget())
            {
                lockedOn = false;
                return;
            }
            // switch mode
            lockedOn = !lockedOn;
            if (lockedOn)
            {
                activeTarget = ClosestTarget();
            }
        }

        private Collider ClosestTarget()
        {
            // sort by distance from this transform, get closest target
            targetingList = targetingList.OrderBy(x => Vector3.Distance(transform.position, 
                x.transform.position)).ToList();
            Collider bestTarget = targetingList[0];
            return bestTarget;
        }

        private bool IsThereATarget()
        {
            // check if there is anything in the list
            return targetingList.Count > 0;
        }
    }
}