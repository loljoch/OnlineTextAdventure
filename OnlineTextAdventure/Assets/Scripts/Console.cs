using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Extensions.String;
using Extensions;

public class Console : MonoBehaviour
{
    private static readonly char[] VALID_CHARS = new char[] { ' ', '_', '(', ')' };

    public TMP_InputField inputField;
    public TMP_Text recommendedText;
    public InputState inputState = InputState.StartMenu;

    [Header("Autocorrect")]
    public UINavigation inputFieldUINavigation;
    [SerializeField] private Transform autocorrectList;
    [SerializeField] private AutoCorrectItem autoCorrectPrefab;

    //Commands
    public delegate void Command(string item, string target);

    private Dictionary<string, Command> commands;
    private string[] mainCommands;
    private string[] battleCommands;
    private string[] possibleCommands;

    private void Awake()
    {
        inputField.onValidateInput += ValidateInput;

        inputField.onValueChanged.AddListener(RefreshGameState);
        inputField.onValueChanged.AddListener(SetRecommendedText);
        inputField.onValueChanged.AddListener(ShowAutoCorrect);

        inputField.onEndEdit.AddListener(ValidateCommand);

        InitializeCommands();
    }

    private void Update()
    {
        if (!inputField.isFocused) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            AutofillRecommended();
        } else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Backspace))
        {
            RemoveWholeWord();
        }
    }

    private void ValidateCommand(string cText)
    {
        List<string> wordList = new List<string>(cText.Split(' '));

        if (!commands.ContainsKey(wordList[0]))
        {
            Debug.Log("Invalid command: " + wordList[0]);
            return;
        }

        while (wordList.Count < 3)
        {
            wordList.Add(string.Empty);
        }
        

        commands[wordList[0]](wordList[1], wordList[2]);
    }

    #region InputCheckFunctions

    private void RefreshGameState(string cText)
    {
        //If empty text then return
        if (cText.Length == 0) return;

        int spaces = cText.SpecificCharacterCount(' ');
        string[] words = cText.Split(' ');

        //If only 1 word
        if (spaces == 0)
        {
            inputState = InputState.Battle;
        }

        //If trying to use things, check if on second or third word
        if (words[0].ToLower() == "use")
        {
            if (spaces == 1)
            {
                inputState = InputState.Item;
            } else if(spaces == 2)
            {
                inputState = InputState.Focus;
            }
        }

        //Match possible commands to inputstate
        switch (inputState)
        {
            case InputState.StartMenu:
                possibleCommands = mainCommands;
                break;
            case InputState.Battle:
                possibleCommands = battleCommands;
                break;
            case InputState.Item:
                possibleCommands = GameManager.Instance.GetItemNames();
                break;
            case InputState.Focus:
                possibleCommands = GameManager.Instance.GetEnemyNames();
                break;
        }

        //UNITY CAN NOT COMPILE THIS YET
        //gameState = (words[0].ToLower(), spaces) switch
        //{
        //    ("use", 1) => GameState.Item,
        //    ("use", 2) => GameState.Focus,
        //    (_, _) => gameState
        //};
    }

    private char ValidateInput(string cText, int charIndex, char addedChar)
    {
        //Only allow letters/digits/VALID_CHARS
        if (!char.IsLetterOrDigit(addedChar) && !addedChar.MatchesAny(VALID_CHARS)) return char.MinValue;

        if (addedChar != ' ') return addedChar;

        //If the previous char is a space, return nothing
        if (cText.Length >= 1 && cText[charIndex - 1] != ' ')
        {
            return addedChar;
        } else
        {
            return char.MinValue;
        }
    }

    private void SetRecommendedText(string cText)
    {
        int firstSpaceIndex = cText.ClosestFirstIndexOf(inputField.caretPosition, ' ') + 1;

        string cWord = cText.Substring(firstSpaceIndex);
        string recString = cText.Substring(0, firstSpaceIndex);

        if (cWord.Trim().Length == 0)
        {
            recommendedText.text = string.Empty;
        } else
        {
            for (int i = 0; i < possibleCommands.Length; i++)
            {
                if (cWord.Trim().ToLower().Matches(possibleCommands[i].ToLower()))
                {
                    recommendedText.text = recString + SetSameCasing(cWord, possibleCommands[i]);
                    break;
                } else
                {
                    recommendedText.text = string.Empty;
                }
            }
        }
    }

    private string SetSameCasing(string cText, string command)
    {
        string newCommand = "";
        int length = cText.Length;

        for (int i = 0; i < command.Length; i++)
        {
            if (i < length)
            {
                newCommand += cText[i];
            } else
            {
                newCommand += command[i];
            }
        }

        return newCommand;
    }

    #endregion

    #region AutocorrectFunctions
    private void ShowAutoCorrect(string cText)
    {
        autocorrectList.gameObject.SetActive(true);
        autocorrectList.DestroyChildren();

        List<UINavigation> items = new List<UINavigation>();

        items.Add(inputFieldUINavigation);

        for (int i = 0; i < possibleCommands.Length; i++)
        {
            AutoCorrectItem item = Instantiate(autoCorrectPrefab, autocorrectList);
            item.SetText(possibleCommands[i]);
            item.SetListeners(inputFieldUINavigation, Autocorrect);
            items.Add(item.navigation);
        }


        //Manage navigation
        for (int i = 0; i < items.Count; i++)
        {
            if (i == 0)
            {
                items[i].onSelectUp = items[items.Count - 1].selectable;
                items[i].onSelectDown = items[i + 1].selectable;
            } else if(i == items.Count - 1)
            {
                items[i].onSelectUp = items[i - 1].selectable;
                items[i].onSelectDown = items[0].selectable;
            } else
            {
                items[i].onSelectUp = items[i - 1].selectable;
                items[i].onSelectDown = items[i + 1].selectable;
            }
        }
    }

    #endregion

    #region Shortcuts

    private void AutofillRecommended()
    {
        if (recommendedText.text == string.Empty) return;

        //Fill text
        inputField.text = recommendedText.text;

        SetCaretToEnd();
    }

    private void Autocorrect(string text)
    {
        SetCaretToEnd();
        RemoveWholeWord();
        inputField.text += text;
        SetCaretToEnd();
    }

    private void RemoveWholeWord()
    {
        string cText = inputField.text;
        int caretIndex = inputField.caretPosition;
        int closestSpaceIndex = cText.ClosestFirstIndexOf(caretIndex, ' ') + 1;

        inputField.text = cText.Remove(closestSpaceIndex, caretIndex - closestSpaceIndex);
    }
    #endregion

        
    private void SetCaretToEnd() => inputField.caretPosition = inputField.text.Length;

    private void InitializeCommands()
    {
        commands = new Dictionary<string, Command>
        {
            {"start", StartGame },
            {"use", Use }
        };

        mainCommands = new string[] { "Start", "Help" };
        battleCommands = new string[] { "Use", "Help" };
}

    private void StartGame(string item, string target)
    {
        Debug.Log("Started the game");
    }

    private void Use(string item, string target)
    {
        Debug.Log($"Used {item} on {target}");
    }


    public enum InputState
    {
        StartMenu,
        Battle,
        Item,
        Focus
    }
}
