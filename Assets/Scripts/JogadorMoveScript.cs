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
    private float lastJumpTime = -999f;
    public float groundIgnoreTimeAfterJump = 0.08f;
    public float dashDuration = 0.4f;
    public Vector2 moveInput;
    public Vector2 directionInput;
    public Vector2 jumpInput;
    public bool isJumping;
    public bool isWallJumping;
    public float wallJumpDuration = 0.2f;
    public float wallJumpForceX = 8f;
    public float wallJumpForceY = 7f;
    public float wallCheckDistance = 0.3f;
    private bool grounded;
    public bool isDashing;
    public bool isGrabbing;
    public bool isFacingRight = true;
    public bool isGrabbingRight = false;
    private bool grabbingPressed = false;
    public bool canGrab = false;
    public bool canDash = true;
    public bool canDoubleJump = true;
    public float jumpBufferTime = 0.3f;
    private float jumpBufferCounter = 0f;
    private bool jumpHeld = false;
    public float gravityForce = 1.5f;
    public float lowGravityForce = 0.5f;
    public bool isSleeping = false;
    public LayerMask terrainLayer;
    public Rigidbody2D rb;
    public BoxCollider2D col;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        HandleGround();
        isTouchingWall();
        Move();
        HandleSleepState();
    }

    public bool isGrounded()
    {
        if (Time.time - lastJumpTime < groundIgnoreTimeAfterJump)
        {
            return false;
        }

        return Physics2D.Raycast(transform.position, Vector2.down, 0.42f, terrainLayer);
    }

    private void HandleGround()
    {
        grounded = isGrounded();

        if (grounded && rb.linearVelocity.y <= 0.05f)
        {
            isDashing = false;
            canDash = true;
            canDoubleJump = true;

            if (jumpBufferCounter > 0f && !isGrabbing && !isDashing)
            {
                DoJump();
            }
        }
    }

    private bool CheckWallBox(Vector2 direction)
    {
        Bounds bounds = col.bounds;

        Vector2 origin = bounds.center;
        Vector2 size = new Vector2(0.05f, bounds.size.y * 0.9f);

        return Physics2D.BoxCast(
            origin,
            size,
            0f,
            direction,
            wallCheckDistance,
            terrainLayer
        );
    }

    public bool isTouchingWall()
    {
        bool touchingLeft = CheckWallBox(Vector2.left);
        bool touchingRight = CheckWallBox(Vector2.right);

        if(touchingRight)
        {
            isGrabbingRight = true;
        }
        else if(touchingLeft)
        {
            isGrabbingRight = false;
        }

        if(touchingLeft || touchingRight)
        {
            canGrab = true;
            DoGrab();
            return true;
        }
        else
        {
            canGrab = false;

            if (isGrabbing)
            {
                isGrabbing = false;
                isGrabbingRight = false;
                rb.gravityScale = gravityForce;

                if (rb.linearVelocity.y > 0)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y / 2f);
                }
            }
            return false;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terreno"))
        {
            isGrounded();
            isTouchingWall();
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Terreno"))
        {
            isGrounded();
            isTouchingWall();
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
        if (isSleeping)
        {
            return;
        }

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
            jumpHeld = true;
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
            else if (isGrounded() && rb.linearVelocity.y <= 0.05f)
            {
                DoJump();
            }
            else if (canDoubleJump && !isGrounded() && !isWallJumping)
            {
                DoJump();
                canDoubleJump = false;
            }
            else
            {
                jumpBufferCounter = jumpBufferTime;
            }
        }
        else if (context.canceled)
        {
            jumpHeld = false;
            if(isJumping && Time.time - jumpInitialTime >= jumpMinDuration)
            {
                ResetJump();
            }else if(isJumping)
            {
                CancelInvoke("ResetJump");
                Invoke("ResetJump", jumpMinDuration - (Time.time - jumpInitialTime));
            }
        }
    }

    private void DoJump()
    {
        jumpInitialTime = Time.time;
        lastJumpTime = Time.time;

        rb.gravityScale = lowGravityForce;
        isJumping = true;
        isDashing = false;
        isGrabbing = false;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        jumpBufferCounter = 0f;

        CancelInvoke("ResetJump");

        if(jumpHeld)
        {
            Invoke("ResetJump", jumpDuration);
        }
        else
        {
            Invoke("ResetJump", jumpMinDuration);
        }
    }

    private void ResetJump()
    {
        if (!isJumping)
        {
            return;
        }

        isJumping = false;
        rb.gravityScale = gravityForce;
        if (rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
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
            grabbingPressed = true;
        }
        else if(context.canceled)
        {
            grabbingPressed = false;
            if(isGrabbing)
            {
                isGrabbing = false;
                isGrabbingRight = false;
                rb.gravityScale = gravityForce;
            }
        }
    }

    private void DoGrab()
    {
        if(!grabbingPressed || isWallJumping)
        {
            return;
        }

        isGrabbing = true;
        isJumping = false;
        isDashing = false;
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
    }

    public bool IsDashing()
    {
        return isDashing;
    }

    public void ResetPlayerState()
    {
        isDashing = false;
        isGrabbing = false;
        isJumping = false;
        isWallJumping = false;
        rb.gravityScale = gravityForce;
        rb.linearVelocity = Vector2.zero;
    }
    
    private void HandleSleepState()
    {
        if (isSleeping)
        {
            rb.gravityScale = gravityForce;
            moveAction.action.Disable();
            jumpAction.action.Disable();
            dashAction.action.Disable();
            grabAction.action.Disable();

            moveInput = Vector2.zero;
            if(grounded)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            moveAction.action.Enable();
            jumpAction.action.Enable();
            dashAction.action.Enable();
            grabAction.action.Enable();
        }
    }
}
