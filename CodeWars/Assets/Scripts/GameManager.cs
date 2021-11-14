using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Basic Settings")]
    public GameData CurrentGameData;

    [Header("Objects")]

    public CharacterStats CurrentPlayerStats;

    public Enemy CurrentEnemyStats;

    public GameObject[] Enemies;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }

        instance = this;

        //First Round
        CurrentGameData.Round = 1;

        //Create first attack
        CurrentGameData.CurrentAttacks.Clear();
        CurrentGameData.AddAttack(CurrentGameData.AvailableAttacks.FirstOrDefault(a => a.TurnsToActivate == 0));

        //Initialize enemy
        CurrentGameData.CurrentEnemyLevelIncrease = GetRandomLevelIncrease();
        RespawnEnemy();
    }

    private void RespawnEnemy()
    {
        var enemyToSpawn = Enemies[Random.Range(0, Enemies.Length)];
        var instantiatedEnemy = Instantiate(enemyToSpawn, CurrentGameData.SpawnPosition, CurrentGameData.SpawnRotation);
        CurrentEnemyStats = instantiatedEnemy.GetComponent<Enemy>();
        if (CurrentEnemyStats != null)
        {
            CurrentEnemyStats.SetupLevel(CurrentGameData.CurrentEnemyLevel);
        }
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

        var (successAttack, attackDamage) = character.AttackDamage(attack.Damage, attack.CriticalChance);

        var endRound = false;
        if (successAttack)
        {
            endRound = PerformEnemyDamageAndCheckForEndRound(enemy, attackDamage);
        }

        if (!endRound)
        {
            IncreaseTurn();
        }
    }

    private bool PerformEnemyDamageAndCheckForEndRound(CharacterStats character, int attackDamage)
    {
        character.PlayDamageAnimation();
        character.DamageCharacter(attackDamage);

        if (!character.IsDead)
        {
            return false;
        }

        if (character.IsPlayer)
        {
            EndGame();
            return true;
        }

        character.DeathAndIncreaseRound();
        return true;
    }

    private void EndGame()
    {
        Debug.Log("End Game");
        SceneManager.LoadScene(0);
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

    public void IncreaseRound()
    {
        CurrentGameData.Round++;
        CurrentGameData.CurrentEnemyLevelIncrease--;

        if (CurrentGameData.CurrentEnemyLevelIncrease <= 0)
        {
            CurrentGameData.CurrentEnemyLevel++;
            CurrentGameData.CurrentEnemyLevelIncrease = GetRandomLevelIncrease();
        }

        RespawnEnemy();

        //Choose Upgrade
        CurrentGameData.CurrentTurn = Turn.Player;
    }
}
