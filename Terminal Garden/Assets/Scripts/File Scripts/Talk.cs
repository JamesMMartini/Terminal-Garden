using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talk : File
{
    [SerializeField] Conversation dialogueLines;

    // Start is called before the first frame update
    void Start()
    {
        FileName = "Talk";
        FileType = ".exe";
    }

    public override string Open()
    {
        GameManager.Instance.DialogueManager.StartConversation(dialogueLines.Lines);

        return "CONVERSATION OPENED";
    }
}
