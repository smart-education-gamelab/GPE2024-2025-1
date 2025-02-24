using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// @author Bram van den Dongen
/// 21/11/2024
/// This is the script that causes playermovement using WASD and makes the slime climb walls
/// Link to documentation: N.A.
/// </summary>
public enum PlayerState
{
    Idle,
    Walking,
    Falling
}
public class Playermovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform sprite;
    [SerializeField] private Vector3 direction;
    [SerializeField] private GroundCheck groundType;

    internal bool canMove = true;
    internal bool isGrounded;

    private bool isOnSlime;
    private Rigidbody2D rb;

    public PlayerState currentState = PlayerState.Idle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        sprite.rotation = Quaternion.Euler(0, 0, angle);

        if (rb.velocity.x < 0)
        {
            sprite.localScale = new Vector3(0.4f, -0.4f, 0.4f);
        }
        else
        {
            sprite.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }
        
        //If WASD is pressed set move the player if possible
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && canMove && (isGrounded || isOnSlime))
        {
            if(currentState != PlayerState.Walking)
            {
                animator.Play("WalkStart");
                currentState = PlayerState.Walking;
            }
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
        else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && canMove && (isGrounded || isOnSlime))
        {
            if (currentState != PlayerState.Walking)
            {
                animator.Play("WalkStart");
                currentState = PlayerState.Walking;
            }
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        } 
        else if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && canMove && isOnSlime)
        {
            if (currentState != PlayerState.Walking)
            {
                animator.Play("WalkStart");
                currentState = PlayerState.Walking;
            }
            rb.velocity = new Vector2(rb.velocity.x, moveSpeed);
        }
        else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W) && canMove && isOnSlime)
        {
            if (currentState != PlayerState.Walking)
            {
                animator.Play("WalkStart");
                currentState = PlayerState.Walking;
            }
            rb.velocity = new Vector2(rb.velocity.x, -moveSpeed);
        }

        //If the player is not moving while on slime set his movement to zero
        if(isOnSlime && !Input.anyKey)
        {
            if(currentState != PlayerState.Idle)
            {
                animator.Play("WalkStop");
            }
            rb.velocity = Vector2.zero;
            currentState = PlayerState.Idle;
        }
        

        if ((isGrounded || isOnSlime) && rb.velocity == Vector2.zero && currentState == PlayerState.Walking)
        {
            animator.Play("WalkStop");
            currentState = PlayerState.Idle;
        }

        if (!isGrounded && !isOnSlime && currentState != PlayerState.Falling && groundType.currentGround != GroundCheck.groundType.Slime)
        {
            animator.Play("StartFall");
            currentState = PlayerState.Falling;
        }

        if(isGrounded && currentState != PlayerState.Idle && currentState != PlayerState.Walking)
        {
            animator.Play("Land");
            currentState = PlayerState.Idle;
        }
    }

    // Collision checks to see what surface the player is on and apply neccisary effetcts
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            spriteRenderer.flipY = false;
        }
        if(collision.gameObject.tag == "SlimeBridge")
        {
            isOnSlime = true;
            rb.gravityScale = 0;
            if (rb.velocity.y != 0 && collision.transform.position.x > transform.position.x && Input.GetKey(KeyCode.W))
            {
                spriteRenderer.flipY = true;
            }
            else
            {
                spriteRenderer.flipY = false;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
        if (collision.gameObject.tag == "SlimeBridge")
        {
            isOnSlime = false;
            rb.gravityScale = 5;
        }
    }
}
