using System;

[Serializable]
public class CharacterStats
{
    public float CriticalChanceMutliplier = 1f;

    public int DamageBonus = 0;

    public float Evasion = 0.5f;

    public float Accuracy = 0.7f;

    public int MaxHealth = 100;

    public int CurrentHealth = 100;

    public CharacterStats()
    {
        CurrentHealth = MaxHealth;
    }

    public int DamageCharacter(int damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            Death();
            return 0;
        }

        return CurrentHealth;
    }

    public void Death()
    {

    }
}
