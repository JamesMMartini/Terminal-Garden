using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TerminalManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Keyboard.current.onTextInput += OnTextInput;
    }

    private void OnTextInput(char ch)
    {
        if (!char.IsControl(ch))
            GameManager.Instance.terminalInput += ch;
    }

    public void Return(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.Instance.ExecuteCommand();
        }
    }
}
