using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogmanQuest : File
{
    [SerializeField] Conversation success;
    [SerializeField] Conversation failure;
    [SerializeField] Conversation notAccepted;
    [SerializeField] Sprite happySprite;
    [SerializeField] Sprite scaredSprite;

    GameObject given;

    void Start()
    {
        FileName = "give";
        FileType = ".exe";

        given = null;
    }

    public override string Open()
    {
        if (given == null)
        {
            GameManager.Instance.seekingParameter = this;

            return "PLEASE PASS OBJECT TO GIVE";
        }
        else
        {
            return "OBJECT ALREADY GIVEN";
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
            if (parameterObject.name == "flower")
            {
                Attributes atts = parameterObject.GetComponent<Attributes>();
                bool healthy = false;
                foreach (string att in atts.GetAttributes())
                    if (att == "healthy")
                        healthy = true;

                if (healthy)
                {
                    GameManager.Instance.DialogueManager.StartConversation(success.Lines);
                    GetComponent<SpriteRenderer>().sprite = happySprite;
                    GameManager.Instance.seekingParameter = null;

                    gameObject.GetComponent<Talk>().dialogueLines = success;
                }
                else
                {
                    GameManager.Instance.DialogueManager.StartConversation(failure.Lines);
                    GetComponent<SpriteRenderer>().sprite = scaredSprite;
                    GameManager.Instance.seekingParameter = null;

                    gameObject.GetComponent<Talk>().dialogueLines = failure;
                }

                given = parameterObject;
                GameManager.Instance.player.Inventory.Remove(parameterObject);
                return "OBJECT ACCEPTED";
            }
            else
            {
                GameManager.Instance.DialogueManager.StartConversation(notAccepted.Lines);
                GameManager.Instance.seekingParameter = null;

                return "OBJECT NOT ACCEPTED";
            }
        }
        else
        {
            return "OBJECT NOT FOUND IN PLAYER INVENTORY";
        }
    }
}
