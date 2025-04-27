using UnityEngine;

public class Instrument : MonoBehaviour
{
    public float currentConversionSpeed = 0;

    public Color playerColor;
    public SpriteRenderer radius;
    
    [HideInInspector]
    public Vector3 baseRadius;
    [HideInInspector]
    public Instr curentInstrument;
    [HideInInspector]
    public instrumentsofviolenceagainstmusic[] instruments;

    private int curentId;

    public enum Instr
    {
        Guitar,
        Drums,
        Bass
    }

    private void Start()
    {
        instruments = Resources.LoadAll<instrumentsofviolenceagainstmusic>("Instruments/");
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
    }
}