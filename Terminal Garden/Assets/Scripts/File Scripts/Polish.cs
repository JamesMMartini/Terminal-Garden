using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polish : File
{
    bool cleaned;

    public bool CleanedSuccessfully;

    [SerializeField] string QuestName;

    void Start()
    {
        cleaned = false;
        CleanedSuccessfully = false;

        GameManager.Instance.AddQuest(QuestName);

        FileName = "polish";
        FileType = ".exe";
    }

    public override string Open()
    {
        if (!cleaned)
        {
            GameManager.Instance.SetSeekingParameter(this);

            //GameManager.Instance.seekingParameter = this;

            return "PLEASE PASS CLEANING OBJECT";
        }
        else
        {
            return "OBJECT ALREADY CLEANED";
        }
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

                            GameManager.Instance.PinkIndex++;
                        }

                        GameManager.Instance.player.RemoveObject(parameterObject);
                        //GameManager.Instance.player.Inventory.Remove(parameterObject);
                        cleaned = true;
                        GameManager.Instance.seekingParameter = null;
                        GameManager.Instance.RemoveQuest(QuestName);
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
