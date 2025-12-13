using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class SkillManager : MonoBehaviour
{
    [Header("Skill Button Effects")]
    public GameObject stunEffect;
    public GameObject slowEffect;
    public GameObject healEffect;
    public GameObject cloneEffect;

    [Header("Stun Skill")]
    public float baseStunDuration = 1f;
    public float currentStunValue = 0f;
    public float maxStunValue = 5f;
    public GameObject shockWave;
    public GameObject stunedTXT;

    [Header("Slow Skill")]
    public float baseSlowDuration = 1f;
    public float newSlowSpeed = 0.5f;
    public float currentSlowValue = 0f;
    public float maxSlowValue = 5f;
    public GameObject slowedTXT;

    [Header("Heal Skill")]
    public int baseHealAmount = 10;
    public int currentHealValue = 0;
    public int maxHealValue = 20;

    [Header("Skill Button")]
    public Button stunButton;
    public Button slowButton;
    public Button healButton;
    public Button cloneButton;

    public static SkillManager instance;
    private PlayerController player;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        player = PlayerController.instance;
        UpdateSkillButtons(0, player.MaxBar);
        if (shockWave != null || stunedTXT != null || slowedTXT != null)
        {
            slowedTXT.SetActive(false); slowedTXT.SetActive(false);
            stunedTXT.SetActive(false);
            shockWave.SetActive(false);
        }
        
    }

    #region Stun Skill
    public void NewStunValue(float addedDuration)
    {
        currentStunValue += addedDuration;
        currentStunValue = Mathf.Clamp(currentStunValue, 0f, maxStunValue);
    }

    public void ActivateStunField()
    {
        Shockwave();
        StartCoroutine(StunAllEnemies());
        StartCoroutine(StunedText());
        ResetBar();
    }

    void Shockwave()
    {
        shockWave.transform.position = player.transform.position;
        shockWave.SetActive(true);
        StartCoroutine(HideShockwave());
    }
    private IEnumerator StunedText()
    {
        stunedTXT.SetActive(true);
        yield return new WaitForSeconds(2f);
        stunedTXT.SetActive(false);
    }

    IEnumerator HideShockwave()
    {
        yield return new WaitForSeconds(1f);
        shockWave.SetActive(false);
    }

    IEnumerator StunAllEnemies()
    {
        float duration = currentStunValue > 0 ? currentStunValue : baseStunDuration;

        IStunnable[] enemies = FindObjectsOfType<MonoBehaviour>(false)
            .OfType<IStunnable>()
            .ToArray();

        foreach (var enemy in enemies)
            enemy.Stun(duration);

        yield return null;
    }
    #endregion

    #region Slow Skill
    public void NewSlowValue(float addedDuration)
    {
        currentSlowValue += addedDuration;
        currentSlowValue = Mathf.Clamp(currentSlowValue, 0f, maxSlowValue);
    }

    public void ActivateSlowField()
    {
        StartCoroutine(SlowAllEnemies());
        StartCoroutine(SlowedText());
        ResetBar();
    }

    IEnumerator SlowAllEnemies()
    {
        float duration = currentSlowValue > 0 ? currentSlowValue : baseSlowDuration;

        ISlowable[] enemies = FindObjectsOfType<MonoBehaviour>(false)
            .OfType<ISlowable>()
            .ToArray();

        foreach (var enemy in enemies)
            enemy.SlowEffect(newSlowSpeed, duration);

        yield return null;
    }
    private IEnumerator SlowedText()
    {
        slowedTXT.SetActive(true);
        yield return new WaitForSeconds(2f);
        slowedTXT.SetActive(false);
    }
    #endregion

    #region Heal Skill
    public void NewHealValue(int newHealValue)
    {
        currentHealValue += newHealValue;
        currentHealValue = Mathf.Clamp(currentHealValue, 0, maxHealValue);
    }

    public void HealPlayer()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            int bonusHeal = currentHealValue + baseHealAmount;
            player.Heal(bonusHeal);
        }
        else
        {
            Debug.LogWarning("No PlayerController found!");
        }

        ResetBar();
    }
    #endregion

    #region Update Skill BTN

    public void UpdateSkillButtons(float currentBar, float maxBar)
    {
        bool isFull = currentBar >= maxBar;

        if (stunButton != null) stunButton.interactable = isFull;
        if (slowButton != null) slowButton.interactable = isFull;
        if (healButton != null) healButton.interactable = isFull;
        if (cloneButton != null) cloneButton.interactable = isFull;

        if (stunEffect != null) stunEffect.SetActive(isFull);
        if (slowEffect != null) slowEffect.SetActive(isFull);
        if (healEffect != null) healEffect.SetActive(isFull);
        if (cloneEffect != null) cloneEffect.SetActive(isFull);
    }

    public void ResetBar()
    {
        if (player != null)
        {
            player.currentBar = 0;
            player.UpdateUIBar();
            UpdateSkillButtons(0, player.MaxBar);
        }
    }
    #endregion
}
