using UnityEngine;

[System.Serializable]
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

    public bool DamageCharacter(int damage)
    {
        var evadeAttack = Random.Range(0, 1) > Evasion;

        if (evadeAttack)
        {
            return false;
        }

        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            Death();
            return true;
        }

        return true;
    }

    public (bool, int) AttackDamage(int attackDamage, float attackCriticalChance)
    {
        var hit = Random.Range(0, 1) < Accuracy;

        if (!hit)
        {
            return (hit, 0);
        }

        var damage = attackDamage + DamageBonus;

        var critical = Random.Range(0, 1) < attackCriticalChance * CriticalChanceMutliplier;

        if (critical)
        {
            damage *= 2;
        }

        return (hit, damage);
    }


    public void Death()
    {

    }
}
