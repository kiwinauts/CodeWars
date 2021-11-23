using System;
using UnityEngine;

[Serializable]
public class Attack
{
    public string Name;

    public int Id;

    public int Damage = 5;

    [Range(0, 1)]
    public float CriticalChance = 0;

    public int TurnsToActivate = 0;

    public int RemainingTurns = 0;

    public ParticleSystem AttackParticles;

    public bool isActive
    {
        get { return RemainingTurns == 0; }
    }

    public void Initialize()
    {
        RemainingTurns = TurnsToActivate;
    }

    public void ProgressOneTurn()
    {
        RemainingTurns--;

        if (RemainingTurns < 0)
        {
            RemainingTurns = 0;
        }
    }

    public AttackUI MapToAttackUI()
    {
        return new AttackUI
        {
            CriticalChance = CriticalChance,
            Damage = Damage,
            Name = Name,
            RemainingTurns = RemainingTurns
        };
    }

    public void Reset()
    {
        RemainingTurns = TurnsToActivate + 1;
    }
}
