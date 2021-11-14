using UnityEngine;

[System.Serializable]
public class Enemy : CharacterStats
{
    public int Level = 1;

    public GameObject EnemyObj;

    public Attack Attack;

    public void IncreaseLevel()
    {
        Debug.Log("Enemy increase level");
        Level++;
    }

    public void RespawnEnemy(GameObject enemy)
    {
        if (enemy == null)
        {
            return;
        }

        EnemyObj = enemy;
        Animator = enemy.GetComponent<Animator>();
        CurrentHealth = MaxHealth;
    }
}
