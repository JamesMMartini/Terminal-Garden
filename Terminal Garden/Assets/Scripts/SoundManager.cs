using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;
    [SerializeField] AudioSource source;

    public static SoundManager Instance;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayClip(AudioClips clip)
    {
        source.PlayOneShot(clips[(int)clip]);
    }

    public enum AudioClips
    {
        typeClick = 0,
        dialogueAdvance = 1,
        mouseClick = 2,
        questToggle = 3
    }
}
