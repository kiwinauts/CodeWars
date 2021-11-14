using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public string Name;

    public float CriticalChanceMutliplier = 1f;

    public int DamageBonus = 0;

    public float Evasion = 0.1f;

    public float Accuracy = 0.7f;

    public int MaxHealth = 100;

    public int CurrentHealth = 100;

    public Animator Animator;

    public bool IsPlayer = false;

    public void Start()
    {
        CurrentHealth = MaxHealth;
        Animator = GetComponent<Animator>();
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

    public void DeathAndIncreaseRound()
    {
        const string DeathAnimationName = "Death";
        Animator?.SetTrigger(DeathAnimationName);
        Invoke("DestroyCharacter", 1f);
    }

    public void DestroyCharacter()
    {
        Destroy(gameObject);
        GameManager.instance.IncreaseRound();
    }
}
