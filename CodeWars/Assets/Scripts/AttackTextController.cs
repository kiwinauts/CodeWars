using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackTextController : MonoBehaviour
{
    public Text Name;

    public Text CriticalChance;

    public Text Damage;

    public Text RemainingTurns;

    public void UpdateAttack(AttackUI attack)
    {
        Name.text = attack.Name;
        CriticalChance.text = $"{attack.CriticalChance*100:F0}%";
        Damage.text = attack.Damage.ToString();
        RemainingTurns.text = attack.RemainingTurns == 0 ? "Done" : attack.RemainingTurns.ToString();
    }
}
