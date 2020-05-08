using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(TextMeshProUGUI))]
public class HealthBar : MonoBehaviour
{
    public Character testCharacter;
    private List<Character> characters = new List<Character>();
    private TextMeshProUGUI healthBar;

    private void Awake()
    {
        healthBar = GetComponent<TextMeshProUGUI>();
    }

    [EasyAttributes.Button]
    public void AssignTestCharacter()
    {
        AssignCharacter(testCharacter);
    }

    public void AssignCharacter(params Character[] character)
    {
        for (int i = 0; i < character.Length; i++)
        {
            characters.Add(character[i]);
            characters[i].OnHealthChange += UpdateHealth;
        }

        UpdateHealth();
    }

    public void UpdateHealth()
    {
        int current = 0;
        int max = 0;
        for (int i = 0; i < characters.Count; i++)
        {
            current += characters[i].currentHealth;
            max += characters[i].maxHealth;
        }

        healthBar.SetText($"{current}/{max}");
    }
}
