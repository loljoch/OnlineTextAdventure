using TMPro;
using UnityEngine;

public class TextCaretControl : MonoBehaviour
{
    private TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    //Makes sure the caret is always at the end
    private void Update()
    {
        SetCaretToEnd();
    }

    private void SetCaretToEnd()
    {
        inputField.caretPosition = inputField.text.Length;
    }
}
