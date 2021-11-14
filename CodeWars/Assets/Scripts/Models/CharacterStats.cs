using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    public string Name;

    public float CriticalChanceMutliplier = 1f;

    public int DamageBonus = 0;

    public float Evasion = 0.5f;

    public float Accuracy = 0.7f;

    public int MaxHealth = 100;

    public int CurrentHealth = 100;

    public Animator Animator;

    public CharacterStats()
    {
        CurrentHealth = MaxHealth;
    }

    public bool DamageCharacter(int damage)
    {
        var evadeAttack = Random.Range(0f, 1f) < Evasion;
        
        if (evadeAttack)
        {
            Debug.Log($"{Name} -> evade Attack: {evadeAttack}");
            return false;
        }

        CurrentHealth -= damage;

        return true;
    }

    public bool IsDead
    {
        get
        {
            return CurrentHealth <= 0;
        }
    }

    public (bool, int) AttackDamage(int attackDamage, float attackCriticalChance)
    {
        var hit = Random.Range(0f, 1f) < Accuracy;

        if (!hit)
        {
            Debug.Log($"{Name} -> miss attack");
            return (hit, 0);
        }

        var damage = attackDamage + DamageBonus;

        var critical = Random.Range(0f, 1f) < attackCriticalChance * CriticalChanceMutliplier;

        if (critical)
        {
            damage *= 2;
        }

        Debug.Log($"{Name} -> Damage Attack: {damage} (Critical: {critical})");
        return (hit, damage);
    }

    public void PlayAttackAnimation()
    {
        const string AttackAnimationName = "Attack";
        Animator?.SetTrigger(AttackAnimationName);
    }

    public void PlayDamageAnimation()
    {
        const string DamageAnimationName = "Damage";
        Animator?.SetTrigger(DamageAnimationName);
    }

    public void Death()
    {
        const string DeathAnimationName = "Death";
        Animator?.SetTrigger(DeathAnimationName);
    }
}
