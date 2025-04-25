using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DancerNPC : MonoBehaviour
{
    public int moveSpeed = 10;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    
    private Coroutine movingCoroutine;
    private int rand;
    private PlayerTemp curPlayer;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        Invoke(nameof(RandomMovement), Random.Range(1f, 6f));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.TryGetComponent(out curPlayer);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent<PlayerTemp>(out _)) return;
        
        if (curPlayer == other.GetComponent<PlayerTemp>())
            curPlayer = null;
    }

    private void RandomMovement()
    {
        rand = Random.Range(-1, 1);
        transform.localScale = new Vector3(rand < 0 ? -transform.localScale.x : transform.localScale.x, transform.localScale.y, transform.localScale.z);

        // play walk animation
        rb.linearVelocity = new Vector2(rand < 0 ? -moveSpeed : moveSpeed, rb.linearVelocity.y);
        
        Invoke(nameof(MovementPause), Random.Range(1f,6f));
    }

    private void MovementPause()
    {
        rb.linearVelocity = Vector2.zero;
        // play idle animation
        Invoke(nameof(RandomMovement), Random.Range(1f,6f));
    }
}
