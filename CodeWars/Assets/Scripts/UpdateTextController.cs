using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTextController : MonoBehaviour
{
    public Text Name;

    public Text UpdateValue;

    public Text RoundsToUnlock;

    public Text CriticalChance;

    public Text Damage;

    public void UpdateText(UpdateVm vm)
    {
        Name.text = vm.Name;
        UpdateValue.text = "";
        CriticalChance.text = "";
        RoundsToUnlock.text = "";
        Damage.text = "";

        switch (vm.updateType)
        {
            case UpdateType.NewAttackMove:
                {
                    Name.text = vm.Attack?.Name;
                    CriticalChance.text = $"{vm.Attack?.CriticalChance * 100 ?? 0:F0}%";
                    RoundsToUnlock.text = $"{vm.Attack?.TurnsToActivate ?? 0}";
                    Damage.text = $"{vm.Attack?.Damage ?? 0}";
                    break;
                }
            case UpdateType.BonusCriticalChance:
            case UpdateType.BonusEvasion:
            case UpdateType.BonusAccuracy:
                UpdateValue.text = $"{vm.UpdateValue * 100:F0}%";
                break;
            case UpdateType.BonusHealth:
            case UpdateType.BonusAttackDamage:
                UpdateValue.text = Convert.ToInt32(vm.UpdateValue).ToString();
                break;
            default:
                break;
        }
    }
}
