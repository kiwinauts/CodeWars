using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Basic Settings")]
    public GameData CurrentGameData;

    [Header("Characters")]

    public CharacterStats CurrentPlayerStats;

    public Enemy CurrentEnemyStats;

    public UpdateVm[] Updates;

    public GameObject[] Enemies;

    private UIManager uiManager;

    private bool clickedEscape;

    // Start is called before the first frame update
    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;
        uiManager = GetComponent<UIManager>();

        //First Round
        CurrentGameData.Round = 1;
        uiManager.UpdateRound(CurrentGameData.Round);

        //Create first attack
        CurrentGameData.CurrentAttacks.Clear();
        CurrentGameData.AddAttack(CurrentGameData.AvailableAttacks.FirstOrDefault(a => a.TurnsToActivate == 0));
        InitializeAttacks();
        uiManager.CreateAttackButtons(CurrentGameData.CurrentAttacks);

        //Initialize enemy
        CurrentGameData.CurrentEnemyLevelIncrease = GetRandomLevelIncrease();
        RespawnEnemy();

        //Initialize Player health
        uiManager.UpdateCharacterHealth(1.0f * CurrentPlayerStats.CurrentHealth / CurrentPlayerStats.MaxHealth, true, CurrentPlayerStats.CurrentHealth, CurrentPlayerStats.MaxHealth);

        //Updates
        AddInUpdatesTheNewAttacks();

        //Stats
        uiManager.ChangeCharacterStats(CurrentPlayerStats.MapToStats(), true);
        uiManager.ChangeCharacterStats(CurrentEnemyStats.MapToStats(), false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !clickedEscape)
        {
            clickedEscape = true;
            CurrentPlayerStats.DeathAndIncreaseRound();
            return;
        }
    }

    private void AddInUpdatesTheNewAttacks()
    {
        var attacksToUnlock = CurrentGameData.AvailableAttacks.Where(a => !CurrentGameData.CurrentAttacks.Contains(a));
        foreach (var attack in attacksToUnlock)
        {
            Updates = Updates.Append(new UpdateVm
            {
                updateType = UpdateType.NewAttackMove,
                Attack = attack
            }).ToArray();
        }
    }

    private void InitializeAttacks()
    {
        int id = 0;
        CurrentGameData.AvailableAttacks = CurrentGameData.AvailableAttacks.OrderBy(a => a.Damage).ToList();
        foreach (var attack in CurrentGameData.AvailableAttacks)
        {
            attack.Id = id;
            attack.Initialize();
            id++;
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
            uiManager.UpdateCharacterHealth(1.0f * CurrentEnemyStats.CurrentHealth / CurrentEnemyStats.MaxHealth, false, CurrentEnemyStats.CurrentHealth, CurrentEnemyStats.MaxHealth);
            uiManager.UpdateEnemyLevel(CurrentEnemyStats.Level);
        }
        uiManager.ChangeCharacterStats(CurrentEnemyStats.MapToStats(), false);
    }

    private int GetRandomLevelIncrease()
    {
        return Random.Range(CurrentGameData.IncreaseLevelOfEnemyAfterRoundMin, CurrentGameData.IncreaseLevelOfEnemyAfterRoundMax);
    }

    public void PlayerAttack(Attack attackSelected)
    {
        uiManager.DisableCurrentAttacks();

        if (attackSelected == null)
        {
            IncreaseTurn();
            return;
        }

        CurrentPlayerStats.PlayAttackAnimation(attackSelected);

        if (attackSelected.TurnsToActivate != 0)
        {
            attackSelected.Reset();
        }
    }

    public void CharacterAttacked(CharacterStats character, Attack attack)
    {
        CharacterStats enemy = character.IsPlayer ? CurrentEnemyStats : CurrentPlayerStats;

        var (successAttack, attackDamage, critical) = character.AttackDamage(attack.Damage, attack.CriticalChance);

        var endRound = false;
        if (successAttack)
        {
            endRound = PerformEnemyDamageAndCheckForEndRound(enemy, attackDamage, critical);
        }
        else
        {
            uiManager.MissMessage();
        }

        if (!endRound)
        {
            IncreaseTurn();
        }
    }

    private bool PerformEnemyDamageAndCheckForEndRound(CharacterStats character, int attackDamage, bool critical)
    {
        character.PlayDamageAnimation();
        var hitTarget = character.DamageCharacter(attackDamage);
        uiManager.UpdateCharacterHealth(character.CurrentHealth * 1.0f / character.MaxHealth, character.IsPlayer, character.CurrentHealth, character.MaxHealth);

        if (!hitTarget)
        {
            uiManager.EvadeMessage();
        }
        else if (critical)
        {
            uiManager.CriticalHitMessage();
        }

        if (!character.IsDead)
        {
            return false;
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

    public void IncreaseRound(bool endGame)
    {
        if (endGame)
        {
            EndGame();
            return;
        }

        CurrentGameData.Round++;
        uiManager.UpdateRound(CurrentGameData.Round);

        CurrentGameData.CurrentEnemyLevelIncrease--;
        if (CurrentGameData.CurrentEnemyLevelIncrease <= 0)
        {
            CurrentGameData.CurrentEnemyLevel++;
            CurrentGameData.CurrentEnemyLevelIncrease = GetRandomLevelIncrease();
        }

        RespawnEnemy();

        ChooseUpgradeAndSendToUIManager();
        CurrentGameData.CurrentTurn = Turn.Player;
        InitializeAttacks();
    }

    public void ChooseUpgradeAndSendToUIManager()
    {
        Updates = Updates.OrderBy(x => Random.Range(0, 1) >= 0.5f).ToArray();

        var updatesToChoose = 3;
        if (Updates.Count() < 3)
        {
            updatesToChoose = Updates.Count();
        }
        else if (Updates.Count() == 0)
        {
            uiManager.CreateAttackButtons(CurrentGameData.CurrentAttacks);
            return;
        }

        var randomUpdates = new List<UpdateVm>();
        var currentUIPosition = 0;

        while (randomUpdates.Count < updatesToChoose)
        {
            var chooseElement = Random.Range(0f, 1f);

            var shouldChooseElement = (updatesToChoose * 1f - randomUpdates.Count) / (Updates.Length - currentUIPosition);

            if (chooseElement <= shouldChooseElement)
            {
                randomUpdates.Add(Updates[currentUIPosition]);
            }

            currentUIPosition++;
        }

        uiManager.ShowUpdates(randomUpdates);
    }

    public void ChosenUpdate(UpdateVm update)
    {
        uiManager.HideAllUpdates();

        var removeUpdate = false;
        switch (update.updateType)
        {
            case UpdateType.NewAttackMove:
                CurrentGameData.AddAttack(update.Attack);
                removeUpdate = true;
                break;
            case UpdateType.BonusAttackDamage:
                CurrentPlayerStats.DamageBonus += System.Convert.ToInt32(update.UpdateValue);
                break;
            case UpdateType.BonusHealth:
                var healthUpdate = System.Convert.ToInt32(update.UpdateValue);
                CurrentPlayerStats.MaxHealth += healthUpdate;
                CurrentPlayerStats.CurrentHealth = Mathf.Min(CurrentPlayerStats.MaxHealth, CurrentPlayerStats.CurrentHealth + healthUpdate);
                break;
            case UpdateType.BonusCriticalChance:
                CurrentPlayerStats.CriticalChance = Mathf.Min(CurrentPlayerStats.MaxCritical, CurrentPlayerStats.CriticalChance + update.UpdateValue);
                
                if (CurrentPlayerStats.CriticalChance == CurrentPlayerStats.MaxCritical)
                {
                    removeUpdate = true;
                }

                break;
            case UpdateType.BonusAccuracy:
                CurrentPlayerStats.Accuracy = Mathf.Min(1f, CurrentPlayerStats.Accuracy + update.UpdateValue);
                if (CurrentPlayerStats.Accuracy == 1f)
                {
                    removeUpdate = true;
                }
                break;
            case UpdateType.BonusEvasion:
                CurrentPlayerStats.Evasion = Mathf.Min(CurrentPlayerStats.MaxEvasion, CurrentPlayerStats.Evasion + update.UpdateValue);
                if (CurrentPlayerStats.Evasion == CurrentPlayerStats.MaxEvasion)
                {
                    removeUpdate = true;
                }
                break;
            case UpdateType.FullHealth:
                CurrentPlayerStats.CurrentHealth = CurrentPlayerStats.MaxHealth;
                break;
            default:
                break;
        }

        if (removeUpdate)
        {
            Updates = Updates.Where(u => !u.Equals(update)).ToArray();
        }

        uiManager.UpdateCharacterHealth(CurrentPlayerStats.CurrentHealth * 1.0f / CurrentPlayerStats.MaxHealth, true, CurrentPlayerStats.CurrentHealth, CurrentPlayerStats.MaxHealth);
        uiManager.ChangeCharacterStats(CurrentPlayerStats.MapToStats(), true);
        uiManager.CreateAttackButtons(CurrentGameData.CurrentAttacks);
    }
}
