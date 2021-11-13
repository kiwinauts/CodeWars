using UnityEngine;

[System.Serializable]
public class Enemy : CharacterStats
{
    public int Level = 1;

    public GameObject EnemyObj;

    public void IncreaseLevel()
    {
        Debug.Log("Enemy increase level");
        Level++;
    }

    public void RespawnEnemy(GameObject Enemy)
    {
        EnemyObj = Enemy;
        Animator = Enemy.GetComponent<Animator>();
        CurrentHealth = MaxHealth;
    }
}
