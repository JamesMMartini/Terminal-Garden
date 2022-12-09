using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRaccoon : File
{
    public BugManager bugQuest;

    // Start is called before the first frame update
    void Start()
    {
        FileName = "debug";
        FileType = ".exe";
    }

    public override string Open()
    {
        bugQuest.DebugMrRaccoon();

        return "DEBUGGING MR. RACCOON";
    }
}
