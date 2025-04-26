using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class DancerNPC : MonoBehaviour
{
    public int moveSpeed = 5;
    public SpriteRenderer colorIndicator;
    
    private Rigidbody2D rb;
    
    private Coroutine movingCoroutine;
    private int rand;
    private PlayerMovement curPlayer;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        colorIndicator.color = new Color(colorIndicator.color.r, colorIndicator.color.g, colorIndicator.color.b, 0);

        Invoke(nameof(RandomMovement), Random.Range(1f, 6f));
    }

    private void FixedUpdate()
    {
       // Debug.Log(curPlayer?.name);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out curPlayer))
            colorIndicator.color = new Color(curPlayer.playerColor.r, curPlayer.playerColor.g, curPlayer.playerColor.b, colorIndicator.color.a);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent<PlayerMovement>(out _)) return;
        
        if (curPlayer == other.GetComponent<PlayerMovement>())
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
