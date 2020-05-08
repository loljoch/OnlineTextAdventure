using UnityEngine;
using Game.Inventory;

public class Character : MonoBehaviour
{
    public new string name = "Jack";
    public int maxHealth = 10;
    public int currentHealth = 10;

    public delegate void onHealthChange();
    public onHealthChange OnHealthChange;


    public void TakeDamage(int amount)
    {
        currentHealth -= ((currentHealth - amount) < 0)? currentHealth : amount;

        OnHealthChange?.Invoke();
    }
}
