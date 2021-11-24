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

    private RectTransform _rectTransform;
    private Tweener _moveSequence;
    private bool mouseOver;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _moveSequence = _rectTransform.DOMoveY(150f, 1).SetRelative().Pause().SetEase(Ease.InOutElastic).SetAutoKill(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        if (_rectTransform != null && !_moveSequence.IsPlaying())
        {
            _moveSequence.PlayForward();
        }
    }

    private void Update()
    {
        if (!mouseOver && _moveSequence != null && !_moveSequence.IsPlaying())
        {
            _moveSequence.PlayBackwards();
        }
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
        mouseOver = false;
        if (_rectTransform != null && !_moveSequence.IsPlaying())
        {
            _moveSequence.PlayBackwards();
        }
    }

    private void OnDestroy()
    {
        _rectTransform = null;
        _moveSequence.Kill();
    }
}
