using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair : File
{
    public bool repaired;
    public bool RepairedSuccessfully;

    [SerializeField] Sprite repairedSprite;
    [SerializeField] Sprite brokenSprite;
    [SerializeField] Sprite corruptedSprite;
    [SerializeField] string QuestName;

    void Start()
    {
        if (!repaired)
        {
            GetComponent<SpriteRenderer>().sprite = brokenSprite;
            GetComponent<Collect>().enabled = true;
            GameManager.Instance.AddQuest(QuestName);
        }

        FileName = "repair";
        FileType = ".exe";
    }

    public override string Open()
    {
        if (!repaired)
        {
            GameManager.Instance.SetSeekingParameter(this);

            //GameManager.Instance.seekingParameter = this;

            return "PLEASE PASS MATERIALS";
        }
        else
        {
            return "OBJECT ALREADY REPAIRED";
        }
    }

    public override string Execute(string parameter)
    {
        if (!repaired)
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

                    bool hasWood = false;
                    bool broken = false;
                    foreach (string a in attsString)
                    {
                        if (a == "wood")
                            hasWood = true;
                        else if (a == "broken")
                            broken = true;
                    }

                    if (hasWood)
                    {
                        if (!broken)
                        {
                            RepairedSuccessfully = true;
                            GetComponent<SpriteRenderer>().sprite = repairedSprite;
                        }
                        else
                        {
                            RepairedSuccessfully = false;
                            GetComponent<SpriteRenderer>().sprite = corruptedSprite;
                            GameManager.Instance.PinkIndex++;
                        }

                        GameManager.Instance.player.RemoveObject(parameterObject);
                        //GameManager.Instance.player.Inventory.Remove(parameterObject);
                        GetComponent<Collect>().enabled = false;
                        repaired = true;
                        GameManager.Instance.seekingParameter = null;
                        GameManager.Instance.RemoveQuest(QuestName);
                        return "SUCCESSFULY REPAIRED TARGET. CLOSING PROGRAM";
                    }
                    else
                    {
                        return "WOOD NOT FOUND ON OBJECT. PLEASE ENTER VALID WOOD OBJECT";
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
            return "OBJECT ALREADY REPAIRED";
        }
    }
}
