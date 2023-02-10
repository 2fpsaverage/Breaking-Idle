using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float meth;
    public float methIncrease;
    public float dMethIncrease;
    public int prestiges;
    public int prestigeCost;
    public int upgradesBought;
    public long upgradeCost;

    public SaveData()
    {
        this.meth = 0.0f;
        this.methIncrease = 0.0f;
        this.dMethIncrease = 2.0f;
        this.prestiges = 0;
        this.prestigeCost = 100000;
        this.upgradesBought = 0;
        this.upgradeCost = 0;
    }
}
