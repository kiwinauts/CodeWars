using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider PlayerHealth;
    
    public Slider EnemyHealth;

    public Text EnemyLevel;

    public void UpdateCharacterHealth(float health, bool isPlayer)
    {
        if (isPlayer)
        {
            PlayerHealth.value = health;
            return;
        }

        EnemyHealth.value = health;
    }

    public void UpdateEnemyLevel(int level)
    {
        EnemyLevel.text = $"Level {level}";
    }
}
