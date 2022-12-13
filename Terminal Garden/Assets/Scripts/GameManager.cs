using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public DialogueManager DialogueManager;

    public bool RunUpdate;
    public string terminalInput = "";

    public PlayerController player;
    public TextFile_SO helpText;
    public TMP_Text fileList;

    [SerializeField] float turnDegrees;

    [Header("Time")]
    [SerializeField] public float timeStep;
    float timeCount;

    [Header("Terminal")]
    [SerializeField] public TerminalManager terminal;
    [SerializeField] Conversation openingConversation;

    // Parameter Stuff
    public File seekingParameter;

    [SerializeField] TMP_Text questListTMP;
    public List<string> questList;

    [SerializeField] int pinkIndexMax;
    public int PinkIndex;

    public string currentScene;

    public GameObject blackPanel;
    private float fadeInT = 0f;
    private bool fadeIn = false;
    private float fadeOutT = 0f;
    private bool fadeOut = false;

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

        //DontDestroyOnLoad(Instance);

        questList = new List<string>();
    }

    // Start is called before the first frame update
    void Start()
    {
        RunUpdate = false;
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        fileList.text = "";

        PinkIndex = 0;

        DialogueManager.StartConversation(openingConversation.Lines);

        if (openingConversation.name == "PinkEnd")
        {
            DialogueManager.isPinkEnd = true;
        }
        else if (openingConversation.name == "RebornEnd")
        {
            DialogueManager.isRebornEnd = true;
        }

        currentScene = SceneManager.GetActiveScene().name;

        StartFadeIn();
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

        if (PinkIndex >= 4 && currentScene == "World One")
        {
            StartCoroutine(GoToEndScene("PinkEnd"));
        }
        else if (questList.Count == 0 && currentScene == "World One")
        {
            StartCoroutine(GoToEndScene("RebornEnd"));
        }

        FadeIn();
        FadeOut();
    }

    public string ExecuteCommand()
    {
        terminalInput.Trim();
        string returnString = terminalInput + " - ";

        if (terminalInput == "w")
        {
            player.walking = true;
            terminal.DeselectObject();
            returnString += "NOW WALKING";
        }
        else if (terminalInput == "s")
        {
            player.walking = false;
            terminal.DeselectObject();
            returnString += "STOPPING WALK";
        }
        else if (terminalInput == "a")
        {
            float rotation = -turnDegrees;
            player.rotation = rotation;
            terminal.DeselectObject();
            returnString += "ROTATING " + rotation + " DEGREES";
        }
        else if (terminalInput == "d")
        {
            float rotation = turnDegrees;
            player.rotation = rotation;
            terminal.DeselectObject();
            returnString += "ROTATING " + rotation + " DEGREES";
        }
        else if (terminalInput == "inventory")
        {
            terminal.ShowInventory();
        }
        else if (terminalInput == "help")
        {
            fileList.text = helpText.text;
            returnString += "LISTING COMMAND REFERENCES";
        }
        else if (terminalInput.StartsWith("cd"))
        {
            string objName = terminalInput.Substring(terminalInput.IndexOf(" ") + 1);

            returnString += "SELECTING " + objName;

            GameObject obj = GameObject.Find(objName);

            if (obj != null)
            {
                terminal.SelectObject(obj, false);

                File[] files = terminal.selectedObject.GetComponents<File>();

                fileList.text = "";
                foreach (File file in files)
                {
                    fileList.text += file.FileName + file.FileType + "\r\n";
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

            List<string> objectList = new List<string>();

            foreach (GameObject interactable in allInteractable)
            {
                Vector3 distToObj = interactable.transform.position - player.transform.position;

                if (distToObj.magnitude < 30f && !objectList.Contains(interactable.name))
                    objectList.Add(interactable.name);
            }

            fileList.text = "INTERACTABLE OBJECTS WITHIN 40M:" + "\r\n";
            foreach (string interactable in objectList)
            {
                fileList.text += "\r\n" + interactable;
            }
        }
        else if (terminalInput == "ls")
        {
            if (terminal.selectedObject != null)
            {
                returnString += "GETTING FILES IN OBJECT";

                File[] files = terminal.selectedObject.GetComponents<File>();

                //List<string> files = terminal.selectedObject.GetComponent<Executables>().GetFiles();

                fileList.text = "";
                foreach (File file in files)
                {
                    fileList.text += file.FileName + file.FileType + "\r\n";
                }
            }
            else
            {
                returnString += "\r\nPLEASE SELECT AN OBJECT TO VIEW ITS FILES";
            }
        }
        else
        {
            if (terminal.selectedObject != null)
            {
                try
                {
                    string file = terminalInput;
                    string filename = file.Substring(0, file.IndexOf("."));
                    File objFile = null;

                    File[] filesInObj = terminal.selectedObject.GetComponents<File>();
                    foreach (File f in filesInObj)
                    {
                        if (f.FileName == filename)
                            objFile = f;
                    }

                    if (objFile != null && objFile.enabled)
                    {
                        returnString += "\r\nOPENING " + file;
                        returnString += "\r\n" + objFile.Open();
                    }
                    else
                    {
                        returnString += "\r\nUNABLE TO FIND FILE " + file + " IN SELECTED OBJECT";
                    }
                }
                catch (Exception ex)
                {
                    returnString += "\r\n UNABLE TO RUN COMMAND. ENSURE FILE NAME IS CORRECT";
                }
            }
            else
            {
                returnString += "UNKNOWN COMMAND";
            }
        }

        terminalInput = "";
        return returnString;
    }

    public string EnterParameter()
    {
        terminalInput.Trim();

        if (terminalInput != "exit")
        {
            string returnString = seekingParameter.Execute(terminalInput);
            terminalInput = "";
            return returnString;
        }
        else
        {
            seekingParameter = null;
            terminalInput = "";
            return "CLOSING PROGRAM";
        }
    }

    public void AddQuest(string quest)
    {
        questList.Add(quest);

        if (questList.Count > 0)
        {
            questListTMP.text = questList[0];

            for (int i = 1; i < questList.Count; i++)
                questListTMP.text += "\r\n" + questList[i];
        }
        else
        {
            questListTMP.text = "";
        }
    }

    public void RemoveQuest(string quest)
    {
        questList.Remove(quest);
        SoundManager.Instance.PlayClip(SoundManager.AudioClips.questComplete);

        if (questList.Count > 0)
        {
            questListTMP.text = questList[0];

            for (int i = 1; i < questList.Count; i++)
                questListTMP.text += "\r\n" + questList[i];
        }
        else
        {
            questListTMP.text = "";
        }
    }

    public void SetSeekingParameter(File file)
    {
        seekingParameter = file;

        terminal.ShowInventory();
    }


    public void StartFadeIn()
    {
        if (blackPanel.GetComponent<Image>().color.a == 1)
        {
            fadeInT = 0f;

            fadeIn = true;
        }
    }



    public void StartFadeOut()
    {
        if (blackPanel.GetComponent<Image>().color.a == 0)
        {
            fadeOutT = 0f;

            fadeOut = true;
        }
    }

    public void FadeIn()
    {
        if (fadeIn)
        {
            if (fadeInT < 1f)
            {
                fadeInT += 0.2f * Time.deltaTime;
                blackPanel.GetComponent<Image>().color = Color.Lerp(new Color(blackPanel.GetComponent<Image>().color.r, blackPanel.GetComponent<Image>().color.g, blackPanel.GetComponent<Image>().color.b, 1f), new Color(blackPanel.GetComponent<Image>().color.r, blackPanel.GetComponent<Image>().color.g, blackPanel.GetComponent<Image>().color.b, 0f), fadeInT);

            }
            else
            {
                fadeIn = false;
            }
        }
    }

    public void FadeOut()
    {
        if (fadeOut)
        {
            if (fadeOutT < 1f)
            {
                fadeOutT += 0.2f * Time.deltaTime;
                blackPanel.GetComponent<Image>().color = Color.Lerp(new Color(blackPanel.GetComponent<Image>().color.r, blackPanel.GetComponent<Image>().color.g, blackPanel.GetComponent<Image>().color.b, 0f), new Color(blackPanel.GetComponent<Image>().color.r, blackPanel.GetComponent<Image>().color.g, blackPanel.GetComponent<Image>().color.b, 1f), fadeOutT);

            }
            else
            {
                fadeOut = false;

            }
        }
    }

    IEnumerator GoToEndScene(string sceneName)
    {
        StartFadeOut();
        yield return new WaitForSeconds(8f);
        SceneManager.LoadScene(sceneName);
        
    }

}
