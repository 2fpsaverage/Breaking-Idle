using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public TextMeshProUGUI methText;
    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI prestigeButtonText;
    public TextMeshProUGUI upgradesBoughtText;
    public GameObject prestigeButton;
    public GameObject upgradeButton;
    public GameObject winText;
    public GameObject restartButton;
    private int upgradesBought = 0;
    public float meth = 0.0f;
    private float methIncrease = 0.0f;
    private long upgradeCost = 0;
    private float dMethIncrease = 2.0f;
    private int prestiges = 0;
    public int prestigeCost = 100000;
    private bool winState = false;
    public AudioSource audioSource;

    void Start()
    {
        DataPersistenceManager.instance.LoadGame();
        audioSource = GetComponent<AudioSource>();

        if (PlayerPrefs.GetInt("showPrestige") == 1)
        {
            prestigeButton.SetActive(true);
        }

        //InvokeRepeating("Cook", 0, 1.0f);
        InvokeRepeating("Save", 0, 30.0f);
    }

    void Update()
    {
        if(meth < 1000000)
        {
            methText.text = ("Meth: " + Mathf.Round(meth));
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

        if(meth >= 3.402823e+38)
        {
            winText.SetActive(true);
            restartButton.SetActive(true);
            prestigeButton.SetActive(false);
            upgradeButton.SetActive(false);
            winState = true;
        }
        
        upgradesBoughtText.text = ("Cooking: " + upgradesBought);

        if(prestigeCost < 1000000)
        {
            prestigeButtonText.text = ("Prestige\n(available at " + prestigeCost + " meth)");
        }
        else
        {
            prestigeButtonText.text = ("Prestige\n(available at " + string.Format("{0:#.##e0}", prestigeCost) + " meth)");
            
        }

        if(meth >= 100000 && !winState)
        {
            PlayerPrefs.SetInt("showPrestige", 1);
            prestigeButton.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        Cook();
    }

    private void Cook()
    {
        meth = meth + methIncrease/60;
    }
    //Saves Data
    private void Save()
    {
        DataPersistenceManager.instance.SaveGame();
    }
    //Loads Data
    public void LoadData(SaveData data)
    {
        this.meth = data.meth;
        this.methIncrease = data.methIncrease;
        this.dMethIncrease = data.dMethIncrease;
        this.prestiges = data.prestiges;
        this.prestigeCost = data.prestigeCost;
        this.upgradesBought = data.upgradesBought;
        this.upgradeCost = data.upgradeCost;
    }

    public void SaveData(SaveData data)
    {
        if (data == null) 
        {
            Debug.LogWarning("Tried to save the game but game data was null. This may indicate an issue with the order in which SaveGame() is getting called");
            return;
        }

        data.meth = this.meth;
        data.methIncrease = this.methIncrease;
        data.dMethIncrease = this.dMethIncrease;
        data.prestiges = this.prestiges;
        data.prestigeCost = this.prestigeCost;
        data.upgradesBought = this.upgradesBought;
        data.upgradeCost = this.upgradeCost;
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
                if(upgradeCost < 2147483647)
                {
                    upgradeCost *= 5;
                }
                if(upgradeCost < 0)
                {
                    upgradeCost = 2147483647;
                }
            }

            if(methIncrease == 0)
            {
                methIncrease += 5;
            }
            else
            {
                methIncrease *= dMethIncrease;
                methIncrease *= 10;
                methIncrease = Mathf.Round(methIncrease);
                methIncrease /= 10;
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
            dMethIncrease *= 10;
            dMethIncrease = Mathf.Round(dMethIncrease);
            dMethIncrease /= 10;
            
            prestigeCost = 100000 + (50000 * prestiges);
        }
    }

    public void Restart()
    {
        winText.SetActive(false);
        restartButton.SetActive(false);
        PlayerPrefs.SetInt("showPrestige", 0);
        prestigeButton.SetActive(false);
        upgradesBought = 0;
        methIncrease = 0.0f;
        meth = 0.0f;
        upgradeCost = 0;
        dMethIncrease = 2.0f;
        prestiges = 0;
        prestigeCost = 100000;
        winState = false;
        DataPersistenceManager.instance.NewGame();
        Debug.Log("New Game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        Save();
        Application.Quit();
        Debug.Log("quitting...");
    }

    void OnDisable()
    {
        Save();
    }
}
