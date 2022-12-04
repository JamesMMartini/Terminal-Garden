using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attributes : File
{
    [SerializeField] string[] attributes;

    void Start()
    {
        FileName = "attributes";
        FileType = ".txt";
    }

    public string[] GetAttributes()
    {
        return attributes;
    }

    public void AddAttribute(string att)
    {
        string[] atts = new string[attributes.Length + 1];
        for (int i = 0; i < attributes.Length; i++)
            atts[i] = attributes[i];

        atts[atts.Length - 1] = att;

        attributes = atts;
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
