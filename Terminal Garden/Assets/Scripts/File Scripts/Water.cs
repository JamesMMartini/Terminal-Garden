using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : File
{
    bool watered;

    public bool WateredSuccessfully;

    void Start()
    {
        watered = false;
        WateredSuccessfully = false;

        FileName = "water";
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
                        Collect collect = GetComponent<Collect>();

                        if (parameterObject.name == "water")
                        {
                            WateredSuccessfully = true;
                            collect.GetComponent<Attributes>().AddAttribute("healthy");
                        }
                        else
                        {
                            WateredSuccessfully = false;
                            collect.GetComponent<Attributes>().AddAttribute("corrupted");
                        }

                        GameManager.Instance.player.Inventory.Remove(parameterObject);
                        GetComponent<Collect>().enabled = true;
                        watered = true;
                        GameManager.Instance.seekingParameter = null;
                        return "SUCCESSFULY WATERED TARGET. CLOSING PROGRAM";
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
            return "OBJECT ALREADY WATERED";
        }
    }
}
