using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static RoadController;

public class Instrument : MonoBehaviour
{

    /*[SerializeField] instrumentsofviolenceagainstmusic Gitar;
    [SerializeField] instrumentsofviolenceagainstmusic Drum;*/

    public float baseCoversionSpeed;

    

    //[SerializeField]  GameObject Instruments;


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
        
        radius.color = playerColor;
        isGitar();
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


    public void isDrum()
    {
        
    }


    public void isGitar()
    {
       
    }

    
    
   

}
