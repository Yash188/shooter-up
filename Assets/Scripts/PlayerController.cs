using System;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
   
    [SerializeField] GameObject bulletPrefab; 
    [SerializeField] int damage = 10;
    [SerializeField] float moveSpeed = 5f;



    private PlayerManager Manager;
    private UIManager UIManager;

    private void Start()
    {
        Manager = GetComponentInParent<PlayerManager>();
        UIManager = UIManager.mInstance;
    }


    void Update()
    {
        PlayerMovement();
         
        // Player shooting
        if (Input.GetKeyDown(KeyCode.Space) && !UIManager.isPaused)
        {
            ShootBullet();
        }
    }

    /// <summary>
    /// movement for player
    /// </summary>
    private void PlayerMovement()
    {
        // Player movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, vertical, 0f).normalized;

        // Calculate the new position after movement
        Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;

        // Restrict the player within the screen bounds
        newPosition.x = Mathf.Clamp(newPosition.x, ScreenBox.Left, ScreenBox.Right);
        newPosition.y = Mathf.Clamp(newPosition.y, ScreenBox.Bottom, ScreenBox.Top);

        // Set the new position
        transform.position = newPosition;
    }

    /// <summary>
    /// player shooting
    /// </summary>
    void ShootBullet()
    {
        // Instantiate a bullet
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity); 
        BulletController bulletController = bullet.GetComponent<BulletController>();
        bulletController.source = this;

        // Customize bullet behavior (you can expand this logic for different bullet types)
        bulletController.SetBulletType(BulletType.Square,damage);

        Manager.OnBulletShoot();
    }

    public void TakeDamage(float damageAmount)
    {
        Manager.TakeDamage(damageAmount) ;
    }

    
}