using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    public Shrine shrine;
    [SerializeField] float shrineDispersionRadius = 8.0f;

    public bool isPaused;

    public GameObject player;
    public playerController playerScript;
    public WaveManager waveManager;

    float timeScaleOrig;

    int gameGoalCount;

    public Round[] rounds;
    int roundIndex = 0;
    public TextMeshProUGUI roundNum;
    public TextMeshProUGUI enemiesRemainingNum;
    public TextMeshProUGUI enemiesRemainingLabel;

    public TextMeshProUGUI currencyNum;

    [SerializeField] int timeBetweenRounds = 30;

    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();

        shrine = GameObject.FindWithTag("Shrine").GetComponent<Shrine>();

        timeScaleOrig = Time.timeScale;
    }

    private void Start()
    {
        waveManager = WaveManager.instance;
        waveManager.remainingEnemiesUpdated.AddListener(UpdateRemainingText);
        waveManager.allMobsKilled.AddListener(MoveToNextRound);
        waveManager.currentRound = rounds[0];
        roundNum.text = "1/" + rounds.Length;
        StartCoroutine(StartRoundAfterDelay(timeBetweenRounds));
    }

    private void OnDisable()
    {
        waveManager.allMobsKilled.RemoveListener(MoveToNextRound);
        waveManager.remainingEnemiesUpdated.RemoveListener(UpdateRemainingText);
    }

    void MoveToNextRound()
    {
        roundIndex++;
        Debug.Log("Round index: " + roundIndex);
        if (roundIndex >= rounds.Length)
        {
            win();
            return;
        }
        roundNum.text = (roundIndex + 1) + "/" + rounds.Length;
        StartCoroutine(StartRoundAfterDelay(timeBetweenRounds));
    }

    IEnumerator StartRoundAfterDelay(int delay)
    {
        enemiesRemainingLabel.text = "Next Round";
        enemiesRemainingNum.text = delay.ToString();
        StartCoroutine(UpdateTimeTilRoundDisplay(delay));
        yield return new WaitForSeconds(delay);
        waveManager.StartRound();
        enemiesRemainingLabel.text = "Remaining";
        enemiesRemainingNum.text = rounds[roundIndex].GetTotalEnemiesInWave().ToString();

    }

    void UpdateRemainingText(int num)
    {
        int curr = Int32.Parse(enemiesRemainingNum.text);
        curr -= num;
        enemiesRemainingNum.text = curr.ToString();
    }

    void UpdateCurrencyNum(int num)
    {
        currencyNum.text = num.ToString();
    }

    IEnumerator UpdateTimeTilRoundDisplay(int delay)
    {
        yield return new WaitForSeconds(1);
        delay--;
        enemiesRemainingNum.text = delay.ToString();
        if (delay > 1)
        {
            StartCoroutine(UpdateTimeTilRoundDisplay(delay));
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(isPaused);
            } else if (menuActive == menuPause)
            {
                stateResume();
            } 

            
        }
    }

    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void stateResume()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void updateGameGoal(int amount)
    {
        gameGoalCount += amount;
        if (gameGoalCount <= 0 )
        {
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(true);
        }
    }

    public void win()
    {
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(true);
    }

    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    public Vector3 GetShrineLocation()
    {
        return shrine.transform.position;
    }

    public Vector3 GetShrineLocationDispersed()
    {
        return new Vector3(UnityEngine.Random.Range(shrine.transform.position.x - shrineDispersionRadius, shrine.transform.position.x + shrineDispersionRadius), shrine.transform.position.y, UnityEngine.Random.Range(shrine.transform.position.y - shrineDispersionRadius, shrine.transform.position.y + shrineDispersionRadius));
    }
}
