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

    [Header("Player HP")]
    public float MaxHP = 200f;
    public float currentHP;

    [Header("Player EXP")]
    public float MaxExp = 200f;
    public float currentEXP = 0f;
    public Slider expSlider;
    public Image expFill;

    [Header("Skill Bar System")]
    public float MaxBar = 200f;
    public float currentBar = 0f;
    public Slider barSlider;
    public Image barFill;
    public Gradient barGradient;

    [Header("HP Bar")]
    public Slider hpSlider;
    public Image hpFill;
    public Gradient hpGradient;

    private Coroutine barCoroutine;

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

        UpdateUIHealth();
        UpdateUIExp();
        UpdateUIBar();

        barCoroutine = StartCoroutine(DecreaseBarOverTime());
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

    #region EXP & Bar Logic

    public void TakeExp(float exp)
    {
        currentEXP += exp;
        currentEXP = Mathf.Clamp(currentEXP, 0, MaxExp);
        UpdateUIExp();
    }

    public void TakeBar(float amount)
    {
        currentBar += amount;
        currentBar = Mathf.Clamp(currentBar, 0, MaxBar);
        UpdateUIBar();
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, MaxHP);
        Debug.LogError("Current HP: " + currentHP);
        UpdateUIHealth();

        if (currentHP <= 0)
        {
            //put the game Over here
            Destroy(gameObject);
        }
    }

    //---------------- FOR HEAL SKILL -------------//
    public void Heal(int amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, MaxHP);
        UpdateUIHealth();
    }

    #endregion

    #region For UI
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

    IEnumerator DecreaseBarOverTime()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            if (currentBar > 0)
                TakeBar(-1); // decrease the bar
            yield return new WaitForSeconds(1f);
        }
    }

    #endregion
}
