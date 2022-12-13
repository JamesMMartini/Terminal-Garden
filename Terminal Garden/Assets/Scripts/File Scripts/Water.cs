using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : File
{
    bool watered;

    public bool WateredSuccessfully;

    [SerializeField] string QuestName;

    void Start()
    {
        watered = false;
        WateredSuccessfully = false;

        FileName = "water";
        FileType = ".exe";

        GameManager.Instance.AddQuest(QuestName);
    }

    public override string Open()
    {
        if (!watered)
        {
            GameManager.Instance.SetSeekingParameter(this);

            //GameManager.Instance.seekingParameter = this;

            return "PLEASE PASS WATER OBJECT";
        }
        else
        {
            return "OBJECT ALREADY WATERED";
        }
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
                            collect.collectible.GetComponent<Attributes>().AddAttribute("healthy");
                        }
                        else
                        {
                            WateredSuccessfully = false;
                            GameManager.Instance.PinkIndex++;
                            collect.collectible.name = "corrupted flower";
                            collect.collectible.GetComponent<Attributes>().AddAttribute("corrupted");
                        }

                        GameManager.Instance.player.RemoveObject(parameterObject);
                        //GameManager.Instance.player.Inventory.Remove(parameterObject);
                        GetComponent<Collect>().enabled = true;
                        watered = true;
                        GameManager.Instance.seekingParameter = null;
                        GameManager.Instance.RemoveQuest(QuestName);
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
