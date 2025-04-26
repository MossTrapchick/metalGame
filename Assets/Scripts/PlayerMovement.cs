using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
   

    /*    public SpriteRenderer radius;
        public Color playerColor;*/

    private Vector2 moveDirection = Vector2.zero;
    private InputSystem_Actions inputSystemAction;
    private Rigidbody2D rb;

    private RaycastHit2D[] hits;
    private bool isGrounded = false;

    private float baseSpeed;
    private Vector3 baseRadius;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();


        baseSpeed = moveSpeed;
        

        
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
        
 

        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        if (isGrounded)
            rb.linearVelocity = Vector2.up * jumpForce;
    }

    public void PickupBonus(BonusInfo bonusInfo)
    {
        switch (bonusInfo.type)
        {
            case BonusInfo.BonusType.MoveSpeed:
            {
                moveSpeed += bonusInfo.value;
                break;
            }
            case BonusInfo.BonusType.ConversionSpeed:
            {
               // conversionSpeed += bonusInfo.value;
                break;
            }
            case BonusInfo.BonusType.InfluenceRadius:
            {
               // radius.transform.localScale *= bonusInfo.value;
                break;
            }
            default:
                break;
        }
        Invoke(nameof(BonusEnd), bonusInfo.bonusDurationSec);
    }

    private void BonusEnd()
    {
        moveSpeed = baseSpeed;
        //conversionSpeed = baseCoversionSpeed;
      // radius.transform.localScale = baseRadius;
    }

}

