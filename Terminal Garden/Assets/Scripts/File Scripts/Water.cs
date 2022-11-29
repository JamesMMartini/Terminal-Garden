using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : File
{
    bool watered;

    void Start()
    {
        watered = false;

        FileName = "Water";
        FileType = ".exe";
    }

    public override string Open()
    {
        GameManager.Instance.seekingParameter = this;

        return "PLEASE PASS WATER OBJECT";
    }

    public override string Execute(string parameter)
    {
        if (!watered)
        {
            GameObject parameterObejct = GameObject.Find(parameter);

            if (parameterObejct != null)
            {
                Attributes atts = parameterObejct.GetComponent<Attributes>();
                if (atts != null)
                {
                    string[] attsString = atts.GetAttributes();

                    bool hasWater = false;
                    foreach (string a in attsString)
                        if (a == "Water")
                            hasWater = true;

                    if (hasWater)
                    {
                        watered = true;
                        GameManager.Instance.seekingParameter = null;
                        return "SUCCESSFULY WATERED TARGET. CLOSING PROGRAM";
                    }
                    else
                    {
                        return "WATER NOT FOUND ON OBJECT. PLEASE ENTER VALID WATER OBJECT";
                    }
                }
                else
                {
                    return "NO ATTRIBUTES FOUND ON OBJECT. PLEASE ENTER VALID OBJECT";
                }
            }
            else
            {
                return "OBJECT NOT FOUND. PLEASE ENTER VALID OBJECT";
            }
        }
        else
        {
            return "OBJECT ALREADY WATERED";
        }
    }
}
