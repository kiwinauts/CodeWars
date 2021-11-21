using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource AudioSource;

    public AudioClip UpdateClip;

    public AudioClip ChoseAttackClip;

    public AudioClip MissAttackClip;

    public AudioClip CriticalClip;

    public AudioClip NormalAttackClip;

    public AudioClip AvoidAttackClip;

    public AudioClip DeathClip;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;
    }

    public void ChoseUpdate()
    {
        PlayAudio(UpdateClip);
    }

    public void MissAttack()
    {
        PlayAudio(MissAttackClip);
    }

    public void ChoseAttack()
    {
        PlayAudio(ChoseAttackClip);
    }

    public void CriticalHit()
    {
        PlayAudio(CriticalClip);
    }

    public void NormalAttack()
    {
        PlayAudio(NormalAttackClip);
    }

    public void AvoidAttack()
    {
        PlayAudio(AvoidAttackClip);
    }

    public void Death()
    {
        PlayAudio(DeathClip);
    }

    private void PlayAudio(AudioClip clip)
    {
        if (AudioSource == null || AudioSource.isPlaying || clip == null)
        {
            return;
        }

        AudioSource.clip = clip;
        AudioSource.Play();
    }
}
