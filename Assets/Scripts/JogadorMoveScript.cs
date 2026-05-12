using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using System.Collections;

public class JogadorMoveScript : MonoBehaviour
{
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference dashAction;
    public InputActionReference grabAction;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float dashSpeed = 10f;
    public float grappleSpeed = 3f;
    public float jumpDuration = 0.5f;
    public float jumpMinDuration = 0.2f;
    public float jumpInitialTime;
    public float dashDuration = 0.4f;
    public int grabDuration = 4;
    public Vector2 moveInput;
    public Vector2 directionInput;
    public Vector2 jumpInput;
    public bool isJumping;
    public bool isWallJumping;
    public float wallJumpDuration = 0.2f;
    public float wallJumpForceX = 8f;
    public float wallJumpForceY = 7f;
    public bool isGrounded;
    public bool isDashing;
    public bool isGrabbing;
    public bool isFacingRight = true;
    public bool isGrabbingRight = false;
    public bool canGrab = false;
    public bool canDash = true;
    public bool canJump = true;
    public float gravityForce = 1.5f;
    public float lowGravityForce = 0.5f;
    public Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Move();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Piso"))
        {
            isGrounded = true;
            isJumping = false;
            isDashing = false;
            canDash = true;
            rb.gravityScale = 0;
        }

        if (collision.gameObject.CompareTag("Parede"))
        {
            canGrab = true;
        }

        if (collision.gameObject.CompareTag("Obstáculo"))
        {
            // perder vida e voltar pro começo da cena
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Piso"))
        {
            isGrounded = false;
        }

        if (collision.gameObject.CompareTag("Parede"))
        {
            canGrab = false;
            isGrabbing = false;
            rb.gravityScale = gravityForce;
        }
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        dashAction.action.Enable();
        grabAction.action.Enable();

        moveAction.action.performed += OnMove;
        jumpAction.action.performed += OnJump;
        dashAction.action.performed += OnDash;
        grabAction.action.performed += OnGrab;
        moveAction.action.canceled += OnMove;
        jumpAction.action.canceled += OnJump;
        grabAction.action.canceled += OnGrab;
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
        dashAction.action.Disable();
        grabAction.action.Disable();

        moveAction.action.performed -= OnMove;
        jumpAction.action.performed -= OnJump;
        dashAction.action.performed -= OnDash;
        grabAction.action.performed -= OnGrab;
        moveAction.action.canceled -= OnMove;
        jumpAction.action.canceled -= OnJump;
        grabAction.action.canceled -= OnGrab;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        directionInput = context.ReadValue<Vector2>();
        if (directionInput.x > 0)
        {
            moveInput.x = 1;
            isFacingRight = true;
        }
        else if (directionInput.x < 0)
        {
            moveInput.x = -1;
            isFacingRight = false;
        }
        else
        {
            moveInput.x = 0;
        }

        if (directionInput.y > 0)
        {
            moveInput.y = 1;
        }
        else if (directionInput.y < 0)
        {
            moveInput.y = -1;
        }
        else
        {
            moveInput.y = 0;
        }
    }

    private void Move()
    {
        if (!isGrabbing && !isDashing && !isWallJumping)
        {
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        }
        if (isGrabbing)
        {
            rb.linearVelocity = new Vector2(0, moveInput.y * grappleSpeed);
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpInitialTime = Time.time;

            if (isGrabbing)
            {
                rb.gravityScale = lowGravityForce;
                isJumping = true;
                isDashing = false;
                isGrabbing = false;
                isWallJumping = true;

                float direction = isGrabbingRight ? -1f : 1f;


                rb.linearVelocity = new Vector2(direction * wallJumpForceX, wallJumpForceY);

                Invoke("ResetWallJump", wallJumpDuration);
                Invoke("ResetJump", jumpDuration);
            }
            else if (isGrounded)
            {
                rb.gravityScale = lowGravityForce;
                isJumping = true;
                isDashing = false;
                isGrabbing = false;

                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

                Invoke("ResetJump", jumpDuration);
            }
        }
        else if (context.canceled)
        {
            if(isJumping && Time.time - jumpInitialTime >= jumpMinDuration)
            {
                ResetJump();
            }else if(isJumping)
            {
                Invoke("ResetJump", jumpMinDuration - (Time.time - jumpInitialTime));
            }
        }
    }

    private void ResetJump()
    {
        if(isJumping)
        {
            isJumping = false;
            rb.gravityScale = gravityForce;
            if(rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            }
        }
    }

    private void ResetWallJump()
    {
        if(isWallJumping)
        {
            isWallJumping = false;
        }
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (canDash)
        {
            rb.gravityScale = 0;
            canDash = false;
            isDashing = true;
            isJumping = false;
            isGrabbing = false;
            rb.linearVelocity = new Vector2(directionInput.x * dashSpeed, directionInput.y * dashSpeed);
            StartCoroutine(ResetDashAfterTime(directionInput));
        }
    }

    private IEnumerator ResetDashAfterTime(Vector2 directionInput)
    {
        yield return new WaitForSeconds(dashDuration);
        ResetDash(directionInput);
    }

    private void ResetDash(Vector2 direction)
    {
        if(isDashing)
        {
            isDashing = false;
            rb.gravityScale = gravityForce;
            if (moveInput.x == 0)
            {
                rb.linearVelocity = new Vector2(0, 0);
            }else
            {
                rb.linearVelocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
            }
        }
    }


    private void OnGrab(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (canGrab)
            {
                isGrabbing = true;
                isJumping = false;
                isDashing = false;
                rb.gravityScale = 0;
                rb.linearVelocity = Vector2.zero;
                if(directionInput.x > 0)
                {
                    isGrabbingRight = true;
                }
                else if(directionInput.x < 0)
                {
                    isGrabbingRight = false;
                }
            }
        }
        else if(context.canceled)
        {
            if(isGrabbing)
            {
                isGrabbing = false;
                rb.gravityScale = gravityForce;
            }
        }
    }

    
    
}
