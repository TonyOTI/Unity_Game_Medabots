using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public const float FRAME = 1f / 60f;
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public int attackDamage = 10;
    public Vector2 attackBoxSize = new Vector2(1.2f, 1f);
    public float attackForwardOffset = 0.8f;

    Rigidbody2D rb;
    bool isGrounded;
    int jumpCounter;
    bool attackPressed = false;
    bool facingRight = true;
    bool jumpPressed = false;
    float move;
    bool movementPressed = false;

    // Event functions

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleAttackInput();

        HandleJumpInput();

        HandleMovementInput();
    }

    void FixedUpdate()
    {
        // Jump
        if (jumpPressed)
        {
            HandleJump();
            jumpPressed = false;
        }
        // Movement
        if (movementPressed)
        {
            HandleMovement();
            movementPressed = false;
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        if (attackPressed)
        {
            HandleAttack();
            attackPressed = false;
        }
    }

    // Player functions

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void HandleAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            attackPressed = true;
        }
    }

    void HandleAttack()
    {
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        Vector2 attackCenter = (Vector2)transform.position + direction * attackForwardOffset;
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackCenter, attackBoxSize, 0f);
        HashSet<EnemyAI> enemiesHit = new HashSet<EnemyAI>();

        foreach (Collider2D hit in hits)
        {
            EnemyAI enemy = hit.GetComponentInParent<EnemyAI>();
            if (enemy != null && enemiesHit.Add(enemy))
            {
                enemy.TakeDamage(attackDamage);
            }
        }
    }

    void HandleJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCounter < 2)
        {
            jumpPressed = true;
        }
    }

    void HandleJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;
        jumpCounter += 1;
    }

    void HandleMovementInput()
    {
        move = Input.GetAxisRaw("Horizontal");
        if (move != 0)
        {
            movementPressed = true;
        }
    }

    void HandleMovement()
    {
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        isGrounded = true;
        jumpCounter = 0;
    }


}
