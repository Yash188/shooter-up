using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{

   
    /// <summary>
    /// player lives count
    /// </summary>
    public Action<float> lives;
    public int bulletFiredCount { get; private set; } = 0;
    public bool isRespawning { get; private set; }

    [SerializeField] int respawnDelay = 3;
    [SerializeField] PlayerController playerPrefab; 

    private int maxLife = 3;
    private float livesLeft;
    private PlayerController playerInstance;
    private GameManager gameManager;




    private void Start()
    {
        livesLeft = maxLife;
        lives(livesLeft);
        gameManager = GameManager.mInstance;

        gameManager.OnLevelStart += OnLevelStart;
        gameManager.OnLevelEnd += OnLevelEnd;
        
    }


    public void TakeDamage(float damageAmount)
    {

        // Check if the player is still alive
        if (livesLeft <= 0)
        {  
            gameManager.OnLevelChanged();
            gameManager.OnGameOver(false);

            return;
        }

        livesLeft -= damageAmount; 
        Destroy(playerInstance.gameObject); 
        lives(livesLeft);

        /// respawn 
        StartCoroutine(RespawnPlayer());
    }

   IEnumerator RespawnPlayer()
    {
        isRespawning = true;
        yield return new WaitForSeconds(respawnDelay); 
        SpawnPlayer();
         
    }

    /// <summary>
    /// spawn player
    /// </summary>
    void SpawnPlayer()
    {
        playerInstance = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity,transform);
        isRespawning = false ;

    }

   
    /// <summary>
    /// reset before every new game
    /// </summary>
    public void ResetPlayer()
    {
        livesLeft = maxLife;
        lives(livesLeft);
        bulletFiredCount = 0;
    }

    /// <summary>
    /// count on bullets fired
    /// </summary>
    public void OnBulletShoot()
    {
        bulletFiredCount++;

    }

    #region Events

    /// <summary>
    /// callback for level start
    /// </summary>
    /// <param name="_level"></param>
    void OnLevelStart(int _level)
    {
        SpawnPlayer();
    }

    /// <summary>
    /// callback for level end
    /// </summary>
    private void OnLevelEnd()
    {
        Destroy(playerInstance.gameObject);
        StopAllCoroutines();
    }
    #endregion
}
