using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chop : File
{
    [SerializeField] Sprite choppedSprite;

    // Start is called before the first frame update
    void Start()
    {
        if (FileName == "")
        {
            FileName = "chop";
        }

        if (FileType == "")
        {
            FileType = ".exe";
        }
    }

    public override string Open()
    {
        bool hasAxe = false;
        foreach (GameObject inv in GameManager.Instance.player.Inventory)
        {
            if (inv.name == "axe")
                hasAxe = true;
        }

        if (hasAxe)
        {
            // Update the sprite
            GetComponent<SpriteRenderer>().sprite = choppedSprite;

            // Enable the collectible
            GetComponent<Collect>().enabled = true;

            return "TREE CHOPPED, WOOD AVAILABLE TO COLLECT";
        }
        else
        {
            return "AXE NOT FOUND IN PLAYER INVENTORY";
        }
    }
}
