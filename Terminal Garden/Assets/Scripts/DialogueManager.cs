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
    public bool isPinkEnd;
    public bool isRebornEnd;

    public GameManager gameManager;

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
        SoundManager.Instance.PlayClip(SoundManager.AudioClips.dialogueAdvance);

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

        if (isPinkEnd || isRebornEnd)
        {
            endPanel.SetActive(true);
            SoundManager.Instance.PlayClip(SoundManager.AudioClips.endSound);

        }

        gameObject.SetActive(false);
    }
}
