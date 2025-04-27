using System;
using UnityEngine;
using System.Collections;
using Unity.Netcode;

public class DancerNPC : NetworkBehaviour
{
    [SerializeField] float HoldDelay = 3f;
    [SerializeField] SpriteRenderer mask;
    [Range(0f, 1f)] float progress;
    Coroutine coroutineTimer;
    Coroutine coroutineConverter;
    Instrument player;
    private void Start()
    {
        QTEManager.OnQTEPassed.AddListener(ctx => TogglePlaying(true));
        QTEManager.OnQTEMissed.AddListener(ctx => TogglePlaying(false));
    }
    void TogglePlaying(bool enabled)
    {
        if (player == default || player.OwnerClientId != NetworkManager.LocalClientId) return;
        if (enabled)
        {
            if (coroutineTimer != null) StopCoroutine(coroutineTimer);
            if (coroutineConverter == default) coroutineConverter = StartCoroutine(converse(player.currentConversionSpeed, player.playerColor));
        }
        else
        {
            if(coroutineConverter!= null) StopCoroutine(coroutineConverter);
            if (coroutineTimer == default) coroutineTimer = StartCoroutine(HoldTimer(HoldDelay));
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player != default) return;
        TryGetComponent(out player);
    }
    private void OnTriggerExit(Collider other)
    {
        TogglePlaying(false);
    }/*
    [Rpc(SendTo.NotMe)]
    void ConvertingRpc(float speed, bool )
    {
        coroutineTimer = StartCoroutine(converse());
    }*/
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (coroutineTimer != null) StopCoroutine(coroutineTimer);
    }
    IEnumerator converse(float currentConversionSpeed, Color color)
    {
        float MaxValue = 100, t = 0;
        while (progress < 1)
        {
            t += currentConversionSpeed;
            progress = Mathf.InverseLerp(0, MaxValue, t);
            mask.color = Color.Lerp(Color.black, color, progress);
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator HoldTimer(float Delay)
    {
        yield return new WaitForSeconds(Delay);
        player = default;
        mask.color = Color.black;
    }

/*

    public int moveSpeed = 5;
    public SpriteRenderer conversionVal, conversionSprite;

    private Rigidbody2D rb;
    private Color baseColor;
    private Coroutine movingCoroutine;
    private PlayerMovement curPlayer;
    private MusicInstrument curPlayerInstrument;
    private Animator animator;

    public float ConversionValue { get; set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        baseColor = conversionSprite.color;

        ConversionValue = 0;

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
            //ConversionValue += curPlayerInstrument.currentConversionSpeed / 10;
            //curPlayer.Score += Mathf.FloorToInt(curPlayerInstrument.currentConversionSpeed);
        }

        //conversionSprite.color = Color.Lerp(conversionSprite.color, curPlayerInstrument.playerColor, ConversionValue / 10);
        //conversionVal.color = curPlayerInstrument.playerColor;
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
    }*/
}