using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandelionQuest : File
{
    [SerializeField] Conversation success;
    [SerializeField] Conversation failure;
    [SerializeField] Conversation notAccepted;
    [Space(10)]
    [SerializeField] GameObject goodResult;
    [SerializeField] GameObject badResult;

    GameObject[] given;
    [SerializeField] string QuestName;

    void Start()
    {
        if (FileName == "")
        {
            FileName = "give";
        }

        if (FileType == "")
        {
            FileType = ".exe";
        }

        GameManager.Instance.AddQuest(QuestName);

        given = new GameObject[2];
    }

    public override string Open()
    {
        if (given[1] == null)
        {
            GameManager.Instance.seekingParameter = this;

            return "PLEASE PASS OBJECT TO GIVE (2 REMAINING)";
        }
        else
        {
            return "OBJECTS ALREADY GIVEN";
        }
    }

    public override string Execute(string parameter)
    {
        GameObject parameterObject = null;

        foreach (GameObject ob in GameManager.Instance.player.Inventory)
        {
            if (ob.name == parameter)
                parameterObject = ob;
        }

        if (parameterObject != null)
        {
            Attributes atts = parameterObject.GetComponent<Attributes>();
            
            if (atts != null)
            {
                bool ingredient = false;
                foreach (string att in atts.GetAttributes())
                    if (att == "ingredient")
                        ingredient = true;

                if (ingredient)
                {
                    GameManager.Instance.player.Inventory.Remove(parameterObject);

                    if (given[0] == null)
                    {
                        given[0] = parameterObject;

                        GameManager.Instance.seekingParameter = this;

                        return "OBJECT ACCEPTED\r\nPLEASE PASS OBJECT TO GIVE (1 REMAINING)";
                    }
                    else
                    {
                        given[1] = parameterObject;

                        bool corrupted = false;

                        foreach (GameObject ingre in given)
                        {
                            Attributes ingAtts = ingre.GetComponent<Attributes>();
                            foreach (string att in ingAtts.GetAttributes())
                                if (att == "corrupted")
                                    corrupted = true;
                        }

                        if (!corrupted)
                        {
                            GameManager.Instance.DialogueManager.StartConversation(success.Lines);
                            GameManager.Instance.seekingParameter = null;

                            gameObject.GetComponent<Talk>().dialogueLines = success;

                            goodResult.SetActive(true);
                        }
                        else
                        {
                            GameManager.Instance.DialogueManager.StartConversation(failure.Lines);
                            GameManager.Instance.seekingParameter = null;

                            gameObject.GetComponent<Talk>().dialogueLines = failure;

                            GameManager.Instance.PinkIndex++;

                            badResult.SetActive(true);
                        }

                        GameManager.Instance.RemoveQuest(QuestName);

                        return "OBJECT ACCEPTED";
                    }
                }
                else
                {
                    GameManager.Instance.DialogueManager.StartConversation(notAccepted.Lines);
                    GameManager.Instance.seekingParameter = null;

                    return "INVALID INGREDIENT OBJECT";
                }
            }
            else
            {
                GameManager.Instance.DialogueManager.StartConversation(notAccepted.Lines);
                GameManager.Instance.seekingParameter = null;

                return "ATTRIBUTES NOT FOUND";
            }
        }
        else
        {
            return "OBJECT NOT FOUND IN PLAYER INVENTORY";
        }
    }
}
