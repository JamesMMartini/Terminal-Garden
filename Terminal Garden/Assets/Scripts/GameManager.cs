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

    [Header("Terminal")]
    [SerializeField] TerminalManager terminal;

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

    public string ExecuteCommand()
    {
        terminalInput.Trim();
        string returnString = terminalInput + " - ";

        if (terminalInput == "walk")
        {
            player.walking = true;
            terminal.DeselectObject();
            returnString += "NOW WALKING";
        }
        else if (terminalInput == "stop")
        {
            player.walking = false;
            terminal.DeselectObject();
            returnString += "STOPPING WALK";
        }
        else if (terminalInput == "rotate l")
        {
            float rotation = -15f;
            player.rotation = rotation;
            terminal.DeselectObject();
            returnString += "ROTATING " + rotation + " DEGREES";
        }
        else if (terminalInput == "rotate r")
        {
            float rotation = 15f;
            player.rotation = rotation;
            terminal.DeselectObject();
            returnString += "ROTATING " + rotation + " DEGREES";
        }
        else if (terminalInput == "inspect")
        {
            
        }
        else if (terminalInput == "help")
        {

        }
        else
        {
            returnString += "UNKNOWN COMMAND";
        }

        terminalInput = "";
        return returnString;
    }
}
