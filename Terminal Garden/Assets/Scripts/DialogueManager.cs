using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    string[] lines;
    int activeLine = 0;

    [SerializeField] TMP_Text dialogueText;
    [SerializeField] TMP_Text characterName;

    public GameObject endPanel;

    public void StartConversation(string[] newLines)
    {
        gameObject.SetActive(true);

        //foreach (string line in newLines)
        //    Debug.Log(line);

        lines = newLines;
        activeLine = 0;

        string[] lineInfo = lines[activeLine].Split(":");
        characterName.text = "TALK.exe > " + lineInfo[0];
        dialogueText.text = lineInfo[1];
    }

    public void AdvanceDialogue()
    {
        activeLine++;

        if (activeLine < lines.Length)
        {
            string[] lineInfo = lines[activeLine].Split(":");
            characterName.text = "TALK.exe > " + lineInfo[0];
            dialogueText.text = lineInfo[1];
        }
        else
        {
            EndConversation();
        }
    }

    public void EndConversation()
    {
        lines = null;
        activeLine = 0;
        gameObject.SetActive(false);
    }
}
