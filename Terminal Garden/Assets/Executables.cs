using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Executables : MonoBehaviour
{
    [SerializeField] bool HasDelete;

    public List<string> GetFiles()
    {
        List<string> files = new List<string>();

        if (HasDelete)
            files.Add("delete.exe");

        return files;
    }

    public void Delete()
    {
        Destroy(gameObject);
    }

    public string OpenFile(string filename)
    {
        if (filename == "delete.exe")
        {
            Destroy(gameObject);
            return "DELETED " + gameObject.name;
        }
        else
        {
            return "COULD NOT FIND FILE " + filename;
        }
    }
}
