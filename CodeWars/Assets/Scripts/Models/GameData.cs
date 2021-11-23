
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int Round = 1;

    public Turn CurrentTurn = Turn.Player;

    public float IncreaseTurnDelay = 2f;

    public float NewRoundDelay = 1f;

    [Header("Character")]
    public List<Attack> AvailableAttacks;

    public List<Attack> CurrentAttacks;

    [Header("Enemies")]
    public int IncreaseLevelOfEnemyAfterRoundMin = 2;

    public int IncreaseLevelOfEnemyAfterRoundMax = 5;

    public int CurrentEnemyLevelIncrease = 0;

    public GameObject Enemy;

    [Range(1,1)]
    public int CurrentEnemyLevel = 1;

    public Vector3 SpawnPosition;

    public Quaternion SpawnRotation;

    public void AddAttack(Attack attackToAdd)
    {
        if (attackToAdd == null)
        {
            return;
        }
        CurrentAttacks.Add(attackToAdd);
    }
}
