using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public bool RunUpdate;
    public string terminalInput = "";

    public PlayerController player;
    public TextFile_SO helpText;
    public Text fileList;

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

        fileList.text = "";
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
        else if (terminalInput == "turn l")
        {
            float rotation = -15f;
            player.rotation = rotation;
            terminal.DeselectObject();
            returnString += "ROTATING " + rotation + " DEGREES";
        }
        else if (terminalInput == "turn r")
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
            fileList.text = helpText.text;
            returnString += "LISTING COMMAND REFERENCES";
        }
        else if (terminalInput.StartsWith("select"))
        {
            string objName = terminalInput.Substring(terminalInput.IndexOf(" ") + 1);

            returnString += "SELECTING " + objName;

            GameObject obj = GameObject.Find(objName);

            if (obj != null)
            {
                terminal.SelectObject(obj, false);
                List<string> files = terminal.selectedObject.GetComponent<Executables>().GetFiles();

                fileList.text = "";
                foreach (string file in files)
                {
                    fileList.text += file + "\r\n";
                }
            }
            else
            {
                returnString += "\r\nUNABLE TO FIND " + objName;
            }
        }
        else if (terminalInput.StartsWith("lookat"))
        {
            string objName = terminalInput.Substring(terminalInput.IndexOf(" ") + 1);

            returnString += "LOOKINGAT " + objName;

            GameObject obj = GameObject.Find(objName);

            if (obj != null)
            {
                player.LookAt(obj);
            }
            else
            {
                returnString += "\r\nUNABLE TO FIND " + objName;
            }
        }
        else if (terminalInput == "scan")
        {
            returnString += "RETURNING ALL OBJECTS WITHIN 40M";

            GameObject[] allInteractable = GameObject.FindGameObjectsWithTag("Interactable");

            List<GameObject> objectList = new List<GameObject>();

            foreach (GameObject interactable in allInteractable)
            {
                Vector3 distToObj = interactable.transform.position - player.transform.position;

                if (distToObj.magnitude < 40f)
                    objectList.Add(interactable);
            }

            fileList.text = "INTERACTABLE OBJECTS WITHIN 40M:" + "\r\n";
            foreach (GameObject interactable in objectList)
            {
                fileList.text += "\r\n" + interactable.name;
            }
        }
        else if (terminalInput == "ls")
        {
            if (terminal.selectedObject != null)
            {
                returnString += "GETTING FILES IN OBJECT";

                List<string> files = terminal.selectedObject.GetComponent<Executables>().GetFiles();
                fileList.text = "";
                foreach (string file in files)
                {
                    fileList.text += file + "\r\n";
                }
            }
            else
            {
                returnString += "\r\nPLEASE SELECT AN OBJECT TO VIEW ITS FILES";
            }
        }
        else if (terminalInput.StartsWith("open"))
        {
            if (terminal.selectedObject != null)
            {
                string filename = terminalInput.Substring(terminalInput.IndexOf(" ") + 1);
                returnString += "\r\n" + terminal.selectedObject.GetComponent<Executables>().OpenFile(filename);
            }
            else
            {
                returnString += "\r\nPLEASE SELECT AN OBJECT TO OPEN ITS FILES";
            }
        }
        else
        {
            returnString += "UNKNOWN COMMAND";
        }

        terminalInput = "";
        return returnString;
    }
}
