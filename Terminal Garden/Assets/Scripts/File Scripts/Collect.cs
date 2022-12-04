using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : File
{
    [SerializeField] bool singular;
    [SerializeField] public GameObject collectible;

    bool collected;

    // Start is called before the first frame update
    void Start()
    {
        FileName = "collect";
        FileType = ".exe";
    }

    public override string Open()
    {
        string returnStr = "";

        if (!collected)
        {
            GameObject returnObject = Instantiate(collectible);

            returnObject.name = collectible.name;

            returnStr = collectible.name + " ADDED TO INVENTORY";
            GameManager.Instance.player.CollectObject(returnObject);
            GameManager.Instance.terminal.ShowInventory();

            if (singular)
            {
                collected = true;
                gameObject.SetActive(false);
            }

            return returnStr;
        }
        else
        {
            return "OBJECT ALREADY COLLECTED";
        }
    }
}
