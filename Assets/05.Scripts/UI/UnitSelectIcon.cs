using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelectIcon : MonoBehaviour {

    public GameObject unitPrefab;

    [SerializeField]
    private Text priceTxt;

    private int price;

    private void Awake()
    {
        price = unitPrefab.GetComponent<Unit>().unitPrice;
    }

    private void Start()
    {
        priceTxt.text = price.ToString();
    }

    public void CreateUnit()
    {
        GameManager.Instance.BuyUnit(unitPrefab, price);
    }
}
