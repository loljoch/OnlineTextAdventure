using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameConsole : MonoBehaviour
{
    public TMP_InputField inputField;

    private void Awake()
    {
        inputField.onSubmit.AddListener(x => SendString(x));
    }

    public void SendString(string text)
    {
        GameManager.Instance.DecypherCommand(text);
    }
}
