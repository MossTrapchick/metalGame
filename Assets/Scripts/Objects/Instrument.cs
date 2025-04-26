using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Instrument : MonoBehaviour
{

    [SerializeField] instrumentsofviolenceagainstmusic Gitar;
    [SerializeField] instrumentsofviolenceagainstmusic Drum;

    public float baseCoversionSpeed;

    

    //[SerializeField]  GameObject Instruments;


    public Color playerColor;
    public SpriteRenderer radius;
    public Instr curentInstrument;

    public enum Instr
        {
            Guitar,
            Drums,
            Bass

        }

    private void Start()
    {
        
        radius.color = playerColor;
        isGitar();
    }
    public void isDrum()
    {
        baseCoversionSpeed =Drum.conversionSpeed;
        Debug.Log("Вы выбради Барабанную установку");
        radius.transform.localScale = new Vector3(Drum.itemRadius, Drum.itemRadius, Drum.itemRadius);
        Debug.Log(radius.transform.localScale);
        curentInstrument = Instr.Drums;
    }


    public void isGitar()
    {
        baseCoversionSpeed = Gitar.conversionSpeed;
        Debug.Log("Вы выбради акустическую гитару");
        radius.transform.localScale = new Vector3(Gitar.itemRadius, Gitar.itemRadius, Gitar.itemRadius);
        Debug.Log(radius.transform.localScale);
        curentInstrument = Instr.Guitar;
    }

    
    
   

}
