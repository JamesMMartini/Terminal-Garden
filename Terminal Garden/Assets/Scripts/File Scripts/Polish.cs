using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polish : File
{
    bool cleaned;

    public bool CleanedSuccessfully;

    void Start()
    {
        cleaned = false;
        CleanedSuccessfully = false;

        FileName = "polish";
        FileType = ".exe";
    }

    public override string Open()
    {
        GameManager.Instance.seekingParameter = this;

        return "PLEASE PASS CLEANING OBJECT";
    }

    public override string Execute(string parameter)
    {
        if (!cleaned)
        {
            GameObject parameterObject = null;

            foreach (GameObject ob in GameManager.Instance.player.Inventory)
            {
                if (ob.name == parameter)
                    parameterObject = ob;
            }

            if (parameterObject != null)
            {
                Attributes atts = parameterObject.GetComponent<Attributes>();
                if (atts != null)
                {
                    string[] attsString = atts.GetAttributes();

                    bool hasLiquid = false;
                    foreach (string a in attsString)
                        if (a == "liquid")
                            hasLiquid = true;

                    if (hasLiquid)
                    {
                        if (parameterObject.name == "water" || parameterObject.name == "cleanser")
                        {
                            CleanedSuccessfully = true;
                        }
                        else
                        {
                            CleanedSuccessfully = false;
                        }

                        GameManager.Instance.player.Inventory.Remove(parameterObject);
                        cleaned = true;
                        GameManager.Instance.seekingParameter = null;
                        return "SUCCESSFULY POLISHED TARGET. CLOSING PROGRAM";
                    }
                    else
                    {
                        return "LIQUID NOT FOUND ON OBJECT. PLEASE ENTER VALID LIQUID OBJECT";
                    }
                }
                else
                {
                    return "NO ATTRIBUTES FOUND ON OBJECT. PLEASE ENTER VALID OBJECT";
                }
            }
            else
            {
                return "OBJECT NOT FOUND IN PLAYER INVENTORY";
            }
        }
        else
        {
            return "OBJECT ALREADY CLEANED";
        }
    }
}
