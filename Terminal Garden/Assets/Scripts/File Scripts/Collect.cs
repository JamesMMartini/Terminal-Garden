using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : File
{
    [SerializeField] bool singular;
    [SerializeField] GameObject collectible;

    // Start is called before the first frame update
    void Start()
    {
        FileName = "Collect";
        FileType = ".exe";
    }

    public override string Open()
    {
        string returnStr = "";

        if (collectible != null)
        {
            returnStr = collectible.name + " ADDED TO INVENTORY";

            GameManager.Instance.player.CollectObject(collectible);

            GameManager.Instance.terminal.ShowInventory();

            if (singular)
                collectible = null;

            return returnStr;
        }
        else
        {
            return "OBJECT ALREADY COLLECTED";
        }
    }
}
