
using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public int Round = 0;

    public Turn CurrentTurn = Turn.Player;

    public List<Attack> AvailableAttacks;

    public List<Attack> CurrentAttacks;

    public int IncreaseLevelOfEnemyAfterRound = 3;

    public int CurrentIncrease = 0;

    public float EnemyThinkingTime = 2f;

    public void AddAttack(Attack attackToAdd)
    {
        if (attackToAdd == null)
        {
            return;
        }

        CurrentAttacks.Add(attackToAdd);
    }
}
