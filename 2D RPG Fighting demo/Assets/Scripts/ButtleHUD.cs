using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtleHUD : MonoBehaviour
{
    public Text nameText, levelText;
    public Slider hpSlider;

    public void SetHUD(UnitC unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.unitLevel;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
    }

    public void TakeDamage(int damage)
    {
        hpSlider.value -= damage;
    }

    public void SetHP(int hp) {
        hpSlider.value = hp;
    }
}
