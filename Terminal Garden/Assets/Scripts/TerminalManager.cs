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
    [SerializeField] Text inputField;
    [SerializeField] Text terminalLog;
    [SerializeField] Text folderName;
    [SerializeField] Text fileList;
    [SerializeField] GameObject selectedIndicator;

    public GameObject selectedObject;

    // Start is called before the first frame update
    void Start()
    {
        inputField.text = "";
        terminalLog.text = "TYPE help FOR COMMAND REFERENCES";
        Keyboard.current.onTextInput += OnTextInput;
    }

    private void OnTextInput(char ch)
    {
        if (!char.IsControl(ch))
        {
            GameManager.Instance.terminalInput += ch;
            inputField.text = GameManager.Instance.terminalInput;
        }
    }

    public void Return(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            terminalLog.text += "\r\n" + GameManager.Instance.ExecuteCommand();
            inputField.text = "";
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

        selectedIndicator.SetActive(false);
    }
}
