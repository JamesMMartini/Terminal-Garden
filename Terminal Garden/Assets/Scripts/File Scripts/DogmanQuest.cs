using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogmanQuest : File
{
    [SerializeField] Conversation success;
    [SerializeField] Conversation failure;
    [SerializeField] Conversation beer;
    [SerializeField] Conversation notAccepted;
    [SerializeField] Sprite happySprite;
    [SerializeField] Sprite scaredSprite;
    [SerializeField] string QuestName;

    GameObject given;

    void Start()
    {
        FileName = "give";
        FileType = ".exe";

        GameManager.Instance.AddQuest(QuestName);

        given = null;
    }

    public override string Open()
    {
        if (given == null)
        {
            GameManager.Instance.SetSeekingParameter(this);
            
            //GameManager.Instance.seekingParameter = this;

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
            Attributes atts = parameterObject.GetComponent<Attributes>();
            bool healthy = false;
            bool alcohol = false;
            foreach (string att in atts.GetAttributes())
            {
                if (att == "healthy")
                    healthy = true;
                else if (att == "alcohol")
                    alcohol = true;
            }

            if (parameterObject.name.Contains("flower"))
            {
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

                    GameManager.Instance.PinkIndex++;
                }

                given = parameterObject;
                GameManager.Instance.player.RemoveObject(parameterObject);
                //GameManager.Instance.player.Inventory.Remove(parameterObject);
                GameManager.Instance.RemoveQuest(QuestName);
                return "OBJECT ACCEPTED";
            }
            else if (parameterObject.name == "drink" && alcohol)
            {
                GameManager.Instance.DialogueManager.StartConversation(beer.Lines);
                GetComponent<SpriteRenderer>().sprite = happySprite;
                GameManager.Instance.seekingParameter = null;

                gameObject.GetComponent<Talk>().dialogueLines = beer;

                given = parameterObject;
                GameManager.Instance.player.RemoveObject(parameterObject);
                //GameManager.Instance.player.Inventory.Remove(parameterObject);
                GameManager.Instance.RemoveQuest(QuestName);
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
