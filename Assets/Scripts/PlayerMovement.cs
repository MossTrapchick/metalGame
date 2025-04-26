using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float conversionSpeed = 1;
    
    public SpriteRenderer radius;
    public Color playerColor;
    
    private Vector2 moveDirection = Vector2.zero;
    private InputSystem_Actions inputSystemAction;
    private Rigidbody2D rb;

    private RaycastHit2D[] hits;
    private bool isGrounded = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        radius.color = playerColor;
        
        inputSystemAction = new InputSystem_Actions();
        inputSystemAction.Enable();
        
        inputSystemAction.Player.Move.performed += moving => { moveDirection = moving.ReadValue<Vector2>(); };
        inputSystemAction.Player.Move.canceled += movingStopped => { moveDirection = Vector2.zero; };
        
        inputSystemAction.Player.Jump.started += jumping => { Jump(); };
    }

    private void FixedUpdate()
    {
        hits = Physics2D.RaycastAll(transform.position, Vector2.down, 2f);
        isGrounded = hits[^1].collider.CompareTag("Ground");
        
        radius.color = playerColor;

        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        if (isGrounded)
            rb.linearVelocity = Vector2.up * jumpForce;
    }
}

