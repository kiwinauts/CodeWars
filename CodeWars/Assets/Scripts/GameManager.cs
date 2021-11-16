using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Basic Settings")]
    public GameData CurrentGameData;

    [Header("Characters")]

    public CharacterStats CurrentPlayerStats;

    public Enemy CurrentEnemyStats;

    public GameObject[] Enemies;

    private UIManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }

        instance = this;
        uiManager = GetComponent<UIManager>();

        //First Round
        CurrentGameData.Round = 1;

        //Create first attack
        CurrentGameData.CurrentAttacks.Clear();
        CurrentGameData.AddAttack(CurrentGameData.AvailableAttacks.FirstOrDefault(a => a.TurnsToActivate == 0));
        InitializeAttacks();
        uiManager.CreateAttackButtons(CurrentGameData.CurrentAttacks);

        //Initialize enemy
        CurrentGameData.CurrentEnemyLevelIncrease = GetRandomLevelIncrease();
        RespawnEnemy();

        //Initialize Player health
        uiManager.UpdateCharacterHealth(1.0f * CurrentPlayerStats.CurrentHealth / CurrentPlayerStats.MaxHealth, true);
    }

    private void InitializeAttacks()
    {
        foreach (var attack in CurrentGameData.AvailableAttacks)
        {
            attack.Initialize();
        }
    }

    private void RespawnEnemy()
    {
        var enemyToSpawn = Enemies[Random.Range(0, Enemies.Length)];
        var instantiatedEnemy = Instantiate(enemyToSpawn, CurrentGameData.SpawnPosition, CurrentGameData.SpawnRotation);
        CurrentEnemyStats = instantiatedEnemy.GetComponent<Enemy>();
        if (CurrentEnemyStats != null)
        {
            CurrentEnemyStats.SetupLevel(CurrentGameData.CurrentEnemyLevel);
            uiManager.UpdateCharacterHealth(1.0f * CurrentEnemyStats.CurrentHealth / CurrentEnemyStats.MaxHealth, false);
            uiManager.UpdateEnemyLevel(CurrentEnemyStats.Level);
        }
    }

    private int GetRandomLevelIncrease()
    {
        return Random.Range(CurrentGameData.IncreaseLevelOfEnemyAfterRoundMin, CurrentGameData.IncreaseLevelOfEnemyAfterRoundMax);
    }

    public void PlayerAttack(int attackId)
    {
        uiManager.DisableCurrentAttacks();

        var attackSelected = CurrentGameData.CurrentAttacks.FirstOrDefault(a => a.Id == attackId);

        if (attackSelected == null)
        {
            IncreaseTurn();
            return;
        }

        CurrentPlayerStats.PlayAttackAnimation(attackSelected);
    }

    public void CharacterAttacked(CharacterStats character, Attack attack)
    {
        CharacterStats enemy = character.IsPlayer ? CurrentEnemyStats : CurrentPlayerStats;

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
        uiManager.UpdateCharacterHealth(character.CurrentHealth * 1.0f / character.MaxHealth, character.IsPlayer);

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
                EnemyTurn();
                break;
            case Turn.NPC:
                ProgressAttacks();
                CurrentGameData.CurrentTurn = Turn.Player;
                break;
            default:
                break;
        }
    }

    private void ProgressAttacks()
    {
        foreach (var attack in CurrentGameData.CurrentAttacks)
        {
            attack.ProgressOneTurn();
        }
        uiManager.CreateAttackButtons(CurrentGameData.CurrentAttacks);
    }

    private void EnemyTurn()
    {
        CurrentEnemyStats.PlayAttackAnimation(CurrentEnemyStats.Attack);
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
