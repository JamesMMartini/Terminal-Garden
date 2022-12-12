using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pray : File
{
    [SerializeField] string QuestName;

    // Start is called before the first frame update
    void Start()
    {
        FileName = "pray";
        FileType = ".exe";
    }

    public override string Open()
    {
        GameManager.Instance.PinkIndex++;
        GameManager.Instance.RemoveQuest(QuestName);

        return "GLOBAL PINK INDEX INCREASED";
    }
}
