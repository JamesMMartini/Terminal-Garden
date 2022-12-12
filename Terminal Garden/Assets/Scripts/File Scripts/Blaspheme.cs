using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaspheme : File
{
    [SerializeField] Sprite bloodyStone;
    [SerializeField] string QuestName;

    void Start()
    {
        FileName = "blaspheme";
        FileType = ".exe";
    }

    public override string Open()
    {
        GameManager.Instance.SetSeekingParameter(this);

        //GameManager.Instance.seekingParameter = this;

        return "PLEASE PASS OBJECT TO BLASPHEME WITH";
    }

    public override string Execute(string parameter)
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

                bool hasBlood = false;
                foreach (string a in attsString)
                    if (a == "blood")
                        hasBlood = true;

                if (hasBlood)
                {
                    GameManager.Instance.player.RemoveObject(parameterObject);
                    //GameManager.Instance.player.Inventory.Remove(parameterObject);
                    GameManager.Instance.seekingParameter = null;
                    GameManager.Instance.RemoveQuest(QuestName);

                    gameObject.GetComponent<SpriteRenderer>().sprite = bloodyStone;
                    gameObject.GetComponent<Pray>().enabled = false;
                    gameObject.GetComponent<Blaspheme>().enabled = false;
                    return "SUCCESSFULY BLASPHEMED TARGET. CLOSING PROGRAM";
                }
                else
                {
                    return "BLOOD NOT FOUND ON OBJECT. PLEASE ENTER VALID BLOOD OBJECT";
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
}
