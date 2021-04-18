using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject gameOverPanel;
    public GameObject optionsPanel;
    public GameObject gamePlayPanel;

    public TextMeshProUGUI TimeCounterTxt;
    public TextMeshProUGUI TimeCounterRuleTxt;
    public Button FastModeButton;
    
    public TextMeshProUGUI ModeTxt;
    public TextMeshProUGUI PlayerSkillTxt;


    public TextMeshProUGUI GameOverTxt;

    private float timeCounter = 8;
    private int startingTime = 8;
    private float fasterTime = 0.2f;

    public bool fastMode = false;
    public bool gameOver = true;

    public int StartingTime
    {
        get { return startingTime; }

        set
        {
            startingTime = value;

            PlayerSkillTxt.text = "Timer for the water advance: " + startingTime + "s";
        }
    }

    public float TimeCounter
    {
        get { return timeCounter; }

        set
        {
            timeCounter = value;
            TimeCounterTxt.text = ((int)timeCounter).ToString();
            if (timeCounter <= 0)
            {
                timeCounter = 0;
                SearchForNextPipe();
            }
        }
    }

    private void SearchForNextPipe()
    {
        PuzzlePipeManager.instance.activePipe.SearchForNextPipe();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (!gameOver)
        {
            TimeCounter -= Time.deltaTime;
        }
    }

    private void Start()
    {
        TimeCounter = startingTime;
        gameOver = true;
    }

    public void ResetTimer()
    {
        if (fastMode)
        {
            TimeCounter = fasterTime;
        }
        else
        {
            TimeCounter = startingTime;
        }
        
    }

    public void SetFastMode(bool goFast)
    {
        fastMode = goFast;
        if (goFast) TimeCounter = fasterTime; 
    }


    // Show the game over panel
    public void GameOver()
    {
        gameOver = true;
        PuzzlePipeManager.instance.GameOver();
        gameOverPanel.SetActive(true);
        gamePlayPanel.SetActive(false);
    }

    public void StartGame()
    {
        gamePlayPanel.SetActive(true);
        PuzzlePipeManager.instance.StartGame();
        TimeCounter = startingTime;
        TimeCounterTxt.gameObject.SetActive(true);
        TimeCounterRuleTxt.gameObject.SetActive(true);
        gameOver = false;
        SetFastMode(false);
        FastModeButton.interactable = true;
    }

    public void SetGameMode(int diffculty)
    {
        PuzzlePipeManager.instance.gameMode = (Mode)diffculty;

        if (diffculty == (int)Mode.Easy)
        {
            PuzzlePipeManager.instance.boardSizeX = 6;
            PuzzlePipeManager.instance.transform.position = new Vector3(-3, -1.5f, 0);
            ModeTxt.text = "A simple mode, connect the pipes to the end before the water reaches the end";
        }

        if (diffculty == (int)Mode.Medium)
        {
            PuzzlePipeManager.instance.boardSizeX = 6;
            PuzzlePipeManager.instance.transform.position = new Vector3(-3, -1.5f, 0);
            ModeTxt.text = "WATCH OUT FOR THE BOMBS, DONT LET THE WATER HIT IT!!";
        }

        if (diffculty == (int)Mode.Hard)
        {
            PuzzlePipeManager.instance.boardSizeX = 9;
            PuzzlePipeManager.instance.transform.position = new Vector3(-5, -1.5f, 0);
            ModeTxt.text = "Oh Great now you need to get though a bigger maze of pipes, also BOMBS!!";
        }
    }

    public void SetPlayerSkill(int skill)
    {
        PuzzlePipeManager.instance.playerSkill = (PlayerSkill)skill;

        if (skill == (int)PlayerSkill.Rookie)
        {
            startingTime = 8;
        }

        if (skill == (int)PlayerSkill.Skilled)
        {
            startingTime = 10;
        }

        if (skill == (int)PlayerSkill.Master)
        {
            startingTime = 12;
        }
        PlayerSkillTxt.text = "Timer for the water advance: " + startingTime + "s";
    }
}
