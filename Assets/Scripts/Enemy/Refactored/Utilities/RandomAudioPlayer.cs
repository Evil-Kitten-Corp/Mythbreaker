using UnityEngine;

namespace FinalScripts
{
    [RequireComponent(typeof(AudioSource))]
    public class RandomAudioPlayer : MonoBehaviour
    {
        public bool randomizePitch = true;
        public float pitchRandomRange = 0.2f;
        public float playDelay;
        public AudioClip[] defaultBank;
        
        [HideInInspector] public bool playing;
        [HideInInspector] public bool canPlay;

        protected AudioSource Audiosource;
        
        public AudioSource AudioSource => Audiosource;

        public AudioClip Clip { get; private set; }

        private void Awake()
        {
            Audiosource = GetComponent<AudioSource>();
        }
        
        public AudioClip PlayGetRandomClip()
        {
            return InternalPlayRandomClip();
        }
        
        public void PlayRandomClip()
        {
            Clip = InternalPlayRandomClip();
        }

        AudioClip InternalPlayRandomClip()
        {
            var clip = defaultBank[Random.Range(0, defaultBank.Length)];
            if (clip == null) return null;

            Audiosource.pitch = randomizePitch ? Random.Range(1.0f - pitchRandomRange, 1.0f + pitchRandomRange) : 1.0f;
            Audiosource.clip = clip;
            Audiosource.PlayDelayed(playDelay);

            return clip;
        }
    }
}