using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public int health;
    

    Rigidbody2D rb;
    bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = 100; // Initialize health
    }

    void Update()
    {
       
    }

    public void TakeDamage(int amount)
    {
        if (amount > 0)
        {
            health = Mathf.Max(0, health - amount);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        isGrounded = true;
    }
}
