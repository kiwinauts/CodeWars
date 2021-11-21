using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TweenerCore<Vector3, Vector3, VectorOptions> _scaleAnimation;

    private void Start()
    {
        _scaleAnimation = transform.DOScale(1.1f, 1f).Pause().SetAutoKill(false).SetEase(Ease.InOutElastic);
    }



    private void OnDestroy()
    {
        _scaleAnimation.Kill();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _scaleAnimation.PlayBackwards();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _scaleAnimation.PlayForward();
    }
}
