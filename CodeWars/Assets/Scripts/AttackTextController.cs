using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class AttackTextController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text Name;

    public Text CriticalChance;

    public Text Damage;

    public Text RemainingTurns;

    public Image RemainingTurnsImage;

    private Tweener _moveSequence;

    private void Awake()
    {
        var rectTransform = GetComponent<RectTransform>();
        _moveSequence = rectTransform.DOMoveY(150f, 1).SetRelative().Pause().SetEase(Ease.InOutElastic).SetAutoKill(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _moveSequence.PlayForward();
    }

    public void UpdateAttack(AttackUI attack)
    {
        Name.text = attack.Name;
        CriticalChance.text = $"{attack.CriticalChance * 100:F0}%";
        Damage.text = attack.Damage.ToString();
        RemainingTurns.text = attack.RemainingTurns == 0 ? "" : attack.RemainingTurns.ToString();
        RemainingTurnsImage.gameObject.SetActive(attack.RemainingTurns != 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _moveSequence.PlayBackwards();
    }
}
