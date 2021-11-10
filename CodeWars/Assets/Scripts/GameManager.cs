using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameData CurrentGameData;

    public CharacterStats CurrentPlayerStats;

    public Enemy CurrentEnemyStats;

    public GameObject[] Enemies;

    // Start is called before the first frame update
    void Start()
    {
        CurrentGameData.CurrentAttacks.Clear();
        CurrentGameData.AddAttack(CurrentGameData.AvailableAttacks.FirstOrDefault(a => a.TurnsToActivate == 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseTurn()
    {
        Debug.Log("Turn Ended");
        switch (CurrentGameData.CurrentTurn)
        {
            case Turn.Player:
                CurrentGameData.CurrentTurn = Turn.NPC;
                Invoke("EnemiesTurn", CurrentGameData.EnemyThinkingTime);
               break;
            case Turn.NPC:
                CurrentGameData.CurrentTurn = Turn.Player;
                break;
            default:
                break;
        }
    }

    public void EnemiesTurn()
    {
        Debug.Log("Enemy Turn");
        IncreaseTurn();
    }

    public void IncreaseRound()
    {
        CurrentGameData.Round++;
        CurrentGameData.CurrentIncrease++;

        if (CurrentGameData.CurrentIncrease >= CurrentGameData.IncreaseLevelOfEnemyAfterRound)
        {
            CurrentEnemyStats.IncreaseLevel();
            CurrentGameData.CurrentIncrease = 0;
        }

        CurrentEnemyStats.RespawnEnemy(Enemies[Random.Range(0, Enemies.Length)]);
    }
}
