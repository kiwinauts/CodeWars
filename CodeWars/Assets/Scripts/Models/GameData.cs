
using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public int Round = 0;

    public Turn CurrentTurn = Turn.Player;

    public List<Attack> AvailableAttacks;

    public List<Attack> CurrentAttacks;

    public int IncreaseLevelOfEnemyAfterRoundMin = 2;

    public int IncreaseLevelOfEnemyAfterRoundMax = 5;

    public int CurrentEnemyLevelIncrease = 0;

    public float EnemyThinkingTime = 2f;

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
