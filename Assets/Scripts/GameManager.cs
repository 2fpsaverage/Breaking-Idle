using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI methText;
    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI prestigeButtonText;
    public TextMeshProUGUI upgradesBoughtText;
    public GameObject prestigeButton;
    private int upgradesBought = 0;
    public float meth = 0.0f;
    private float methIncrease = 0.0f;
    private int upgradeCost = 0;
    private float dMethIncrease = 2.0f;
    private int prestiges = 0;
    public int prestigeCost = 100000;

    void Start()
    {
        if (PlayerPrefs.HasKey("meth"))
        {
            meth = PlayerPrefs.GetFloat("meth");
        }
        if (PlayerPrefs.HasKey("methIncrease"))
        {
            methIncrease = PlayerPrefs.GetFloat("methIncrease");
        }
        if (PlayerPrefs.HasKey("dMethIncrease"))
        {
            dMethIncrease = PlayerPrefs.GetFloat("dMethIncrease");
        }
        if (PlayerPrefs.HasKey("prestiges"))
        {
            prestiges = PlayerPrefs.GetInt("prestiges");
        }
        if (PlayerPrefs.HasKey("prestigeCost"))
        {
            prestigeCost = PlayerPrefs.GetInt("prestigeCost");
        }
        if (PlayerPrefs.HasKey("upgradesBought"))
        {
            upgradesBought = PlayerPrefs.GetInt("upgradesBought");
        }
        if (PlayerPrefs.HasKey("upgradeCost"))
        {
            upgradeCost = PlayerPrefs.GetInt("upgradeCost");
        }
        if (PlayerPrefs.GetInt("showPrestige") == 1)
        {
            prestigeButton.SetActive(true);
        }

        InvokeRepeating("Cook", 0, 1.0f);
    }

    void Update()
    {
        if(meth < 1000000)
        {
            methText.text = ("Meth: " + meth);
        }
        else
        {
            methText.text = ("Meth: " + string.Format("{0:#.##e0}", meth));
        }

        if(upgradeCost < 1000000)
        {
            upgradeText.text = ("We need to cook!\n(increases meth production by " + dMethIncrease + "x)\n(cost: " + upgradeCost + " meth)");
        }
        else
        {
            upgradeText.text = ("We need to cook!\n(increases meth production by " + dMethIncrease + "x)\n(cost: " + string.Format("{0:#.##e0}", upgradeCost) + " meth)");
        }
        
        upgradesBoughtText.text = ("Cooking: " + upgradesBought);

        prestigeButtonText.text = ("Prestige\n(available at " + prestigeCost + " meth)");

        if(meth >= 100000)
        {
            PlayerPrefs.SetInt("showPrestige", 1);
            prestigeButton.SetActive(true);
        }
    }

    private void Cook()
    {
        meth = meth + methIncrease;
        meth = Mathf.Round(meth);
    }

    public void WeNeedToCook()
    {
        if(upgradeCost <= meth)
        {
            meth -= upgradeCost;

            upgradesBought += 1;

            if(upgradeCost == 0)
            {
                upgradeCost += 10;
            }
            else
            {
                upgradeCost *= 5;
            }

            if(methIncrease == 0)
            {
                methIncrease += 5;
            }
            else
            {
                methIncrease *= dMethIncrease;
                methIncrease = Mathf.Round(methIncrease);
            }
        }
    }

    public void Prestige()
    {
        if(meth >= prestigeCost)
        {
            methIncrease = 0;
            meth = 0;
            upgradesBought = 0;
            upgradeCost = 0;
        
            prestiges += 1;
            dMethIncrease += 0.1f;
            prestigeCost = 100000 + (50000 * prestiges);
        }
    }

    void OnDisable()
    {
        PlayerPrefs.SetFloat("meth", meth);
        PlayerPrefs.SetFloat("methIncrease", methIncrease);
        PlayerPrefs.SetFloat("dMethIncrease", dMethIncrease);
        PlayerPrefs.SetInt("prestiges", prestiges);
        PlayerPrefs.SetInt("prestigeCost", prestigeCost);
        PlayerPrefs.SetInt("upgradesBought", upgradesBought);
        PlayerPrefs.SetInt("upgradeCost", upgradeCost);
    }
}
