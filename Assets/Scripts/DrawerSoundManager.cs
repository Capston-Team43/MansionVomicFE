using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerSoundManager : MonoBehaviour
{
    public static DrawerSoundManager Instance;

    public AudioClip openCloseClip;
    public AudioClip hidingClip;
    public AudioClip pickupClip;
    public AudioClip dropClip; // Ãß°¡

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    public void PlayDrawerSound() => PlaySound(openCloseClip);
    public void PlayHidingSound() => PlaySound(hidingClip);
    public void PlayPickupSound() => PlaySound(pickupClip);
    public void PlayDropSound() => PlaySound(dropClip);

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}

