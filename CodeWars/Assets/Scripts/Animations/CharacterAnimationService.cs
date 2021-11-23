using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationService : MonoBehaviour
{
    public CharacterStats Character;

    public void AttackFinished()
    {
        Character.CharacterAttackedAnimationEvent();
    }


    public void DeathFinished()
    {
        Character.DestroyCharacterAnimationEvent();
    }
}
