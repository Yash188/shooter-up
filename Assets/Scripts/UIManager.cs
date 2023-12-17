using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager mInstance;

    public bool isPaused { get; private set; } = false;

    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject achievementPanel;
    [SerializeField] GameObject inGamePanel;
    [SerializeField] GameObject levelCompletePanel;
    [SerializeField] TMP_Text bulletFiredText;
    [SerializeField] Image healthBar; 
    [SerializeField] TMP_Text livesCountText;
    [SerializeField] TMP_Text levelDisplayText;

    GameManager gameManager;
    bool hasGameStarted = false;
    bool levelCompleted = false;


    private void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this;

        }
        else
            Destroy(mInstance);

    }

    private void Start()
    {
        gameManager = GameManager.mInstance; 
        gameManager.OnLevelEnd += OnLevelEnd;
        gameManager.OnGameOver += OnGameOver;
        gameManager.enemyManager.enemyHealth += OnEnemyHealthChange;
        gameManager.playerManager.lives += OnPlayerLivesChange;
    }

    
    private void Update()
    {
        PauseInput();
        StartInput();
    }

    /// <summary>
    /// listens for space input
    /// </summary>
    void StartInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!hasGameStarted)
            {
                startPanel.SetActive(false);
                inGamePanel.SetActive(true);
                gameManager.OnLevelStart(1);
                hasGameStarted = true;
            }
            else
            {
                if (levelCompleted)
                {
                    levelCompleted = false;
                    gameManager.newLevel();
                    levelCompletePanel.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// listen for pause inputs
    /// </summary>
    void PauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                pausePanel.SetActive(false);
                Time.timeScale = 1;
                isPaused = false;

            }
            else
            {
                pausePanel.SetActive(true);
                Time.timeScale = 0;
                isPaused = true;
            }
        }
    }

    /// <summary>
    /// restart ui button callback
    /// </summary>
    public void Restart()
    {
        gameManager.RestartGame();
        inGamePanel.SetActive(true);
        levelDisplayText.text = "Level : 1";

    }

    #region Events

    /// <summary>
    /// game end callback
    /// </summary>
    /// <param name="_playerWon"></param>
    private void OnGameOver(bool _playerWon)
    {
        levelCompletePanel.SetActive(false);
        inGamePanel.SetActive(false);

        if (_playerWon)
        {
            achievementPanel.SetActive(true);
            bulletFiredText.text = "Bullets Fired : " + gameManager.playerManager.bulletFiredCount;
        }
        else
            gameOverPanel.SetActive(true);

        /// reset variables
        hasGameStarted = true;
        levelCompleted = false;
    }

    /// <summary>
    /// callback for level complete
    /// </summary>
    private void OnLevelEnd()
    { 
        levelCompletePanel.SetActive(true);
        levelCompleted = true;
        levelDisplayText.text = "Level : " + (gameManager.currentLevel + 1);
    }

    /// <summary>
    /// callback for change in player lives
    /// </summary>
    /// <param name="obj"></param>
    private void OnPlayerLivesChange(float obj)
    {
        livesCountText.text = ": " + obj;
    }

    /// <summary>
    /// callback for enemy health update
    /// </summary>
    /// <param name="obj"></param>
    private void OnEnemyHealthChange(float obj)
    {
        healthBar.fillAmount = obj;
    }

    #endregion
}
