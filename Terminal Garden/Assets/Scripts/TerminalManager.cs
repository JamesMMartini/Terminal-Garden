using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class TerminalManager : MonoBehaviour
{
    [SerializeField] Camera cam;

    [Header("UI Objects")]
    [SerializeField] TMP_Text inputField;
    [SerializeField] TMP_Text terminalLog;
    [SerializeField] TMP_Text folderName;
    [SerializeField] TMP_Text fileList;
    [SerializeField] GameObject selectedIndicator;

    public GameObject selectedObject;

    // Start is called before the first frame update
    void Start()
    {
        inputField.text = "";
        terminalLog.text = "TYPE help FOR COMMAND REFERENCES";
        Keyboard.current.onTextInput += OnTextInput;
    }

    public void AdvanceDialogue(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (GameManager.Instance.DialogueManager.gameObject.activeInHierarchy)
            {
                GameManager.Instance.DialogueManager.AdvanceDialogue();
            }
        }
    }

    private void OnTextInput(char ch)
    {
        if (!char.IsControl(ch) && !GameManager.Instance.DialogueManager.gameObject.activeInHierarchy)
        {
            GameManager.Instance.terminalInput += ch;
            inputField.text = GameManager.Instance.terminalInput;
        }
    }

    public void Return(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (GameManager.Instance.seekingParameter == null)
            {
                terminalLog.text += "\r\n" + GameManager.Instance.ExecuteCommand();
                inputField.text = "";
            }
            else
            {
                terminalLog.text += "\r\n" + GameManager.Instance.EnterParameter();
                inputField.text = "";
            }
        }
    }

    public void Click(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out hit, 40f))
            {
                Transform objectHit = hit.transform;
                if (objectHit.tag == "Interactable")
                {
                    SelectObject(objectHit.gameObject, true);
                }
            }
        }
    }

    public void SelectObject(GameObject objSelected, bool clicked)
    {
        // Deselect the current object if necessary
        if (selectedObject != null)
            DeselectObject();
        
        selectedObject = objSelected;

        Vector3 indicatorPos = Mouse.current.position.ReadValue();
        indicatorPos.y -= 30f;

        if (clicked)
        {
            selectedIndicator.SetActive(true);
            selectedIndicator.transform.position = indicatorPos;
            selectedIndicator.GetComponentInChildren<TMP_Text>().text = selectedObject.name;
        }

        terminalLog.text += "\r\nSELECTED " + selectedObject.name;
        inputField.text = "";
        folderName.text += selectedObject.name + ">";
        fileList.text = "";
    }

    public void DeselectObject()
    {
        if (selectedObject != null)
        {
            terminalLog.text += "\r\nDESELECTED " + selectedObject.name;
        }
        selectedObject = null;

        folderName.text = "WORLD>";
        fileList.text = "";

        selectedIndicator.SetActive(false);
    }

    public void ShowInventory()
    {
        DeselectObject();

        folderName.text = "WORLD>PLAYER INVENTORY";

        string inventoryList = "";
        foreach (GameObject gameObj in GameManager.Instance.player.Inventory)
            inventoryList += "\r\n" + gameObj.name;

        fileList.text = inventoryList;
    }
}
