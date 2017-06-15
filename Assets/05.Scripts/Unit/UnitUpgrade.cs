using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitUpgrade
{
    public int Damage { get; set; }
    public int Price { get; set; }

    public UnitUpgrade(int price, int Damage)
    {
        this.Price = price;
        this.Damage = Damage;
    }
}
