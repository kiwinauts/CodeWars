using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Basic Settings")]
    public GameData CurrentGameData;

    [Header("Character")]
    public CharacterStats CurrentPlayerStats;

    [Header("Enemies")]
    public Enemy CurrentEnemyStats;

    public List<GameObject> Enemies;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }

        instance = this;
        
        //Create first attack
        CurrentGameData.CurrentAttacks.Clear();
        CurrentGameData.AddAttack(CurrentGameData.AvailableAttacks.FirstOrDefault(a => a.TurnsToActivate == 0));
        
        //Initialize enemy
        CurrentGameData.CurrentEnemyLevelIncrease = GetRandomLevelIncrease();
        CurrentEnemyStats.RespawnEnemy(Enemies.FirstOrDefault());
    }

    private int GetRandomLevelIncrease()
    {
        return Random.Range(CurrentGameData.IncreaseLevelOfEnemyAfterRoundMin, CurrentGameData.IncreaseLevelOfEnemyAfterRoundMax);
    }

    public void PlayerAttack(int attackId)
    {
        var attackSelected = CurrentGameData.CurrentAttacks.FirstOrDefault(a => a.Id == attackId);

        if (attackSelected == null)
        {
            IncreaseTurn();
            return;
        }

        CharacterAttackAndIncreaseTurn(CurrentPlayerStats, CurrentEnemyStats, attackSelected);
    }

    private void CharacterAttackAndIncreaseTurn(CharacterStats character, CharacterStats enemy, Attack attack)
    {
        character.PlayAttackAnimation();

        var (successAttack, attackDamage) = CurrentPlayerStats.AttackDamage(attack.Damage, attack.CriticalChance);

        if (successAttack)
        {
            PerformEnemyDamage(enemy, attackDamage);
        }

        IncreaseTurn();
    }

    private void PerformEnemyDamage(CharacterStats enemy, int attackDamage)
    {
        enemy.PlayDamageAnimation();
        enemy.DamageCharacter(attackDamage);

        if (enemy.IsDead)
        {
            enemy.Death();
            IncreaseRound();
        }
    }

    private void IncreaseTurn()
    {
        switch (CurrentGameData.CurrentTurn)
        {
            case Turn.Player:
                CurrentGameData.CurrentTurn = Turn.NPC;
                Invoke("EnemyTurn", CurrentGameData.EnemyThinkingTime);
                break;
            case Turn.NPC:
                CurrentGameData.CurrentTurn = Turn.Player;
                break;
            default:
                break;
        }
    }

    private void EnemyTurn()
    {
        CharacterAttackAndIncreaseTurn(CurrentEnemyStats, CurrentPlayerStats, CurrentEnemyStats.Attack);
    }

    private void IncreaseRound()
    {
        CurrentGameData.Round++;
        CurrentGameData.CurrentEnemyLevelIncrease--;

        if (CurrentGameData.CurrentEnemyLevelIncrease <= 0)
        {
            CurrentEnemyStats.IncreaseLevel();
            CurrentGameData.CurrentEnemyLevelIncrease = GetRandomLevelIncrease();
        }

        CurrentEnemyStats.RespawnEnemy(Enemies[Random.Range(0, Enemies.Count)]);
    }
}
