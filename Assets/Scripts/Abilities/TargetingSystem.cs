using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Abilities
{
    public class TargetingSystem : Singleton<TargetingSystem>
    {
        public List<GameObject> screenTargets = new();
        public bool isLocked;
        public Transform target;
        
        [Header("Canvas")]
        public Image aim;
        public Image lockAim;
        public Vector2 uiOffset;

        private void Start()
        {
            GetComponent<WaveFinal>().EnemySpawn += o => screenTargets.Add(o);
        }

        private void Update()
        {
            if (screenTargets.Count < 1)
                return;

            if (isLocked)
            {
                UserInterface();
                int targetIndex = TargetIndex();
                if (targetIndex >= 0 && targetIndex < screenTargets.Count && screenTargets[targetIndex] != null)
                {
                    target = screenTargets[targetIndex].transform;
                }
            }

            if (!isLocked)
            {
                int targetIndex = TargetIndex();
                if (targetIndex >= 0 && targetIndex < screenTargets.Count && screenTargets[targetIndex] != null)
                {
                    target = screenTargets[targetIndex].transform;
                }
            }
        }

        public void Activate()
        {
            LockInterface(true);
            isLocked = true;
        }

        public Transform Deactivate()
        {
            LockInterface(false);
            isLocked = false;
            return target;
        }
        
        private void UserInterface()
        {
            if (target != null)
            {
                aim.transform.position = Camera.main.WorldToScreenPoint(target.position + (Vector3)uiOffset);
                Color c = screenTargets.Count < 1 ? Color.clear : Color.white;
                aim.color = c;
            }
        }

        void LockInterface(bool state)
        {
            float size = state ? .5f : 1;
            float fade = state ? 1 : 0;
            lockAim.DOFade(fade, .15f);
            aim.DOFade(fade, .15f);
            lockAim.transform.DOScale(size, .15f).SetEase(Ease.OutBack);
            lockAim.transform.DORotate(Vector3.forward * 180, .15f, RotateMode.FastBeyond360).From();
            aim.transform.DORotate(Vector3.forward * 90, .15f, RotateMode.LocalAxisAdd);
        }
        
        public int TargetIndex()
        {
            if (screenTargets.Count == 0)
                return -1;

            float[] distances = new float[screenTargets.Count];
            for (int i = 0; i < screenTargets.Count; i++)
            {
                if (screenTargets[i] != null)
                {
                    distances[i] = Vector2.Distance(Camera.main.WorldToScreenPoint(screenTargets[i].transform.position), 
                    new Vector2(Screen.width / 2, Screen.height / 2));
                }
                else
                {
                    distances[i] = float.MaxValue;
                }
            }

            float minDistance = Mathf.Min(distances);
            for (int i = 0; i < distances.Length; i++)
            {
                if (minDistance == distances[i])
                    return i;
            }

            return -1;
        } 
    }
}
