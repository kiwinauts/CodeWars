using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpdateTextController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text Name;

    public Text UpdateValue;

    public Text RoundsToUnlock;

    public Text CriticalChance;

    public Text Damage;

    public GameObject[] NewAttackIcons;

    public Image UpdateValueIcon;

    private RectTransform _rectTransform;
    private TweenerCore<Vector3, Vector3, VectorOptions> _moveSequence;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _moveSequence = _rectTransform.DOScaleY(1.1f, 1).Pause().SetEase(Ease.InOutElastic).SetAutoKill(false);
    }

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_rectTransform != null)
        {
            _moveSequence.PlayForward();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_rectTransform != null)
        {
            _moveSequence.PlayBackwards();
        }
    }
}
