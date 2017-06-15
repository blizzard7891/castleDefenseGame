using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : SingleTon<GameManager>
{
    [SerializeField]
    private Text currencyTxt;
    [SerializeField]
    private Text upgradeTxt;
    [SerializeField]
    private Text unitUpgradePriceTxt;
    [SerializeField]
    private Text unitStatTxt;
    [SerializeField]
    private Castle myCastle;
    [SerializeField]
    private UnitPlace unitPlace;
    [SerializeField]
    private Text resultTxt;
    [SerializeField]
    private Text waveTxt;
    [SerializeField]
    private Button recordingBtn;
    [SerializeField]
    private Button soundBtn;
    [SerializeField]
    private Text questionTxt;
    [SerializeField]
    private Text wrongCountTxt;
    [SerializeField]
    private Text scoreTxt;
    [SerializeField]
    private GameObject lockImg;
    [SerializeField]
    private Button lockunitBtn;
    [SerializeField]
    private Text maxScoreTxt;
    [SerializeField]
    private Text gameScoreTxt;

    private Transform MonsterParent;
    private string question;
    private int currency;
    private int tmpCurrency;
    private int wave = 0;
    private int questionNum = 0;
    private int wrongCount = 0;
    private int maxWrongCount = 3;
    private bool correct;
    private int game1Score;
    private bool isSTTComplete = false;

    private List<Monster> activeMonsters = new List<Monster>();
    
    public ObjectPool Pool { get; set; }

    public int Currency
    {
        get
        {
            return currency;
        }

        set
        {
            currency = value;
            currencyTxt.text = value.ToString();
        }
    }

    public bool WaveActive
    {
        get { return activeMonsters.Count > 0; }
    }

    public int TmpCurrency
    {
        get
        {
            return tmpCurrency;
        }

        set
        {
            tmpCurrency = value;
        }
    }

    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
        MonsterParent = GameObject.Find("MonsterPool").GetComponent<Transform>();
        Physics2D.gravity = new Vector2(0, -30.0f);
    }

    // Use this for initialization
    void Start ()
    {
        Currency = 50;
        TmpCurrency = 0;
        correct = true;
        upgradeTxt.text = myCastle.upgrade.Price.ToString()+" $";
        unitUpgradePriceTxt.text = " $";
        wrongCountTxt.text = wrongCount + " / " + maxWrongCount;

        resultTxt.text = "Ready";

        Call_RequestScore();

        UIManager.Instance.GoToLevel();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.OpenExitPanel();
        }
    }

    public void StartWave()
    {
        wave++;

        StartCoroutine(SpawnWave());
        UIManager.Instance.GoToinGame();
        SoundManager.Instance.InGameBGM(true);
    }

    private IEnumerator SpawnWave()
    {
        waveTxt.text = "WAVE : " + wave.ToString();

        for (int i = 1; i <= wave+2; i++)
        {
            int monsterIndex = 0;

            if(i >= 6)
            {
                if (i % 8 == 0 && i > 8)
                    monsterIndex = 3;
                else if (i % 5 == 0)
                    monsterIndex = 2;
                else if (i % 3 == 0 || i == 8)
                    monsterIndex = 1;
            }

            string type = string.Empty;

            switch (monsterIndex)
            {
                case 0:
                    type = "Zombie";
                    break;
                case 1:
                    type = "Skeleton";
                    break;
                case 2:
                    type = "Dracula";
                    break;
                case 3:
                    type = "Golem";
                    break;
                default:
                    Debug.Log("Error in SpawnWave()");
                    break;
            }

            Monster monster = Pool.GetObject(type).GetComponent<Monster>();

            monster.GetComponent<Transform>().SetParent(MonsterParent);
            monster.Spawn();

            activeMonsters.Add(monster);

            if(i == wave+2 / 2)
            {
                yield return new WaitForSeconds(3f);
            }else
            {
                yield return new WaitForSeconds(0.4f);
            }
        }
        yield return new WaitForSeconds(1.5f);
    }

    public void RemoveMonster(Monster monster)
    {
        activeMonsters.Remove(monster);

        if (!WaveActive)
        {
            if (!recordingBtn.enabled && !isSTTComplete)
            {
                PluginManager.Instance.call_androidSTT();
                SoundManager.Instance.SoundMute(false);
            }

            resultTxt.text = "Ready";
            recordingBtn.enabled = true;
            soundBtn.enabled = true;

            myCastle.ResetHealth();
            UIManager.Instance.GoToLevel();

            SoundManager.Instance.InGameBGM(false);
        }
    }

    public void GameOver()
    {
        if (!recordingBtn.enabled && !isSTTComplete)
        {
            PluginManager.Instance.call_androidSTT();
        }

        gameScoreTxt.text = wave.ToString() + " wave";

        Call_SaveScore();
        StartCoroutine(WaitForSave());
    }

    public void UpgradeCastle()
    {
        if (myCastle.upgrade != null && Currency >= myCastle.upgrade.Price)
        {
            myCastle.Upgrade(5);
            upgradeTxt.text = myCastle.upgrade.Price.ToString() + " $";
        }
    }

    public void UpgradeUnit()
    {
        if (unitPlace.activeUnitPos != null)
        {
            Unit myUnit = unitPlace.activeUnitPos.myUnit.GetComponent<Unit>();

            if (Currency >= myUnit.upgrade.Price)
            {
                myUnit.Upgrade();
                unitUpgradePriceTxt.text = "<color=yellow>" + myUnit.upgrade.Price.ToString() + " Gold</color>";
                unitStatTxt.text = myUnit.GetStat();
            }
        }
    }

    public void BuyUnit(GameObject prefab, int price)
    {
        if (!unitPlace.activeUnitPos.isActive && Currency >= price)
        {
            GameObject castleUnit = Instantiate(prefab);
            castleUnit.transform.position = Camera.main.ScreenToWorldPoint(unitPlace.activeUnitPos.transform.position + new Vector3(0, 0, 10f));
            castleUnit.transform.rotation = Quaternion.identity;
            unitPlace.activeUnitPos.myUnit = castleUnit;

            Currency -= price;

            unitPlace.activeUnitPos.isActive = true;

            Unit myUnit = unitPlace.activeUnitPos.myUnit.GetComponent<Unit>();

            UIManager.Instance.OpenSellBtn(myUnit.unitPrice / 2);
            UIManager.Instance.OpenUpgradeBtn(myUnit.upgrade.Price);
            UIManager.Instance.OpenStatPanel(myUnit.GetStat());
            Debug.Log(myUnit.upgrade.Price);
        }
    }

    public void SellUnit()
    {
        if(unitPlace.activeUnitPos != null)
        {
            Currency += unitPlace.activeUnitPos.myUnit.GetComponent<Unit>().unitPrice / 2;

            Destroy(unitPlace.activeUnitPos.myUnit);
            unitPlace.activeUnitPos.myUnit = null;
            unitPlace.activeUnitPos.isActive = false;

            UIManager.Instance.CloseSellBtn();
            UIManager.Instance.CloseUpgradeBtn();
        }
    }

    void STTresult(string result)
    {
        char[] delimeter = { '/' };
        string[] words = result.Split(delimeter);

        isSTTComplete = true;
        SoundManager.Instance.SoundMute(false);

        if (resultTxt.IsActive() && words != null)
        {
            resultTxt.text =  words[0];
            StartCoroutine(InitResultTxt());
        }

        CheckAnswer(words);
    }

    void QuizResult(string Quiz)
    {
        question = Quiz;
        questionTxt.text = question;
    }

    void ScoreResult(string score)
    {
        game1Score = Int32.Parse(score);
        scoreTxt.text = game1Score.ToString();

        if (game1Score >= 2000)
        {
            UnlockUnit();
        }

        Call_RequestQuestion(correct);
    }

    void GetScore(string score)
    {
        maxScoreTxt.text = score + " wave";
    }

    public void Call_STTstart()
    {
        PluginManager.Instance.setcallbackSTTresult(new delegateSTTresult(STTresult));
        PluginManager.Instance.call_androidSTT();
        isSTTComplete = false;

        if (resultTxt.IsActive())
        {
            SoundManager.Instance.SoundMute(true);
            StartCoroutine(ConnectedRecording());
            recordingBtn.enabled = false;
            soundBtn.enabled = false;
        }
    }

    public void Call_TTSstart()
    {
        PluginManager.Instance.call_androidTTS(question);
    }

    public void Call_RequestQuestion(bool answerCheck)
    {
        questionNum = UnityEngine.Random.Range(0, 800);

        PluginManager.Instance.setcallbackSTTresult(new delegateSTTresult(QuizResult));
        PluginManager.Instance.call_androidQuestion(questionNum, answerCheck);
        //questionNum++;
        wrongCount = 0;
        wrongCountTxt.text = wrongCount + " / " + maxWrongCount;
    }

    public void Call_RequestScore()
    {
        PluginManager.Instance.setcallbackSTTresult(new delegateSTTresult(ScoreResult));
        PluginManager.Instance.call_androidScoreLoad();
    }

    public void Call_SaveScore()
    {
        PluginManager.Instance.call_saveGame2Score(wave);
    }

    public void Call_GetScore()
    {
        PluginManager.Instance.setcallbackSTTresult(new delegateSTTresult(GetScore));
        PluginManager.Instance.call_getGame2Score();
    }

    private void CheckAnswer(string[] result)
    {
        if (CompareAnswer(result))
        {
            correct = true;
            SoundManager.Instance.PlaySFX("correct");

            resultTxt.text = question + "/ correct!!!";

            Currency += TmpCurrency;
            TmpCurrency = 0;

            Call_RequestQuestion(correct);
        }
        else
        {
            wrongCount++;
            wrongCountTxt.text = wrongCount + " / " + maxWrongCount;
            SoundManager.Instance.PlaySFX("wrong");

            if (wrongCount == 3)
            {
                correct = false;
                Call_RequestQuestion(correct);
            }
        }
    }

    private bool CompareAnswer(string[] result)
    {
        foreach (string item in result)
        {
            int length = item.Length;

            try
            {
                if (length != 0)
                {
                    int count = 0;

                    for (int i = 0; i < length; i++)
                    {
                        if (item[i] == question[i])
                            count++;

                        if (i == 0 && item[i] != question[i])
                            break;
                    }

                    if ((100 * count) / length > 70 && length >= question.Length - 2 && length <= question.Length + 2)
                    {
                        return true;
                    }
                }

            }
            catch (Exception e)
            {
                Debug.Log("indexOutOfBoundException");
            }
        }

        return false;
    }

    public IEnumerator InitResultTxt()
    {
        yield return new WaitForSeconds(1.0f);

        resultTxt.text = "Ready";
        recordingBtn.enabled = true;
        soundBtn.enabled = true;
    }

    public IEnumerator ConnectedRecording()
    {
        yield return new WaitForSeconds(0.7f);

        if (!WaveActive)
        {
            resultTxt.text = "Ready";
        }
        else
        {
            resultTxt.text = "Speak!!";
        }
    }

    public IEnumerator WaitForSave()
    {
        yield return new WaitForSeconds(0.5f);
        Call_GetScore();
        yield return new WaitForSeconds(0.5f);

        UIManager.Instance.OpenGameOverPanel();
        Time.timeScale = 0;
        SoundManager.Instance.ChangeBGM();
    }

    public void RepalyGame()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(0);
    }

    private void UnlockUnit()
    {
        lockImg.SetActive(false);
        lockunitBtn.enabled = true;
    }

    public void ExitGame()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("goBack", wave);
    }
}