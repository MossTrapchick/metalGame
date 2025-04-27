using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Unity.Cinemachine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject front, side;
    public float currentMoveSpeed;
    public float jumpForce;

    private Vector2 moveDirection = Vector2.zero;
    private InputSystem_Actions inputSystemAction;

    private Rigidbody2D rb;
    private RaycastHit2D[] hits;
    private bool isGrounded = false;
    private float startMoveSpeed;
    private Instrument ins;

    public int Score { get; set; }

    private void Start()
    {
        LobbyOrchestrator.PlayerData player = LobbyOrchestrator.PlayersInCurrentLobby.Find(p => p.id == OwnerClientId);
        GetComponentInChildren<TMP_Text>().text = player.Name;

        if (!IsOwner) return;
        FindFirstObjectByType<CinemachineCamera>().Target.TrackingTarget = transform;
        ins = GetComponent<Instrument>();
        rb = GetComponent<Rigidbody2D>();
        startMoveSpeed = currentMoveSpeed;
        inputSystemAction = new InputSystem_Actions();
        inputSystemAction.Enable();
        
        inputSystemAction.Player.Move.performed += moving => { moveDirection = moving.ReadValue<Vector2>(); };
        inputSystemAction.Player.Move.canceled += movingStopped => { moveDirection = Vector2.zero; };
        
        inputSystemAction.Player.Jump.started += jumping => { Jump(); };
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        if ((Instrument.Instr)ins.GetCurrentInstrument() == Instrument.Instr.Drums) return;
        
        hits = Physics2D.RaycastAll(transform.position + new Vector3(0, 1, 0), Vector2.down, 1);
        isGrounded = hits[^1].collider.CompareTag("Ground");

        bool isWalking = moveDirection.x != 0;
        anim.SetBool("IsWalking", isWalking);

        TestRpc(OwnerClientId, isWalking, moveDirection.x < 0);

        rb.linearVelocity = new Vector2(moveDirection.x * currentMoveSpeed, rb.linearVelocity.y);
    }
    [Rpc(SendTo.Everyone)]
    void TestRpc(ulong id, bool isWalking, bool InvertX)
    {
        if (id != OwnerClientId) return;
        side.SetActive(isWalking);
        front.SetActive(!isWalking);
        if(isWalking)
            side.transform.localScale = new Vector3(InvertX ? 1 : -1, 1, 1);
    }
    private void Jump()
    {
        if (!IsOwner) return;
        if (!isGrounded || (Instrument.Instr)ins.GetCurrentInstrument() == Instrument.Instr.Drums) return;
        anim.SetTrigger("Jump");
        rb.linearVelocity = Vector2.up * jumpForce;
    }

    public void PickupBonus(BonusInfo bonusInfo)
    {
        switch (bonusInfo.type)
        {
            case BonusInfo.BonusType.MoveSpeed:
            {
                currentMoveSpeed += bonusInfo.value;
                break;
            }
            case BonusInfo.BonusType.ConversionSpeed:
            {
                ins.currentConversionSpeed += bonusInfo.value;
                break;
            }
            case BonusInfo.BonusType.InfluenceRadius:
            {
                ins.radius.transform.localScale *= bonusInfo.value;
                break;
            }
            default:
                break;
        }
        Invoke(nameof(BonusEnd), bonusInfo.bonusDurationSec);
    }

    private void BonusEnd()
    {
        int current = ins.GetCurrentInstrument();
        currentMoveSpeed = startMoveSpeed;
        ins.currentConversionSpeed = ins.instruments[current].conversionSpeed;
        ins.radius.transform.localScale = ins.baseRadius;
    }

}

