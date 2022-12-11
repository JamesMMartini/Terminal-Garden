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
    [SerializeField] GameObject questWindow;

    public GameObject selectedObject;

    // Start is called before the first frame update
    void Start()
    {
        inputField.text = "";
        terminalLog.text = "TYPE help FOR COMMAND REFERENCES";
        Keyboard.current.onTextInput += OnTextInput;
    }

    private void Update()
    {
        if (GameManager.Instance.RunUpdate && !GameManager.Instance.DialogueManager.isActiveAndEnabled)
        {
            // Determine if we need to blink the cursor
            if (inputField.text.EndsWith("_"))
            {
                inputField.text = GameManager.Instance.terminalInput;
            }
            else
            {
                inputField.text = GameManager.Instance.terminalInput + "_";
            }
        }
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

    public void Backspace(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (GameManager.Instance.terminalInput.Length > 0)
            {
                GameManager.Instance.terminalInput = GameManager.Instance.terminalInput.Substring(0, GameManager.Instance.terminalInput.Length - 1);
                inputField.text = GameManager.Instance.terminalInput + "_";
            }
        }
    }

    private void OnTextInput(char ch)
    {
        if (!char.IsControl(ch) && !GameManager.Instance.DialogueManager.gameObject.activeInHierarchy)
        {
            GameManager.Instance.terminalInput += ch;
            SoundManager.Instance.PlayClip(SoundManager.AudioClips.typeClick);
            inputField.text = GameManager.Instance.terminalInput + "_";
        }

        if (GameManager.Instance.terminalInput == " ")
        {
            GameManager.Instance.terminalInput = "";
            inputField.text = GameManager.Instance.terminalInput + "_";
        }
    }

    public void Return(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!GameManager.Instance.DialogueManager.gameObject.activeInHierarchy)
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
    }

    public void Tab(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SoundManager.Instance.PlayClip(SoundManager.AudioClips.questToggle);

            if (questWindow.activeInHierarchy)
                questWindow.SetActive(false);
            else
                questWindow.SetActive(true);
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
                    SoundManager.Instance.PlayClip(SoundManager.AudioClips.mouseClick);

                    SelectObject(objectHit.gameObject, true);

                    File[] files = selectedObject.GetComponents<File>();

                    fileList.text = "";
                    foreach (File file in files)
                    {
                        if (file.enabled)
                            fileList.text += file.FileName + file.FileType + "\r\n";
                    }
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
        //inputField.text = "";
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

    public void WriteToFileList(string fileTitle, string writeToFileList)
    {
        folderName.text = "WORLD>" + fileTitle;
        fileList.text = writeToFileList;
    }

}
