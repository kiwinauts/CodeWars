using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Enemy : CharacterStats
{
    public int Level = 1;

    public GameObject EnemyObj;

    public void IncreaseLevel()
    {
        Debug.Log("Enemy increase level");
        Level++;
    }
}
