using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    private Rigidbody rb;

    private PlayerInput playerInput;
    private InputAction moveAction;

    [Header("Mobile Buttons")]
    public MobileArrowButton leftButton;
    public MobileArrowButton rightButton;

    [Header("Boundary Visualization")]
    public bool showBoundaries = true;
    public Color boundaryColor = Color.red;

    [Header("Player HP")]
    public float MaxHP = 100f;
    public float currentHP;

    [Header("Player EXP")]
    public float MaxExp = 100f;
    public float currentEXP = 0f;
    public Slider expSlider;
    public Image expFill;

    [Header("Skill Bar System")]
    public float MaxBar = 100f;
    public float currentBar = 0f;
    public Slider barSlider;
    public Image barFill;
    public Gradient barGradient;

    [Header("HP Bar")]
    public Slider hpSlider;
    public Image hpFill;
    public Gradient hpGradient;

    [Header("Skill Unlock System")]
    public Button slowButton;
    public Button stunButton;
    public Button healButton;
    public Button cloneButton;

    private List<Button> skillButtons = new List<Button>();
    private Button unlockedSkill = null;

    private Coroutine barDecreaseCoroutine;

    public GameObject skillUpgradePanel;

    [Header("Skill System")]
    public int slowLevel = 0;
    public int stunLevel = 0;
    public int cloneLevel = 0;
    public int healLevel = 0;

    public float hookRange = 6f;
    public float hookSpeed = 20f;
    private bool isSkillReady = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        // Cache PlayerInput & get action
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["movement"]; // name must match InputAction
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        currentHP = MaxHP;
        currentEXP = 0f;
        currentBar = 0f;

        //Initialize skill buttons list
        skillButtons.Clear();
        skillButtons.Add(slowButton);
        skillButtons.Add(stunButton);
        skillButtons.Add(healButton);
        skillButtons.Add(cloneButton);
        LockAllSkills();

        barDecreaseCoroutine = StartCoroutine(DecreaseBarOverTime());

        UpdateUIHealth();
        UpdateUIExp();
        UpdateUIBar();
    }
    private void Update()
    {
        float move = 0f;

        // New Input System movement
        move = moveAction.ReadValue<float>();

        // Override with mobile buttons
        if (leftButton != null && leftButton.isPressed) move = -1;
        if (rightButton != null && rightButton.isPressed) move = 1;

        Vector3 velocity = new Vector3(move * moveSpeed, rb.velocity.y, rb.velocity.z);
        rb.velocity = velocity;
    }

    #region Take Damage & Heal

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, MaxHP);
        UpdateUIHealth();

        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, MaxHP);
        UpdateUIHealth();
    }

    #endregion

    #region EXP & Bar Logic

    public void TakeExp(int exp)
    {
        currentEXP += exp;
        currentEXP = Mathf.Clamp(currentEXP, 0, MaxExp);
        UpdateUIExp();
    }

    public void TakeBar(int amount)
    {
        currentBar += amount;
        currentBar = Mathf.Clamp(currentBar, 0, MaxBar);
        UpdateUIBar();

        if (currentBar >= MaxBar && !isSkillReady && unlockedSkill == null)
        {
            isSkillReady = true;
            UnlockRandomSkill();
        }
    }

    #endregion

    #region UI Updates
    public void LockAllSkills()
    {
        foreach (Button btn in skillButtons)
        {
            if (btn != null)
                btn.interactable = false;
        }
        unlockedSkill = null;
    }
    public void UnlockRandomSkill()
    {
        List<Button> validButtons = new List<Button>();
        foreach (Button btn in skillButtons)
        {
            if (btn != null)
                validButtons.Add(btn);
        }

        if (validButtons.Count > 0)
        {
            int rand = Random.Range(0, validButtons.Count);
            unlockedSkill = validButtons[rand];
            unlockedSkill.interactable = true;

            unlockedSkill.onClick.RemoveAllListeners();
            unlockedSkill.onClick.AddListener(OnSkillUsed);
        }
    }
    public void OnSkillUsed()
    {
        if (unlockedSkill == slowButton) ApplySlowEffect();
        else if (unlockedSkill == stunButton) ApplyStunEffect();
        else if (unlockedSkill == healButton) ApplyHealEffect();
        else if (unlockedSkill == cloneButton) ApplyCloneEffect();

        LockAllSkills();
        currentBar = 0;
        isSkillReady = false;
        UpdateUIBar();
    }

    #endregion

    #region Skill Effects
    private void ApplySlowEffect() { }
    private void ApplyStunEffect() { }
    private void ApplyHealEffect() { Heal(20 + (healLevel * 5)); }
    private void ApplyCloneEffect() { }

    public void UpdateUIHealth()
    {
        if (hpSlider)
        {
            hpSlider.maxValue = MaxHP;
            hpSlider.value = currentHP;
            hpFill.color = hpGradient.Evaluate(hpSlider.normalizedValue);
        }
    }

    public void UpdateUIExp()
    {
        if (expSlider)
        {
            expSlider.maxValue = MaxExp;
            expSlider.value = currentEXP;
        }
    }

    public void UpdateUIBar()
    {
        if (barSlider)
        {
            barSlider.maxValue = MaxBar;
            barSlider.value = currentBar;
            barFill.color = barGradient.Evaluate(barSlider.normalizedValue);
        }
    }
    #endregion

    #region Misc Logic

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack"))
        {
            EnemyBee enemy = other.GetComponent<EnemyBee>();
            if (enemy != null && !enemy.isHooked)
            {
                TakeDamage(10);
            }
        }
    }

    IEnumerator DecreaseBarOverTime()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);
        while (true)
        {
            yield return wait;
            TakeBar(-1);
        }
    }

    private void OnDestroy()
    {
        if (barDecreaseCoroutine != null)
            StopCoroutine(barDecreaseCoroutine);
    }

    #endregion

    #region Upgrade System
    void ShowUpgradePanel()
    {
        if (skillUpgradePanel != null)
            skillUpgradePanel.SetActive(true);
    }
    #endregion
}
