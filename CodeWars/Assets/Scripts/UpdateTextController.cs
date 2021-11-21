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

    public GameObject[] NewAttackIcons;

    public Image UpdateValueIcon;

    private void Reset()
    {
        UpdateValue.text = "";
        CriticalChance.text = "";
        RoundsToUnlock.text = "";
        Damage.text = "";
        UpdateValueIcon.gameObject.SetActive(false);
        ActiveIcons(false);
    }

    public void UpdateText(UpdateVm vm)
    {
        Reset();

        Name.text = vm.Name;
        UpdateValueIcon.sprite = vm.Image;

        switch (vm.updateType)
        {
            case UpdateType.NewAttackMove:
                {
                    Name.text = vm.Attack?.Name;
                    CriticalChance.text = $"{vm.Attack?.CriticalChance * 100 ?? 0:F0}%";
                    RoundsToUnlock.text = $"{vm.Attack?.TurnsToActivate ?? 0}";
                    Damage.text = $"{vm.Attack?.Damage ?? 0}";
                    ActiveIcons(true);
                    break;
                }
            case UpdateType.BonusCriticalChance:
            case UpdateType.BonusEvasion:
            case UpdateType.BonusAccuracy:
                UpdateValueIcon.gameObject.SetActive(true);
                UpdateValue.text = $"+{vm.UpdateValue * 100:F0}%";
                break;
            case UpdateType.BonusHealth:
            case UpdateType.BonusAttackDamage:
                UpdateValueIcon.gameObject.SetActive(true);
                UpdateValue.text = $"+{Convert.ToInt32(vm.UpdateValue)}";
                break;
            case UpdateType.FullHealth:
                UpdateValueIcon.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void ActiveIcons(bool active)
    {
        foreach (var icon in NewAttackIcons)
        {
            icon.SetActive(active);
        }
    }
}
