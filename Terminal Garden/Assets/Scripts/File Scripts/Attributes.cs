using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attributes : File
{
    [SerializeField] string[] attributes;

    void Start()
    {
        FileName = "Attributes";
        FileType = ".txt";
    }

    public string[] GetAttributes()
    {
        return attributes;
    }

    public override string Open()
    {
        string fileListWrite = "ATTRIBUTES:";
        foreach (string att in attributes)
            fileListWrite += "\r\n" + att;

        GameManager.Instance.WriteToFileList(fileListWrite);

        return "WRITING ATTRIBUTES TO FILE LIST";
    }
}
