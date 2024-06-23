using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Refactored.Utilities
{
    // Enemy crowding the player
    [DefaultExecutionOrder(-1)]
    public class TargetDistributor : MonoBehaviour
    {
        public class TargetFollower
        {
            public bool RequireSlot;
            public int AssignedSlot;
            public Vector3 RequiredPoint;

            public TargetDistributor Distributor;

            public TargetFollower(TargetDistributor owner)
            {
                Distributor = owner;
                RequiredPoint = Vector3.zero;
                RequireSlot = false;
                AssignedSlot = -1;
            }
        }

        public int arcsCount;

        private Vector3[] _worldDirection;

        private bool[] _freeArcs;
        private float _arcDegree;

        private List<TargetFollower> _followers;

        public void OnEnable()
        {
            _worldDirection = new Vector3[arcsCount];
            _freeArcs = new bool[arcsCount];

            _followers = new List<TargetFollower>();

            _arcDegree = 360.0f / arcsCount;
            Quaternion rotation = Quaternion.Euler(0, -_arcDegree, 0);
            Vector3 currentDirection = Vector3.forward;
            for (int i = 0; i < arcsCount; ++i)
            {
                _freeArcs[i] = true;
                _worldDirection[i] = currentDirection;
                currentDirection = rotation * currentDirection;
            }
        }

        public TargetFollower RegisterNewFollower()
        {
            TargetFollower follower = new TargetFollower(this);
            _followers.Add(follower);
            return follower;
        }

        public void UnregisterFollower(TargetFollower follower)
        {
            if (follower.AssignedSlot != -1)
            {
                _freeArcs[follower.AssignedSlot] = true;
            }
            
            _followers.Remove(follower);
        }

        private void LateUpdate()
        {
            foreach (var follower in _followers)
            {
                if (follower.AssignedSlot != -1)
                {
                    _freeArcs[follower.AssignedSlot] = true;
                }

                if (follower.RequireSlot)
                {
                    follower.AssignedSlot = GetFreeArcIndex(follower);
                }
            }
        }

        public Vector3 GetDirection(int index)
        {
            return _worldDirection[index];
        }

        public int GetFreeArcIndex(TargetFollower follower)
        {
            bool found = false;

            Vector3 wanted = follower.RequiredPoint - transform.position;
            Vector3 rayCastPosition = transform.position + Vector3.up * 0.4f;

            wanted.y = 0;
            float wantedDistance = wanted.magnitude;

            wanted.Normalize();

            float angle = Vector3.SignedAngle(wanted, Vector3.forward, Vector3.up);
            if (angle < 0)
                angle = 360 + angle;

            int wantedIndex = Mathf.RoundToInt(angle / _arcDegree);
            if (wantedIndex >= _worldDirection.Length)
                wantedIndex -= _worldDirection.Length;

            int choosenIndex = wantedIndex;

            if (!Physics.Raycast(rayCastPosition, GetDirection(choosenIndex), out _, wantedDistance))
                found = _freeArcs[choosenIndex];

            if (!found)
            {//we are going to test left right with increasing offset
                int offset = 1;
                int halfCount = arcsCount / 2;
                while (offset <= halfCount)
                {
                    int leftIndex = wantedIndex - offset;
                    int rightIndex = wantedIndex + offset;

                    if (leftIndex < 0) leftIndex += arcsCount;
                    if (rightIndex >= arcsCount) rightIndex -= arcsCount;

                    if (!Physics.Raycast(rayCastPosition, GetDirection(leftIndex), wantedDistance) &&
                        _freeArcs[leftIndex])
                    {
                        choosenIndex = leftIndex;
                        found = true;
                        break;
                    }

                    if (!Physics.Raycast(rayCastPosition, GetDirection(rightIndex), wantedDistance) &&
                        _freeArcs[rightIndex])
                    {
                        choosenIndex = rightIndex;
                        found = true;
                        break;
                    }

                    offset += 1;
                }
            }

            if (!found)
            {
                return -1;
            }

            _freeArcs[choosenIndex] = false;
            return choosenIndex;
        }

        public void FreeIndex(int index)
        {
            _freeArcs[index] = true;
        }
    }
}