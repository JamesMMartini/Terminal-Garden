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

        DontDestroyOnLoad(this);
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
        questToggle = 3,
        inventoryAdded = 4,
        inventoryRemoved = 5,
        questComplete = 6,
        endSound = 7
    }
}
