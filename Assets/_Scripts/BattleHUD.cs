using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    //public TextMeshProUGUI levelText;
    public HealthBar hpSlider;



    public void SetHUD(Enemy enemy)
    {
        nameText.text = enemy.enemyName;
        //levelText.text = "lvl " + enemy.level;
        hpSlider.SetMaxHealth(enemy.maxHP);
        hpSlider.SetHealth(enemy.currentHP);


    }
}
