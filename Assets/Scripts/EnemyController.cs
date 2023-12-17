    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class EnemyController : MonoBehaviour, IDamageable
{
    public float minShootDelay = 1f;
    public float maxShootDelay = 3f;

    [SerializeField] float moveSpeed = 4f; 
    [SerializeField] BulletController[] bulletList;


    private float upperBound = ScreenBox.Top;  
    private float lowerBound = ScreenBox.Bottom;   
    private float nextShootTime;
    private EnemyManager EnemyManager; 
    private BulletController bulletPrefab;
    private BulletType bulletType;


    void Start()
    { 
        SetRandomNextShootTime();
        EnemyManager = GetComponentInParent<EnemyManager>();
        bulletPrefab = bulletList[0];
        bulletType = BulletType.Box ;
    }


    void Update()
    {
        // Enemy movement
        MoveEnemy();

        // Enemy shooting
        if (Time.time >= nextShootTime)
        {
            if(!GameManager.mInstance.playerManager.isRespawning)
            ShootBullet();

            SetRandomNextShootTime();
        }

    }

    /// <summary>
    /// movement code for enemy 
    /// </summary>
    void MoveEnemy()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime); 
        if (transform.position.y > upperBound || transform.position.y < lowerBound)
        {
            moveSpeed *= -1f;
        }
    }

    /// <summary>
    /// shoot bullet
    /// </summary>
    void ShootBullet()
    {
        // Instantiate a bullet
        BulletController bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<BulletController>().source = this;
        bullet.SetBulletType(bulletType);
         
    }

    public void TakeDamage(float damageAmount = 1)
    {
        EnemyManager.TakeDamage(damageAmount);

        /// change bullet as per level

        if (EnemyManager.currentLevel == 1)
        {
            if (EnemyManager.currentHealth <= 100)
            {
                bulletPrefab = bulletList[0];
                bulletType = BulletType.Box;
            }
        }
        else if (EnemyManager.currentLevel == 2)
        {
            if (EnemyManager.currentHealth <= 50)
            {
                bulletPrefab = bulletList[1];
                bulletType = BulletType.Triangle;

            } 
        }
        else if (EnemyManager.currentLevel == 3)
        {
            if (EnemyManager.currentHealth <= 66 && EnemyManager.currentHealth >= 33)
            {
                bulletPrefab = bulletList[1];
                bulletType = BulletType.Triangle;

            }
            else if (EnemyManager.currentHealth <= 33)
            {
                bulletPrefab = bulletList[2];
                bulletType = BulletType.Circle;
            }
        }
    }

    /// <summary>
    /// calculates shoot time for enemy
    /// </summary>
    void SetRandomNextShootTime()
    {
        nextShootTime = Time.time + Random.Range(minShootDelay, maxShootDelay);
    }

    
}

