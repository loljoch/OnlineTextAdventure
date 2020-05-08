using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoCorrectItem : MonoBehaviour
{
    [HideInInspector] public UINavigation navigation;
    [HideInInspector] public Button selectable;
    private TextMeshProUGUI textField;

    private void Awake()
    {
        textField = GetComponentInChildren<TextMeshProUGUI>();
        navigation = GetComponent<UINavigation>();
        selectable = GetComponent<Button>();
    }

    public void SetListeners(UINavigation inputNav, Action<string> autocorrectFunction)
    {
        selectable.onClick.AddListener(inputNav.selectable.Select);
        selectable.onClick.AddListener(() => autocorrectFunction(textField.text));
    }


    public void SetText(string text)
    {
        textField.SetText(text);
    }

}
