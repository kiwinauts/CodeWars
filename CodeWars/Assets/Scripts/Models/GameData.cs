
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int Round = 1;

    public Turn CurrentTurn = Turn.Player;
    
    [Header("Character")]
    public List<Attack> AvailableAttacks;

    public List<Attack> CurrentAttacks;

    [Header("Enemies")]
    public int IncreaseLevelOfEnemyAfterRoundMin = 2;

    public int IncreaseLevelOfEnemyAfterRoundMax = 5;

    public int CurrentEnemyLevelIncrease = 0;

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
        attackToAdd.Id = CurrentAttacks.Count;
        CurrentAttacks.Add(attackToAdd);
    }
}
