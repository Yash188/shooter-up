using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager mInstance;

    /// <summary>
    /// callback for level progress update
    /// </summary>
    public Action OnLevelChanged, OnLevelEnd;
    /// <summary>
    /// callback for whence level start
    /// </summary>
    public Action<int> OnLevelStart;
    /// <summary>
    /// callback for game end update
    /// </summary>
    public Action<bool> OnGameOver;

    public PlayerManager playerManager { get; private set; }
    public EnemyManager enemyManager { get; private set; }

    public int currentLevel { get; private set; } = 1;

    private void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this;
        }
        else
            Destroy(this);
    }


    private void Start()
    {
        OnLevelChanged += OnLevelChange;
        playerManager = FindObjectOfType<PlayerManager>();
        enemyManager = FindObjectOfType<EnemyManager>(); 
        
    }
     

    void OnLevelChange()
    {
        OnLevelEnd();

        //Destroy active bullets
        foreach (var item in FindObjectsOfType<BulletController>()) 
        {
            Destroy(item.gameObject);
        }

        // change level
        Debug.Log("Level Completed");

        if (currentLevel == 3 && enemyManager.currentHealth <= 0) 
        {
            Debug.Log("Game Completed");
            OnGameOver(true);
            return;
        }

        
        currentLevel++;
    }

    public void newLevel()
    {
        OnLevelStart(currentLevel);
    }


    public void RestartGame()
    {

        currentLevel = 1;
        playerManager.ResetPlayer();
        enemyManager.ResetEnemy();
        OnLevelStart(1);
    }

    
}
