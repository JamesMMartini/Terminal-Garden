using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLamp : File
{
    [SerializeField] bool pinkLamp;
    [SerializeField] Light lampLight;

    bool on;

    // Start is called before the first frame update
    void Start()
    {
        if (FileName == "")
            FileName = "togglelamp";

        if (FileType == "")
            FileType = ".exe";
    }

    public override string Open()
    {
        if (on)
        {
            on = false;
            lampLight.enabled = false;
            return "LAMP DISABLED";
        }
        else
        {
            on = true;
            lampLight.enabled = true;
            return "LAMP ENABLED";
        }
    }
}
