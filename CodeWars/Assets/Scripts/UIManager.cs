using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Health")]
    public Slider PlayerHealth;

    public Text PlayerHealthText;

    public Slider EnemyHealth;

    public Text EnemyHealthText;

    [Header("Updates")]
    public Button[] UpdateUI;

    [Header("Attacks")]
    public GameObject AttackCanvasParent;

    public GameObject AttackButton;

    public List<Button> CurrentUIAttacks;

    public Text AttackMessage;

    public Text AttackMessageDrop;

    public Vector2 AttackUIOffset;

    public float AttackMessageSeconds;

    public string[] MissMessages;

    public string[] CriticalMessages;

    public string[] EvadeMessages;

    [Header("Stats")]
    public Text EnemyLevel;

    public CharacterStatsTextController PlayerStats;
    
    public CharacterStatsTextController EnemyStats;

    public Text RoundNumber;

    private void Awake()
    {
        ClearCurrentAttacks();
    }

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;
        HideAllUpdates();
        HideAttackMessage();
    }
    
    public void MissMessage()
    {
        ShowAttackMessage(MissMessages[Random.Range(0, MissMessages.Length)]);
    }

    public void CriticalHitMessage()
    {
        ShowAttackMessage(CriticalMessages[Random.Range(0, CriticalMessages.Length)]);
    }

    public void EvadeMessage()
    {
        ShowAttackMessage(EvadeMessages[Random.Range(0, CriticalMessages.Length)]);
    }

    private void ShowAttackMessage(string message)
    {
        AttackMessage.text = message;
        AttackMessageDrop.text = message;
        Invoke("HideAttackMessage", AttackMessageSeconds);
    }

    private void HideAttackMessage()
    {
        AttackMessage.text = "";
        AttackMessageDrop.text = "";
    }

    public void UpdateRound(int round)
    {
        RoundNumber.text = $"Round {round}";
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

    public void UpdateCharacterHealth(float health, bool isPlayer, int currentHealth, int maxHealth)
    {
        currentHealth = currentHealth <= 0 ? 0 : currentHealth;

        if (isPlayer)
        {
            if (PlayerHealth.value != health && health != 1)
            {
                PlayerHealth.transform.DOShakeScale(1f, 0.3f);
            }

            PlayerHealth.value = health;
            PlayerHealthText.text = $"{currentHealth}/{maxHealth}";

            return;
        }

        if (EnemyHealth.value != health && health != 1)
        {
            EnemyHealth.transform.DOShakeScale(1f, 0.3f);
        }

        EnemyHealthText.text = $"{currentHealth}/{maxHealth}";
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
            var instantiated = Instantiate(AttackButton, AttackCanvasParent.transform);
            var rectTransform = instantiated.GetComponent<RectTransform>();

            rectTransform.anchoredPosition = new Vector2(CalculateXLocation(currentAttacks.Count(), currentAttackIndex), AttackUIOffset.y);

            var button = AddAttackListener(instantiated, currentAttack);

            var textController = button.GetComponent<AttackTextController>();

            if (textController != null)
            {
                textController.UpdateAttack(currentAttack.MapToAttackUI());
            }

            CurrentUIAttacks.Add(button);
            currentAttackIndex++;
        }
    }

    private float CalculateXLocation(int currentAttacksCount, int currentAttackIndex)
    {
        if (currentAttacksCount == 1)
        {
            return 0;
        }

        var initialX = (-1) * AttackUIOffset.x * (currentAttacksCount - 1);
        return System.Convert.ToSingle(initialX + currentAttackIndex * AttackUIOffset.x * 2);
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
        ClearCurrentAttacks();
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
            var text = currentUIButton.GetComponent<UpdateTextController>();
            if (text != null)
            {
                text.UpdateText(updateVm);
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

    public void ChangeCharacterStats(StatsUI stats, bool isPlayer)
    {
        if (isPlayer)
        {
            PlayerStats.UpdateStats(stats);
            return;
        }

        EnemyStats.UpdateStats(stats);
    }
}
