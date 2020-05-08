using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Extensions.String;


public class GameConsole : MonoBehaviour
{
    public TMP_InputField commandField;
    public UINavigation navigation;
    public Transform autoCorrectList;
    public AutoCorrectItem autoCorrectPrefab;

    private Field currentField = Field.command;
    private string[] availableCommands = new string[] { "Attack", "Use", "Menu" };
    private string[] availableItems = new string[] { "Axe", "Dagger", "Sword" };
    private List<AutoCorrectItem> spawnedItems = new List<AutoCorrectItem>();

    private int textSpaces = 10;

    private void Awake()
    {
        commandField.onValueChanged.AddListener(CheckString);
        commandField.onSubmit.AddListener(CheckIfCommand);
    }

    public void CheckIfCommand(string text)
    {
        //string[] words = commandField.text.Split(' ');

        //if (words == null) return;
        //if (words.Length == 2) return;


        //if (words[0] == availableCommands[(int)Commands.menu])
        //{
        //    GameManager.Instance.OpenMenu();
        //} else if (words.Length == 3)
        //{
        //    if (words[0] == availableCommands[(int)Commands.attack])
        //    {
        //        GameManager.Instance.Attack(words[1], words[2]);
        //    } else
        //    {
        //        GameManager.Instance.Use(words[1], words[2]);
        //    }
        //}
    }

    public void CheckString(string text)
    {
        int spaces = text.SpecificCharacterCount(' ');
        currentField = (spaces < (int)Field.invalid)? (Field)spaces : Field.invalid;

        if (textSpaces != spaces)
        {
            ShowAutoCorrect();
            textSpaces = spaces;
        }
    }

    public void ShowAutoCorrect()
    {
        string[] correctWords = null;

        //switch (currentField)
        //{
        //    case Field.command:
        //        correctWords = availableCommands;
        //        break;
        //    case Field.focus:
        //        correctWords = GameManager.Instance.GetEnemyNames();
        //        break;
        //    case Field.item:
        //        if(commandField.text.Split(' ')[0] == availableCommands[0])
        //        correctWords = GameManager.Instance.GetItemNames(
        //            (commandField.text.Split(' ')[0] == availableCommands[0]) ? Game.Inventory.TargetableOn.Attackable : Game.Inventory.TargetableOn.Usable);
        //        break;
        //}

        if (correctWords == null)
        {
            autoCorrectList.gameObject.SetActive(false);
            return;
        }

        autoCorrectList.gameObject.SetActive(true);

        //Deactivate items
        for (int i = 0; i < spawnedItems.Count; i++)
        {
            spawnedItems[i].gameObject.SetActive(false);
        }

        //Set the items to the text
        for (int i = 0; i < correctWords.Length; i++)
        {
            AutoCorrectItem cItem;
            if (i > spawnedItems.Count - 1)
            {
                cItem = Instantiate(autoCorrectPrefab, autoCorrectList);
                spawnedItems.Add(cItem);
            } else
            {
                cItem = spawnedItems[i];
            }
            cItem.name = correctWords[i];
            cItem.SetText(correctWords[i]);

            cItem.gameObject.SetActive(true);
        }

        //Manage navigation
        for (int i = 0; i < spawnedItems.Count; i++)
        {
            if (i == 0 && i == spawnedItems.Count - 1)
            {
                spawnedItems[i].navigation.onSelectUp = commandField;
                spawnedItems[i].navigation.onSelectDown = commandField;
            } else if (i == spawnedItems.Count - 1)
            {
                spawnedItems[i].navigation.onSelectUp = spawnedItems[i - 1].selectable;
                spawnedItems[i].navigation.onSelectDown = commandField;
            } else if (i == 0)
            {
                spawnedItems[i].navigation.onSelectUp = commandField;
                spawnedItems[i].navigation.onSelectDown = spawnedItems[i + 1].selectable;
            } else
            {
                spawnedItems[i].navigation.onSelectUp = spawnedItems[i - 1].selectable;
                spawnedItems[i].navigation.onSelectDown = spawnedItems[i + 1].selectable;
            }
        }

        navigation.onSelectUp = spawnedItems[spawnedItems.Count - 1].selectable;
        navigation.onSelectDown = spawnedItems[0].selectable;
    }

    public void AutoCorrect(string word)
    {
        string text = commandField.text;
        int wordLength = text.Split(' ')[(int)currentField].Length;
        int index = (currentField != Field.item)? text.IndexOf(' ') + 1 : text.LastIndexOf(' ') + 1;

        text = text.Remove(index, wordLength);
        text = text.Insert(index, word + ' ');

        commandField.text = text;
    }

    private enum Field
    {
        command = 0,
        focus = 1,
        item = 2,
        invalid = 3
    }

    private enum Commands
    {
        attack = 0,
        use = 1,
        menu = 2
    }
}
