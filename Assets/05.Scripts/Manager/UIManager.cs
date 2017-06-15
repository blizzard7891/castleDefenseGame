using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingleTon<UIManager>
{

    public GameObject unitMenu;
    public GameObject settingMenu;
    public GameObject gameOverPanel;
    public GameObject levelUI;
    public GameObject inGameUI;

    public GameObject unitPlace;
    public GameObject buttonPanel;
    public GameObject sellBtn;
    public Text sellPrice;
    public GameObject unitUpgradeBtn;
    public Text unitUpgradePrice;
    public GameObject unitStatPanel;
    public Text unitStatTxt;
    public GameObject exitPanel;

    public void GoToLevel()
    {
        CloseAllUI();
        levelUI.SetActive(true);
    }

    public void GoToUnitMenu()
    {
        unitMenu.SetActive(true);
        buttonPanel.SetActive(false);
    }

    public void CloseUnitMenu()
    {
        CloseSellBtn();
        CloseUpgradeBtn();

        unitMenu.SetActive(false);
        buttonPanel.SetActive(true);
        unitPlace.GetComponent<UnitPlace>().Init();
    }

    public void CloseAllUI()
    {
        unitMenu.SetActive(false);
        levelUI.SetActive(false);
        inGameUI.SetActive(false);
    }

    public void PauseGame(bool pause)
    {
        Time.timeScale = pause ? 0f : 1f;
    }

    public void OpenSettingMenu()
    {
        unitPlace.SetActive(false);
        buttonPanel.SetActive(false);
        settingMenu.SetActive(true);
    }

    public void CloseSettingMenu()
    {
        unitPlace.SetActive(true);
        buttonPanel.SetActive(true);
        settingMenu.SetActive(false);
    }

    public void OpenGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void CloseGameOverPanel()
    {
        gameOverPanel.SetActive(false);
    }

    public void GoToinGame()
    {
        CloseAllUI();
        inGameUI.SetActive(true);
    }

    public void OpenSellBtn(int price)
    {
        sellBtn.SetActive(true);
        sellPrice.text = "<color=yellow>" + price.ToString() + " Gold</color>";
    }

    public void CloseSellBtn()
    {
        sellBtn.SetActive(false);
        CloseStatPanel();
    }

    public void OpenUpgradeBtn(int price)
    {
        unitUpgradeBtn.SetActive(true);
        unitUpgradePrice.text = "<color=yellow>" + price.ToString() + " Gold</color>";
    }

    public void CloseUpgradeBtn()
    {
        unitUpgradeBtn.SetActive(false);
    }

    public void OpenStatPanel(string stat)
    {
        unitStatPanel.SetActive(true);
        unitStatTxt.text = stat;
    }

    public void CloseStatPanel()
    {
        unitStatPanel.SetActive(false);
    }

    public void OpenExitPanel()
    {
        if (!exitPanel.activeSelf)
        {
            Time.timeScale = 0;
            exitPanel.SetActive(true);
        }
    }

    public void CloseExitPanel()
    {
        Time.timeScale = 1;
        exitPanel.SetActive(false);
    }
}