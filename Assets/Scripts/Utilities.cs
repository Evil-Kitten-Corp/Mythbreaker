using System.Collections;
using UnityEngine;

namespace FinalScripts
{
    public static class Utilities
    {
        public static IEnumerator FadeOut (this AudioSource audioSource, float fadeTime) 
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0) {
                audioSource.volume -= startVolume * Time.deltaTime / fadeTime;

                yield return null;
            }

            audioSource.Stop ();
            audioSource.volume = startVolume;
        }
    }
}