using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public string Name;

    public float CriticalChance = 0.1f;

    public int DamageBonus = 0;

    public float Evasion = 0.1f;

    public float Accuracy = 0.7f;

    public int MaxHealth = 100;

    public int CurrentHealth = 100;

    public Animator Animator;

    public bool IsPlayer = false;

    public float MaxEvasion = 0.6f;

    public float MaxCritical = 0.6f;

    public int AttackAnimationCount = 1;

    private Attack currentAttack;

    public void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public bool DamageCharacter(int damage)
    {
        var evadeAttack = Random.Range(0f, 1f) < Evasion;

        if (evadeAttack)
        {
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

    public (bool, int, bool) AttackDamage(int attackDamage, float attackCriticalChance)
    {
        var hit = Random.Range(0f, 1f) < Accuracy;

        if (!hit)
        {
            return (hit, 0, false);
        }

        var damage = attackDamage + DamageBonus;

        var critical = Random.Range(0f, 1f) < (attackCriticalChance + CriticalChance);

        if (critical)
        {
            damage *= 2;
        }

        return (hit, damage, critical);
    }

    public void PlayAttackAnimation(Attack attack)
    {
        const string AttackAnimationName = "Attack";
        var attackAnimation = Random.Range(1, AttackAnimationCount + 1);
        Animator?.SetInteger(AttackAnimationName, attackAnimation);
        currentAttack = attack;
    }

    public void CharacterAttackedAnimationEvent()
    {
        const string AttackAnimationName = "Attack";
        GameManager.Instance.CharacterAttacked(this, currentAttack);
        Animator?.SetInteger(AttackAnimationName, 0);
    }

    public void PlayDamageAnimation()
    {
        const string DamageAnimationName = "Damage";
        Animator?.SetTrigger(DamageAnimationName);
    }

    public void DeathAndIncreaseRound()
    {
        const string DeathAnimationName = "Death";
        Animator?.SetTrigger(DeathAnimationName);
    }

    public void DestroyCharacterAnimationEvent()
    {
        Destroy(gameObject);
        GameManager.Instance.IncreaseRound(IsPlayer);
    }

    public StatsUI MapToStats()
    {
        return new StatsUI
        {
            Accuracy = Accuracy,
            CriticalChance = CriticalChance,
            Damage = DamageBonus,
            Evasion = Evasion
        };
    }
}
