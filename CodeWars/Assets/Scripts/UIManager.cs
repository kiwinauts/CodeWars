using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider PlayerHealth;
    
    public Slider EnemyHealth;

    public Text EnemyLevel;

    public GameObject AttackCanvasParent;
    
    public GameObject AttackButton;

    public List<GameObject> CurrentUIAttacks;

    public void Start()
    {
        ClearCurrentAttacks();
    }

    public void ClearCurrentAttacks()
    {
        foreach (var attackUI in CurrentUIAttacks)
        {
            Destroy(attackUI);
        }

        CurrentUIAttacks.Clear();
    }

    public void DisableCurrentAttacks()
    {
        foreach (var attackUI in CurrentUIAttacks)
        {
            var buttonComponent = attackUI.GetComponent<Button>();
            if(buttonComponent != null)
            {
                buttonComponent.interactable = false;
            }
        }
    }

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

    public void CreateAttackButtons(List<Attack> currentAttacks)
    {
        ClearCurrentAttacks();
        foreach (var attackUi in currentAttacks)
        {
            var instantiated = Instantiate(AttackButton);
            instantiated.transform.SetParent(AttackCanvasParent.transform);
            var rectTransform = instantiated.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;

            AddAttackListener(instantiated);
            CurrentUIAttacks.Add(instantiated);
        }
    }

    private void AddAttackListener(GameObject instantiated)
    {
        var instantiatedButton = instantiated.GetComponent<Button>();
        instantiatedButton.onClick.AddListener(() => GameManager.instance.PlayerAttack(Convert.ToInt32(instantiatedButton.name)));
        instantiatedButton.name = CurrentUIAttacks.Count.ToString();
    }
}
