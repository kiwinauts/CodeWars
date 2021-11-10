using System;
using UnityEngine;

[Serializable]
public class Attack
{
    public string Name;

    public string Description;

    public int Damage = 5;

    [Range(0, 1)]
    public float CriticalChance = 0;

    public int TurnsToActivate = 0;

    private int RemainingTurns = 0;

    public bool isActive
    {
        get { return RemainingTurns == 0; }
    }

    public Attack()
    {
        RemainingTurns = TurnsToActivate;
    }

    public void ProgressOneTurn()
    {
        TurnsToActivate--;

        if (TurnsToActivate < 0)
        {
            TurnsToActivate = 0;
        }
    }
}
