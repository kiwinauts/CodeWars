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

    public GameObject[] Enemies;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }

        instance = this;
        CurrentGameData.CurrentAttacks.Clear();
        CurrentGameData.AddAttack(CurrentGameData.AvailableAttacks.FirstOrDefault(a => a.TurnsToActivate == 0));
        CurrentGameData.CurrentEnemyLevelIncrease = GetRandomLevelIncrease();
    }

    private int GetRandomLevelIncrease()
    {
        return Random.Range(CurrentGameData.IncreaseLevelOfEnemyAfterRoundMin, CurrentGameData.IncreaseLevelOfEnemyAfterRoundMax);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerAttack(int attackId)
    {
        IncreaseTurn();
    }

    private void IncreaseTurn()
    {
        Debug.Log("Turn Ended");
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
        Debug.Log("Enemy Turn");
        IncreaseTurn();
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

        CurrentEnemyStats.RespawnEnemy(Enemies[Random.Range(0, Enemies.Length)]);
    }
}
