using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    public static UISoundManager Instance;

    public AudioClip clickSound;
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    public void PlayClickSound()
    {
        if (clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }
}
