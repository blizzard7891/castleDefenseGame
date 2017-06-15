using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    [SerializeField]
    private Stat health;
    [SerializeField]
    private int HP;
    [SerializeField]
    private Text healthTxt;
    [SerializeField]
    public CastleUpgrade upgrade;

    public int Level { get; protected set; }

    

    //public CastleUpgrade NextUpgrade
    //{
    //    get
    //    {
    //        if(Upgrades.Length > Level - 1)
    //        {
    //            return Upgrades[Level - 1];
    //        }
    //        return null;
    //    }
    //}

    private void Awake()
    { 
        health.Initialize();
        Level = 1;
    }

    // Use this for initialization
    void Start () {
        this.health.Bar.Reset();
        this.health.MaxVal = HP;
        this.health.CurrentVal = this.health.MaxVal;

        healthTxt.text = health.CurrentVal.ToString();

        upgrade = new CastleUpgrade(5, 10);

        //Upgrades = new CastleUpgrade[]
        //{
        //    new CastleUpgrade(5,10),
        //    new CastleUpgrade(10,15)
        //};
    }

    // Update is called once per frame
    void Update() {
      
    }

    public void TakeDamage(int damage)
    {
        health.CurrentVal -= damage;

        healthTxt.text = health.CurrentVal.ToString();

        if(health.CurrentVal <= 0)
        {
            healthTxt.text = "0";
            GameManager.Instance.GameOver();
        }
    }

    public void ResetHealth()
    {
        health.CurrentVal = health.MaxVal;
        healthTxt.text = health.CurrentVal.ToString();
    }

    public string GetStat()
    {
        if(upgrade == null)
        {
            return string.Format("\nLevel: {0}[MAX] \n", Level, health.CurrentVal);
        }

        return string.Format("\nLevel: {0} \n", Level);
    }

    public void Upgrade(int price)
    {
        GameManager.Instance.Currency -= upgrade.Price;

        health.Upgrade((int)health.MaxVal + upgrade.HP);
        Level++;

        ResetHealth();

        upgrade.Price += price;
        //upgrade.HP += 10;
    }
}