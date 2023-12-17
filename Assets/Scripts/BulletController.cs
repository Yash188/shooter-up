using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class BulletController : MonoBehaviour,IDamageable
{
    public IDamageable source { get; set; }

    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] BulletType bulletType; 
    [SerializeField] float speed = 5f;
    [SerializeField] Vector3 directionRandom;

    private int damage;
    private Transform player;
    private Rigidbody2D rb;
    private Vector3 movementDirection;

    private void Start()
    {
        Destroy(gameObject, 2.5f);
    }

    void Update()
    {
        MoveBullet();
    }

    /// <summary>
    /// move bullet depending on type
    /// </summary>
    void MoveBullet()
    {
        switch (bulletType)
        {

            case BulletType.Box:
                transform.Translate(movementDirection* bulletSpeed * Time.deltaTime);
                break;
            case BulletType.Square:
                transform.Translate(Vector3.right * bulletSpeed * Time.deltaTime);
                break;
            case BulletType.Triangle:
                transform.Translate((player.position-transform.position).normalized * bulletSpeed * Time.deltaTime);
                break;

        }
    }

    /// <summary>
    /// sets bullet type
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_damage"></param>
    public void SetBulletType(BulletType _type,int _damage = 1)
    {
        bulletType = _type;
        damage = _damage;
        player = FindObjectOfType<PlayerController>().transform;

        switch (_type)
        {
            case BulletType.Box:
                movementDirection = (player.position - transform.position).normalized;
                transform.Translate(movementDirection* bulletSpeed * Time.deltaTime);
                break;
            case BulletType.Circle: 
                rb = GetComponent<Rigidbody2D>();

                Vector3 initialVelocity = CalculateInitialVelocity(transform.position, player.position, speed); 
                rb.velocity = new Vector2(initialVelocity.x, initialVelocity.y);
                break; 
                 
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object implements the IDamageable interface 
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null && damageable != source )
        {
            // Deal damage to the object
            damageable.TakeDamage(damage);

            // Destroy the bullet
            if(!collision.gameObject.tag.Equals("Bullet"))
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damageAmount = 1)
    {
        // destroy bullet of traingle type
       if(bulletType == BulletType.Triangle)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// calcuate initial velocity for circle bullets
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="targetPos"></param>
    /// <param name="bulletSpeed"></param>
    /// <returns></returns>
    Vector3 CalculateInitialVelocity(Vector3 startPos, Vector3 targetPos, float bulletSpeed)
    {
        // Calculate the direction to the player
        Vector3 direction = (targetPos - startPos).normalized;

        // Calculate the initial velocity
        Vector3 initialVelocity = direction * bulletSpeed;

        return initialVelocity + directionRandom;
    }


}

public enum BulletType
{
    Box,
    Triangle,
    Circle,
    Square
}


