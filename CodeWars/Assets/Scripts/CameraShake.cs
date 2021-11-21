using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Vector3 ShakeDirection;

    public float ShakeDuration;

    public int VibrateStrength;

    [Range(0f,180f)]
    public float VibrateRandomness;

    public Ease EaseMode;

    private Camera _cameraComponent;
    
    private Tweener _shake;

    private void Awake()
    {
        _cameraComponent = GetComponent<Camera>();
    }

    private void Start()
    {
        CreateShake();
    }

    private void CreateShake()
    {
        _shake = _cameraComponent.DOShakePosition(ShakeDuration, ShakeDirection, VibrateStrength).SetEase(EaseMode).SetAutoKill(false);
    }

    private void Update()
    {
        if (!_shake.IsPlaying())
        {
            CreateShake();
        }
    }
}
