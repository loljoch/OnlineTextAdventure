using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions.Generics.Singleton;
using TMPro;
using Extensions;
using System;

public class GameManager : GenericSingleton<GameManager, GameManager>
{
    public List<Character> players = new List<Character>();
    public List<Character> enemies = new List<Character>();
    public List<RectTransform> loseChars = new List<RectTransform>();

    [SerializeField] private TextMeshProUGUI charPrefab;
    [SerializeField] private TextMeshProUGUI commandPrefix;

    private delegate void StringCommand(string text);
    private Dictionary<string, StringCommand> commands;

    protected override void Awake()
    {
        InitializeCommands();
    }

    public void DecypherCommand(string text)
    {
        string[] words = text.Split(' ');
        Debug.Log($"First string: {words[0]}, Second string: {words[1]}");

        string command = words[0];
        string item = words[1];

        commands[command](item);
    }

    public void UseCommand(string text)
    {
        commandPrefix.text = "Used a ";
    }

    public void AttackCommand(string text)
    {
        commandPrefix.text = "Attacks with a " + text;
        StartCoroutine(ChangeWordToChar(text));
        StartCoroutine(SendCharsAt(enemies[0]));
    }

    private IEnumerator SendCharsAt(Character character)
    {
        yield return new WaitForSeconds(1f);

        Vector3 wantedPos = character.transform.position;

        for (int i = 0; i < loseChars.Count; i++)
        {
            loseChars[i].LerpRectTransform(wantedPos, 2f, character);
            yield return new WaitForSeconds(0.1f);
        }
    }

    [EasyAttributes.Button]
    public void TestFunction()
    {
        ChangeWordToChars("test");
    }

    private IEnumerator ChangeWordToChar(string word)
    {
        yield return new WaitForSeconds(0.5f);
        loseChars = ChangeWordToChars(word);
    }

    private List<RectTransform> ChangeWordToChars(string word)
    {
        if (!commandPrefix.text.Contains(word))
        {
            Debug.LogError($"\"{word}\" not found in string!");
            return null;
        }

        TMP_TextInfo textInfo = commandPrefix.textInfo;
        Debug.Log("First 3 chars of text info are: " + textInfo.characterInfo[0].character + textInfo.characterInfo[1].character + textInfo.characterInfo[2].character);
        List<RectTransform> charList = new List<RectTransform>();

        string commandText = commandPrefix.text;
        int wordIndex = commandText.IndexOf(word);
        int charCount = textInfo.characterCount;

        Debug.Log($"cText: {commandText}, wIndex: {wordIndex}, cCount: {charCount}");

        //The Y of the first letter, making sure every letter is on the right height
        TMP_CharacterInfo tempCInfo = textInfo.characterInfo[0];
        float firstY = VectorExtensions.Center(commandPrefix.transform.TransformPoint(tempCInfo.topRight), commandPrefix.transform.TransformPoint(tempCInfo.bottomLeft)).y;

        for (int i = wordIndex; i < charCount; i++)
        {
            TMP_CharacterInfo cInfo = textInfo.characterInfo[i];

            if (!cInfo.isVisible) continue;

            //Gets the center of the letter
            Vector3 bottomLeft = commandPrefix.transform.TransformPoint(cInfo.bottomLeft);
            Vector3 topRight = commandPrefix.transform.TransformPoint(cInfo.topRight);
            Vector3 center = VectorExtensions.Center(topRight, bottomLeft);
            center.y = firstY;

            //Spawn the characters
            TextMeshProUGUI obj = Instantiate(charPrefab, center, Quaternion.identity);
            string c = cInfo.character.ToString();
            obj.name = c;
            obj.text = c;
            obj.transform.parent = commandPrefix.transform;
            obj.transform.localScale = Vector3.one;

            charList.Add((RectTransform)obj.transform);
        }

        commandPrefix.text = commandText.Remove(wordIndex);

        return charList;
    }

    public void MenuCommand(string text)
    {

    }

    private void InitializeCommands()
    {
        commands = new Dictionary<string, StringCommand>()
        {
            {"Use", UseCommand},
            {"Attack", AttackCommand},
            {"Menu", MenuCommand},
        };
    }
}
