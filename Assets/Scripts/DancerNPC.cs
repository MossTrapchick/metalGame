using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using Unity.Multiplayer.Playmode;
using UnityEngine.Serialization;

public class DancerNPC : MonoBehaviour
{
    public int moveSpeed = 5;
    public SpriteRenderer conversionVal, conversionSprite;
    private float maxConversionValSize;

    private Rigidbody2D rb;
    private Color baseColor;
    private Coroutine movingCoroutine;
    private PlayerMovement curPlayer;
    private Instrument curPlayerInstrument;
    private Animator animator;

    public float ConversionValue { get; set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        baseColor = conversionSprite.color;

        ConversionValue = 0;
        maxConversionValSize = conversionVal.size.x;

        conversionVal.size = new Vector2(0, conversionVal.size.y);
        conversionSprite.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0);

        Invoke(nameof(RandomMovement), Random.Range(1f, 6f));
    }

    private void FixedUpdate()
    {
        conversionSprite.sprite = GetComponent<SpriteRenderer>().sprite;
        conversionSprite.flipX = GetComponent<SpriteRenderer>().flipX;

        animator.SetBool("Walking", Mathf.FloorToInt(rb.linearVelocityX) != 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out curPlayerInstrument)) return;

        curPlayer = other.GetComponent<PlayerMovement>();
        conversionSprite.color = new Color(curPlayerInstrument.playerColor.r, curPlayerInstrument.playerColor.g, curPlayerInstrument.playerColor.b, conversionSprite.color.a);
        if (IsInvoking(nameof(UndoConverse))) CancelInvoke(nameof(UndoConverse));
        if (!IsInvoking(nameof(Converse))) InvokeRepeating(nameof(Converse), 1f, 1f);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.TryGetComponent(out curPlayerInstrument)) return;

        curPlayer = other.GetComponent<PlayerMovement>();
        conversionSprite.color = new Color(curPlayerInstrument.playerColor.r, curPlayerInstrument.playerColor.g, curPlayerInstrument.playerColor.b, conversionSprite.color.a);
        if (IsInvoking(nameof(UndoConverse))) CancelInvoke(nameof(UndoConverse));
        if (!IsInvoking(nameof(Converse))) InvokeRepeating(nameof(Converse), 1f, 1f);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent<Instrument>(out _)) return;
        if (curPlayerInstrument != other.GetComponent<Instrument>()) return;

        CancelInvoke(nameof(Converse));
        curPlayer = null;
        curPlayerInstrument = null;
        InvokeRepeating(nameof(UndoConverse), 5f, 3f);
    }

    private void Converse()
    {
        if (curPlayerInstrument == null)
        {
            CancelInvoke(nameof(Converse));
            return;
        }

        if (ConversionValue < maxConversionValSize)
        {
            ConversionValue += curPlayerInstrument.currentConversionSpeed / 10;
            curPlayer.Score += Mathf.FloorToInt(curPlayerInstrument.currentConversionSpeed);
        }

        conversionSprite.color = Color.Lerp(conversionSprite.color, curPlayerInstrument.playerColor, ConversionValue / 10);
        conversionVal.color = curPlayerInstrument.playerColor;
        conversionVal.size = new Vector2(ConversionValue / maxConversionValSize, conversionVal.size.y);
    }

    private void UndoConverse()
    {
        if (ConversionValue > 0.1f)
            ConversionValue -= 0.1f;

        conversionSprite.color = Color.Lerp(conversionSprite.color, baseColor, ConversionValue / 10);
        conversionVal.color = Color.Lerp(conversionVal.color, baseColor, ConversionValue / 10);
        conversionVal.size = new Vector2(ConversionValue / maxConversionValSize, conversionVal.size.y);
    }

    private void RandomMovement()
    {
        float rand = Random.Range(-1, 1);
        GetComponent<SpriteRenderer>().flipX = rand < 0;
        rb.linearVelocity = new Vector2(rand < 0 ? -moveSpeed : moveSpeed, rb.linearVelocity.y);

        Invoke(nameof(MovementPause), Random.Range(2f, 11f));
    }

    private void MovementPause()
    {
        rb.linearVelocity = Vector2.zero;
        Invoke(nameof(RandomMovement), Random.Range(1f, 6f));
    }
}