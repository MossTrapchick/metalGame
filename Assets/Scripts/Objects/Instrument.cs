using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static RoadController;

public class Instrument : MonoBehaviour
{

 
    public float baseCoversionSpeed;

    




    public Color playerColor;
    public SpriteRenderer radius;
    public Instr curentInstrument;

    int curentId;
    [SerializeField] instrumentsofviolenceagainstmusic[] instruments;

    public enum Instr
        {
            Guitar,
            Drums,
            Bass
        }


    public void ToggleInstrument(Instr type)
    {
     curentId=instruments[(int)type].Id ;
        ChageInstrument(curentId);
        
    }


    private void Start()
    {
        ChageInstrument(0);
        radius.color = playerColor;
    }



    private void ChageInstrument(int id)
    {

            switch (id)
            {
                case 0:
                    baseCoversionSpeed = instruments[0].conversionSpeed;
                    Debug.Log("Вы выбради Барабанную установку");
                    radius.transform.localScale = new Vector3(instruments[0].itemRadius, instruments[0].itemRadius, instruments[0].itemRadius);
                    Debug.Log(radius.transform.localScale);
                    curentInstrument = Instr.Drums;
                    break;
                case 1:
                    baseCoversionSpeed = instruments[1].conversionSpeed;
                    Debug.Log("Вы выбради акустическую гитару");
                    radius.transform.localScale = new Vector3(instruments[1].itemRadius, instruments[1].itemRadius, instruments[1].itemRadius);
                    Debug.Log(radius.transform.localScale);
                    curentInstrument = Instr.Guitar;
                    break;

            }
        
    }



}
