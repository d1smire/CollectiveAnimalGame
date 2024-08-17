using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterUI : MonoBehaviour
{
    [SerializeField] private Slider sliderHP;
    [SerializeField] private Slider sliderTurnMeter;

    [SerializeField] private Button firstSkill;
    [SerializeField] private Button secondSkill;
    [SerializeField] private Button thirdSkill;

    public int skillNumber = 0;

    private float _maxHealth;

    private void Start()
    {
        firstSkill = GameObject.Find("FirstSkill").GetComponent<Button>();
        secondSkill = GameObject.Find("SecondSkill").GetComponent<Button>();
        thirdSkill = GameObject.Find("ThirdSkill").GetComponent<Button>();

        if (firstSkill != null && secondSkill != null && thirdSkill != null)
        {
            firstSkill.onClick.AddListener(FirstSkill);
            secondSkill.onClick.AddListener(SecondSkill);
            thirdSkill.onClick.AddListener(ThirdSkill);
        }
    }

    private void OnDestroy()
    {
        if (firstSkill != null && secondSkill != null && thirdSkill != null)
        {
            firstSkill.onClick.RemoveAllListeners();
            secondSkill.onClick.RemoveAllListeners();
            thirdSkill.onClick.RemoveAllListeners();
        }
    }
    public void SetMaxHp(float Hp)
    {
        _maxHealth = Hp;
    }

    public void UpdateHealth(float currentHealth)
    {
        if (sliderHP != null)
        {
            sliderHP.value = currentHealth / _maxHealth;
        }
        else
        {
            Debug.LogError("Slider for HP is not assigned!");
        }
    }
    public void UpdateTurnMeter(float currentTurnMeter)
    {
        if (sliderHP != null)
        {
            sliderHP.value = currentTurnMeter / _maxHealth;
        }
        else
        {
            Debug.LogError("Slider for TurnMeter is not assigned!");
        }
    }
    public void FirstSkill()
    {
        skillNumber = 1;
        IsInteract(false);
    }

    public void SecondSkill()
    {
        skillNumber = 2;
        IsInteract(false);
    }

    public void ThirdSkill()
    {
        skillNumber = 3;
        IsInteract(false);
    }

    public void IsInteract(bool IsActive) 
    {
        firstSkill.interactable = IsActive;
        secondSkill.interactable = IsActive;
        thirdSkill.interactable = IsActive;
    }
}
