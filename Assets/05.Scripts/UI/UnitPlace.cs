using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPlace : MonoBehaviour {

    public UnitPlacePos activeUnitPos;

    public void Init()
    {
        activeUnitPos.GetComponent<Image>().color = activeUnitPos.myColor;
    }
}
