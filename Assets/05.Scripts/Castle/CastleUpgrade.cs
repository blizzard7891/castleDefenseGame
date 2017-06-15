using System;
using UnityEngine.UI;

[Serializable]
public class CastleUpgrade
{
    public int HP { get;  set; }
    public int Price { get; set; }
    
    public CastleUpgrade(int price, int hp)
    {
        this.Price = price;
        this.HP = hp;
    }
}
