using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using Unity.Multiplayer.Playmode;

public class DancerNPC : MonoBehaviour
{

    public int moveSpeed = 5;
    public SpriteRenderer colorIndicator;
    public GameObject conversionBar;
    public SpriteRenderer conversionVal;
    private float maxConversionValSize;
    

    
    private Rigidbody2D rb;
    
    private Coroutine movingCoroutine;
    private Instrument curPlayer;
    Instrument.Instr curentInstrument;



    public float ConversionValue { get; set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ConversionValue = 0;
        maxConversionValSize = conversionVal.size.x;
        conversionVal.size = new Vector2(0, conversionVal.size.y);
        colorIndicator.color = new Color(colorIndicator.color.r, colorIndicator.color.g, colorIndicator.color.b, ConversionValue);

        Invoke(nameof(RandomMovement), Random.Range(1f, 6f));

        

    }   

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (!other.TryGetComponent(out curPlayer)) return;

        curentInstrument = curPlayer.curentInstrument;
        colorIndicator.color = new Color(curPlayer.playerColor.r, curPlayer.playerColor.g, curPlayer.playerColor.b, colorIndicator.color.a);
        if (IsInvoking(nameof(UndoConverse))) CancelInvoke(nameof(UndoConverse));
        if (!IsInvoking(nameof(Converse))) InvokeRepeating(nameof(Converse), 1f, 1f);
        
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.TryGetComponent(out curPlayer)) return;
        curentInstrument = curPlayer.curentInstrument;
        colorIndicator.color = new Color(curPlayer.playerColor.r, curPlayer.playerColor.g, curPlayer.playerColor.b, colorIndicator.color.a);
        if (IsInvoking(nameof(UndoConverse))) CancelInvoke(nameof(UndoConverse));
        if (!IsInvoking(nameof(Converse))) InvokeRepeating(nameof(Converse), 1f, 1f);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent<Instrument>(out _)) return;

        if (curPlayer == other.GetComponent<Instrument>())
        {
            CancelInvoke(nameof(Converse));
            curPlayer = null;
            InvokeRepeating(nameof(UndoConverse), 5f, 3f);
        }
    }

    private void Converse()
    {
        if (curPlayer == null)
        {
            CancelInvoke(nameof(Converse));
            return;
        }

        if (ConversionValue < maxConversionValSize) {

            ConversionValue += curPlayer.curentCoversionSpeed * 0.1f;   
        }

        colorIndicator.color = Color.Lerp(colorIndicator.color, curPlayer.playerColor, ConversionValue);
        conversionVal.color = curPlayer.playerColor;
        conversionVal.size = new Vector2(ConversionValue / maxConversionValSize, conversionVal.size.y);
    }

    private void UndoConverse()
    {
        if (ConversionValue > 0.1f)
            ConversionValue -= 0.1f;
        
        colorIndicator.color = Color.Lerp(colorIndicator.color, new Color(1, 1, 1, 0), ConversionValue);
        conversionVal.color = Color.Lerp(conversionVal.color, Color.white, ConversionValue);
        conversionVal.size = new Vector2(ConversionValue / maxConversionValSize, conversionVal.size.y);
    }

    private void RandomMovement()
    {
        rb.linearVelocity = new Vector2(Random.Range(-1, 1) < 0 ? -moveSpeed : moveSpeed, rb.linearVelocity.y);
        // play walk animation
        
        Invoke(nameof(MovementPause), Random.Range(2f,11f));
    }

    private void MovementPause()
    {
        rb.linearVelocity = Vector2.zero;
        // play idle animation
        Invoke(nameof(RandomMovement), Random.Range(1f,6f));
    }
}
