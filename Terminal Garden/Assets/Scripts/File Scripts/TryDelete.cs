using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryDelete : File
{
    // Start is called before the first frame update
    void Start()
    {
        FileName = "TryDelete";
        FileType = ".exe";
    }

    public override string Open()
    {
        Destroy(gameObject);
        return gameObject.name + " DELETED";
    }
}
