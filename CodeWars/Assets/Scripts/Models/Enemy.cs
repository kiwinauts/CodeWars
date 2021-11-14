using UnityEngine;

public class Enemy : CharacterStats
{
    public int Level = 1;

    public Attack Attack;

    public void SetupLevel(int level)
    {
        Level = level;

        //Change Attack stats
    }
}
