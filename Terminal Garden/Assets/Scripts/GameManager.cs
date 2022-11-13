using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public bool RunUpdate;
    public string terminalInput = "";

    public PlayerController player;

    [Header("Time")]
    [SerializeField] float timeStep;
    float timeCount;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(Instance);
    }

    // Start is called before the first frame update
    void Start()
    {
        RunUpdate = false;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeCount >= timeStep)
        {
            RunUpdate = true;
            timeCount = 0f;
        }
        else
        {
            RunUpdate = false;
            timeCount += Time.deltaTime;
        }
    }

    public void ExecuteCommand()
    {
        terminalInput.Trim();

        if (terminalInput == "walk")
        {
            player.walking = true;
        }
        else if (terminalInput == "stop")
        {
            player.walking = false;
        }
        else if (terminalInput.StartsWith("rotate"))
        {
            string num = terminalInput.Substring(terminalInput.IndexOf(" ") + 1);
            float rotation = Single.Parse(num);
            player.rotation = rotation;
        }

        terminalInput = "";
    }
}
