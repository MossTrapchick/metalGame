using UnityEngine;

public class PlayerTemp : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    
    private Vector2 moveDirection = Vector2.zero;
    private InputSystem_Actions inputSystemAction;
    private Rigidbody2D rb;
    
    private CompositeCollider2D compositeCollider;

    private void Start()
    {
        rb ??= GetComponent<Rigidbody2D>();
        
        inputSystemAction = new InputSystem_Actions();
        inputSystemAction.Enable();
        
        inputSystemAction.Player.Move.performed += moving => { moveDirection = moving.ReadValue<Vector2>(); };
        inputSystemAction.Player.Move.canceled += movingStopped => { moveDirection = Vector2.zero; };
        
        inputSystemAction.Player.Jump.started += jumping => { Jump(); };
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        if (rb.linearVelocity.y == 0)
            rb.linearVelocity = Vector2.up * jumpForce;
    }
}

