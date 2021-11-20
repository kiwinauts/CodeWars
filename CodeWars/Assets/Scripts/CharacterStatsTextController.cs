using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatsTextController : MonoBehaviour
{
    public Text Evasion;
    public Text Accuracy;
    public Text Damage;
    public Text CriticalChance;
    
    public void UpdateStats(StatsUI stats)
    {
        Evasion.text = $"{stats.Evasion*100:F0}%";
        Accuracy.text = $"{stats.Accuracy*100:F0}%";
        Damage.text = stats.Damage.ToString();
        CriticalChance.text = $"{(stats.CriticalChance - 1f)*100:F0}%";
    }
}
