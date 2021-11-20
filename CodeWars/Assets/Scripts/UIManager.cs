using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Slider PlayerHealth;

    public Slider EnemyHealth;

    public Text EnemyLevel;

    public GameObject AttackCanvasParent;

    public GameObject AttackButton;

    public List<Button> CurrentUIAttacks;

    public Button[] UpdateUI;

    public void Awake()
    {
        ClearCurrentAttacks();
    }

    public void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
    }

    public void ClearCurrentAttacks()
    {
        foreach (var attackUI in CurrentUIAttacks)
        {
            Destroy(attackUI.gameObject);
        }

        CurrentUIAttacks.Clear();
    }

    public void DisableCurrentAttacks()
    {
        foreach (var attackUI in CurrentUIAttacks)
        {
            attackUI.interactable = false;
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

    public void CreateAttackButtons(IEnumerable<Attack> currentAttacks)
    {
        ClearCurrentAttacks();
        int currentAttackIndex = 0;

        foreach (var currentAttack in currentAttacks)
        {
            var instantiated = Instantiate(AttackButton);
            instantiated.transform.SetParent(AttackCanvasParent.transform);
            var rectTransform = instantiated.GetComponent<RectTransform>();

            rectTransform.anchoredPosition = new Vector2(CalculateXLocation(currentAttacks.Count(), currentAttackIndex), 100);

            var button = AddAttackListener(instantiated, currentAttack);
            CurrentUIAttacks.Add(button);
            currentAttackIndex++;
        }
    }

    private static float CalculateXLocation(int currentAttacksCount, int currentAttackIndex)
    {
        if (currentAttacksCount == 1)
        {
            return 0;
        }

        var initialX = (-1) * 62.5f * 2 * (currentAttacksCount - 1);
        return Convert.ToSingle(initialX + currentAttackIndex * 250f);
    }

    private Button AddAttackListener(GameObject instantiated, Attack attack)
    {
        var instantiatedButton = instantiated.GetComponent<Button>();
        instantiatedButton.onClick.AddListener(() => GameManager.Instance.PlayerAttack(attack));
        instantiatedButton.name = CurrentUIAttacks.Count.ToString();

        if (attack.RemainingTurns > 0)
        {
            instantiatedButton.interactable = false;
        }

        return instantiatedButton;
    }

    public void ShowUpdates(IEnumerable<UpdateVm> updates)
    {
        var updateIndex = 0;
        foreach (var updateVm in updates)
        {
            if (UpdateUI.Length < updateIndex)
            {
                break;
            }

            var currentUIButton = UpdateUI[updateIndex];

            currentUIButton.onClick.RemoveAllListeners();
            currentUIButton.onClick.AddListener(() => GameManager.Instance.ChosenUpdate(updateVm));
            var text = currentUIButton.GetComponentInChildren<Text>();
            if (text != null)
            {
                text.text = updateVm.updateType.ToString();
            }

            currentUIButton.gameObject.SetActive(true);
            updateIndex++;
        }
    }

    public void HideAllUpdates()
    {
        foreach (var currentUIButton in UpdateUI)
        {
            currentUIButton.gameObject.SetActive(false);
        }
    }
}
