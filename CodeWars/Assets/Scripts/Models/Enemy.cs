using UnityEngine;

public class Enemy : CharacterStats
{
    public int Level = 1;

    public Attack Attack;

    public UpgradeMinMax<float> CriticalChanceUpgradePerLevel;

    public UpgradeMinMax<float> EvasionUpgradePerLevel;

    public UpgradeMinMax<float> AccuracyUpgradePerLevel;

    public UpgradeMinMax<int> AttackDamageUpgradePerLevel;
    
    public UpgradeMinMax<int> HealthUpgradePerLevel;

    public float[] MaterialColor;

    public void Awake()
    {
        const string hueVariable = "_HueShift";
        var renderer = GetComponentInChildren<Renderer>();
        renderer?.material?.SetFloat(hueVariable, MaterialColor[Random.Range(0, MaterialColor.Length)]);
    }

    public void SetupLevel(int level)
    {
        Level = level;

        for (int i = 1; i < level; i++)
        {
            CriticalChance += Random.Range(CriticalChanceUpgradePerLevel.Minimum, CriticalChanceUpgradePerLevel.Maximum);
            Evasion += Random.Range(EvasionUpgradePerLevel.Minimum, EvasionUpgradePerLevel.Maximum);
            Accuracy += Random.Range(AccuracyUpgradePerLevel.Minimum, AccuracyUpgradePerLevel.Maximum);
            DamageBonus += Random.Range(AttackDamageUpgradePerLevel.Minimum, AttackDamageUpgradePerLevel.Maximum);
            MaxHealth += Random.Range(HealthUpgradePerLevel.Minimum, HealthUpgradePerLevel.Maximum);
        }

        CriticalChance = Mathf.Min(CriticalChance, CriticalChanceUpgradePerLevel.Cap);
        Evasion = Mathf.Min(Evasion, EvasionUpgradePerLevel.Cap);
        Accuracy = Mathf.Min(Accuracy, AccuracyUpgradePerLevel.Cap);

        CurrentHealth = MaxHealth;
    }
}
