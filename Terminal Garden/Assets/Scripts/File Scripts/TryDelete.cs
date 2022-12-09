using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryDelete : File
{
    public BugManager bugQuest;
    public bool MrRaccoon;
    public GameObject deletable;

    // Start is called before the first frame update
    void Start()
    {
        FileName = "trydelete";
        FileType = ".exe";
    }

    public override string Open()
    {
        if (!MrRaccoon)
        {
            bugQuest.DeleteBug(gameObject);
            Destroy(deletable);
            return gameObject.name + " DELETED";
        }
        else
        {
            bugQuest.DeleteMrRaccoon();

            return "FAILED TO DELETE\r\nBUG OBJECT CORRUPTED";
        }
    }
}
