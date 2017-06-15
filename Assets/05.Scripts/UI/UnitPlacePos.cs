using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPlacePos : MonoBehaviour {

    private UnitPlace myUnitPlace;
    public Color myColor;
    public bool isActive = false;

    public GameObject myUnit = null;

    private void Awake()
    {
        myUnitPlace = transform.GetComponentInParent<UnitPlace>();
        myColor = GetComponent<Image>().color;
    }

    public void UserClick()
    {
        if(myUnitPlace.activeUnitPos != null)
        {
            myUnitPlace.activeUnitPos.GetComponent<Image>().color = myColor;
        }

        myUnitPlace.activeUnitPos = this;
        myUnitPlace.activeUnitPos.GetComponent<Image>().color = new Color(0.2f, 1.0f, 0.6f, 0.6f);

        if(myUnit != null)
        {
            UIManager.Instance.OpenSellBtn(myUnit.GetComponent<Unit>().unitPrice/2);
            UIManager.Instance.OpenUpgradeBtn(myUnit.GetComponent<Unit>().upgrade.Price);
            UIManager.Instance.OpenStatPanel(myUnit.GetComponent<Unit>().GetStat());
        }
        else
        {
            UIManager.Instance.CloseSellBtn();
            UIManager.Instance.CloseUpgradeBtn();
        }

        UIManager.Instance.GoToUnitMenu();
    }
}
