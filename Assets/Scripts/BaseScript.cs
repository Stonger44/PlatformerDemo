using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScript : MonoBehaviour
{
    protected void PlayAudioClip(AudioSource audioSource, AudioClip audioClip, float volume = 1f)
    {
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    protected IEnumerator StopAudioSource_Routine(AudioSource audioSource, float waitForSeconds)
    {
        yield return new WaitForSeconds(waitForSeconds);

        audioSource.Stop();
    }
}
