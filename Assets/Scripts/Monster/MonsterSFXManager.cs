using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSFXManager : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Sound Clips")]
    public AudioClip idleClip;
    public AudioClip walkClip;
    public AudioClip runClip;
    public AudioClip attackClip;

    public void PlayIdleSound()
    {
        PlaySound(idleClip, true);
    }

    public void PlayWalkSound()
    {
        PlaySound(walkClip, true);
    }

    public void PlayRunSound()
    {
        PlaySound(runClip, true);
    }

    public void PlayAttackSound()
    {
        PlaySound(attackClip, false); // ´Ü¹ß
    }

    private void PlaySound(AudioClip clip, bool loop)
    {
        if (audioSource == null || clip == null) return;

        if (audioSource.clip == clip && audioSource.isPlaying) return;

        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }

    public void StopSound()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }
}