using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace not_this_again.Weapons
{
    [Serializable]
    public struct RandomAudioPlay
    {
        public AudioClip[] clips;
        
        public bool randomizePitch;
        public float pitchRandomRange;
        public float playDelay;
        
        [HideInInspector] public bool playing;
        [HideInInspector] public bool canPlay;

        public AudioSource audioSource;

        public AudioClip PlayRandomClip()
        {
            if (clips == null || clips.Length == 0)
                return null;
            
            var clip = clips[Random.Range(0, clips.Length)];

            if (clip == null)
                return null;

            audioSource.pitch = randomizePitch ? Random.Range(1.0f - pitchRandomRange, 1.0f + pitchRandomRange) : 1.0f;
            audioSource.clip = clip;
            audioSource.PlayDelayed(playDelay);

            return clip;
        }
    }
}