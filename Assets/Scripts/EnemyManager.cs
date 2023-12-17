using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    /// <summary>
    /// enemy health left
    /// </summary>
    public Action<float> enemyHealth;
    public int currentLevel { get; private set; }
    public float currentHealth { get; private set; }

    [SerializeField] EnemyController enemyPrefab;
    [SerializeField] Vector3 spawnPosition; 

    private float maxHealth = 100; 
    private GameManager gameManager;
    private EnemyController enemyInstance;




    private void Start()
    {  
        gameManager = GameManager.mInstance;
        gameManager.OnLevelStart += OnLevelStart;
        gameManager.OnLevelEnd += OnLevelEnd;
    }
     
    /// <summary>
    /// damage when a bullet hits
    /// </summary>
    /// <param name="damageAmount"></param>
    public void TakeDamage(float damageAmount = 1)
    {
        currentHealth -= damageAmount;
        enemyHealth(currentHealth / maxHealth);

        // Check if the enemy is still alive
        if (currentHealth <= 0)
        {
            Destroy(enemyInstance.gameObject);
            /// enemy destroyed inform level change,if any
            gameManager.OnLevelChanged();
        }
    } 

    /// <summary>
    /// spawn enemy on level start
    /// </summary>
    void SpawnEnemy()
    {
        enemyInstance = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, transform);
    }

    /// <summary>
    /// reset enemy for every new game
    /// </summary>
    public void ResetEnemy()
    {
        currentHealth = maxHealth;
        enemyHealth(currentHealth / maxHealth);
    }

    #region Events
    /// <summary>
    /// callback for level end
    /// </summary>
    private void OnLevelEnd()
    {
        if (enemyInstance != null)
            Destroy(enemyInstance.gameObject);
    }

    /// <summary>
    /// callback for level start
    /// </summary>
    /// <param name="_level"></param>
    private void OnLevelStart(int _level)
    {
        currentHealth = maxHealth;
        enemyHealth(currentHealth / maxHealth);
        currentLevel = _level;
        SpawnEnemy();
    }
    #endregion
}
