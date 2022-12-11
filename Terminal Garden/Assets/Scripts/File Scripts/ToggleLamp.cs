using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLamp : File
{
    [SerializeField] bool pinkLamp;
    [SerializeField] Light lampLight;
    [SerializeField] Color pinkLampColor;
    [SerializeField] LampManager lampMan;

    bool on;

    // Start is called before the first frame update
    void Start()
    {
        if (pinkLamp)
        {
            lampLight.color = pinkLampColor;
        }

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
            lampMan.LampOff(pinkLamp);
            return "LAMP DISABLED";
        }
        else
        {
            on = true;
            lampLight.enabled = true;
            lampMan.LampOn(pinkLamp);

            gameObject.GetComponent<ToggleLamp>().enabled = false;

            return "LAMP ENABLED";
        }
    }
}
