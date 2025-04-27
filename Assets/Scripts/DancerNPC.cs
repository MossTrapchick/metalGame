using System;
using UnityEngine;
using System.Collections;
using Unity.Netcode;
using Random = UnityEngine.Random;

public class DancerNPC : NetworkBehaviour
{
    public int moveSpeed = 5;
    public SpriteRenderer conversionVal;
    [SerializeField] float HoldDelay = 3f;
    [SerializeField] SpriteRenderer mask;
    [Range(0f, 1f)] float progress;
    Coroutine coroutineTimer;

    Coroutine coroutineConverter;
    Instrument player;

    private Animator animator;
    private Rigidbody2D rb;
    private Color baseColor;
    private Coroutine movingCoroutine;
    private PlayerMovement curPlayer;
    private MusicInstrument curPlayerInstrument;

    private float maxConversionValSize;
    public float ConversionValue { get; set; }

    private void Start()
    {
        QTEManager.OnQTEPassed.AddListener(ctx => TogglePlaying(true));
        QTEManager.OnQTEMissed.AddListener(ctx => TogglePlaying(false));
        
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        maxConversionValSize = conversionVal.transform.localScale.x;
        baseColor = mask.color;

        ConversionValue = 0;

        conversionVal.size = new Vector2(0, conversionVal.size.y);
        mask.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0);

        Invoke(nameof(RandomMovement), Random.Range(1f, 6f));
    }

    private void FixedUpdate()
    {
        mask.sprite = GetComponent<SpriteRenderer>().sprite;
        mask.flipX = GetComponent<SpriteRenderer>().flipX;

        animator.SetBool("Walking", Mathf.FloorToInt(rb.linearVelocityX) != 0);
    }

    void TogglePlaying(bool enabled)
    {
        if (player == default || player.OwnerClientId != NetworkManager.LocalClientId) return;
        if (enabled)
        {
            if (coroutineTimer != null) StopCoroutine(coroutineTimer);
            if (coroutineConverter == default) coroutineConverter = StartCoroutine(Converse(player.currentConversionSpeed, player.playerColor));
        }
        else
        {
            if (coroutineConverter != null) StopCoroutine(coroutineConverter);
            if (coroutineTimer == default) coroutineTimer = StartCoroutine(HoldTimer(HoldDelay));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player != default) return;
        TryGetComponent(out player);
        
        // mask.color = new Color(curPlayerInstrument.playerColor.r, curPlayerInstrument.playerColor.g, curPlayerInstrument.playerColor.b, conversionSprite.color.a);
        if (IsInvoking(nameof(UndoConverse))) CancelInvoke(nameof(UndoConverse));
        if (!IsInvoking(nameof(Converse))) InvokeRepeating(nameof(Converse), 1f, 1f);
    }
/*
    [Rpc(SendTo.NotMe)]
    void ConvertingRpc(float speed, bool )
    {
        coroutineTimer = StartCoroutine(converse());
    }*/

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (coroutineTimer != null) StopCoroutine(coroutineTimer);
    }

    IEnumerator Converse(float currentConversionSpeed, Color color)
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
    
    private void UndoConverse()
    {
        if (ConversionValue > 0.1f)
            ConversionValue -= 0.1f;

        // conversionSprite.color = Color.Lerp(conversionSprite.color, baseColor, ConversionValue / 10);
        conversionVal.color = Color.Lerp(conversionVal.color, baseColor, ConversionValue / 10);
        conversionVal.size = new Vector2(ConversionValue / maxConversionValSize, conversionVal.size.y);
    }

    IEnumerator HoldTimer(float Delay)
    {
        yield return new WaitForSeconds(Delay);
        player = default;
        mask.color = Color.black;
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


/*

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out curPlayerInstrument)) return;

        curPlayer = other.GetComponent<PlayerMovement>();
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

    */