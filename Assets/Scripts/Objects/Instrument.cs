using Unity.Netcode;
using UnityEngine;

public class Instrument : NetworkBehaviour
{
    public Color playerColor;
    public SpriteRenderer radius;
    public MusicInstrument currentInstrument;
    public float currentConversionSpeed = 0;
    public bool isPlaying { get; private set; }
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        if (!IsOwner) return;
        UIInstruments.OnSelectInstrument.AddListener(SelectInstrument);
        QTEManager.OnQTEPassed.AddListener(ctx => TogglePlaying(true, OwnerClientId));
        QTEManager.OnQTEMissed.AddListener(ctx => TogglePlaying(false, OwnerClientId));
    }
    [Rpc(SendTo.Everyone)]
    void TogglePlaying(bool enabled, ulong id)
    {
        if (id != OwnerClientId) return;
        if (isPlaying == enabled) return;
        isPlaying = enabled;
        RoadController.ToggleRoad.Invoke(currentInstrument.type, enabled);
        anim.SetBool("IsPlaying", isPlaying);
    }
    void SelectInstrument(MusicInstrument instrument)
    {
        currentInstrument = instrument;
        SelectRpc(OwnerClientId, instrument);
    }
    [Rpc(SendTo.NotMe)]
    void SelectRpc(ulong id, MusicInstrument instrument)
    {
        if (OwnerClientId != id) return; 
        currentInstrument = instrument;
    }
/*
    private void Start()
    {
        radius.color = playerColor;
        baseRadius = radius.transform.localScale;
        ChangeInstrument(0);
    }

    public void ToggleInstrument(Instr type)
    {
        curentId = instruments[(int)type].Id;
        ChangeInstrument(curentId);
    }

    public int GetCurrentInstrument()
    {
        return curentId;
    }

    private void ChangeInstrument(int id)
    {
        currentConversionSpeed = instruments[id].conversionSpeed;
        radius.transform.localScale = new Vector3(instruments[id].itemRadius, instruments[id].itemRadius, instruments[id].itemRadius);
        curentInstrument = (Instr)id;
    }*/
}