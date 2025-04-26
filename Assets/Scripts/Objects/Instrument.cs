using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Instrument : MonoBehaviour
{

    [SerializeField] instrumentsofviolenceagainstmusic Gitar;
    [SerializeField] instrumentsofviolenceagainstmusic Drum;

    public Color playerColor;
    public SpriteRenderer radius;
    //[SerializeField]  GameObject Instruments;




    private void Start()
    {
        radius.color = playerColor;
    }

    private void FixedUpdate()
    {
        radius.color = playerColor;
    }



    public void isDrum()
    {
        Debug.Log("Вы выбради Барабанную установку");
        radius.transform.localScale = new Vector3(Drum.itemRadius,0,0);
        Debug.Log(radius.transform.localScale);
    }


    public void isGitar()
    {
        Debug.Log("Вы выбради акустическую гитару");
        radius.transform.localScale = new Vector3(Gitar.itemRadius, 0, 0);
        Debug.Log(radius.transform.localScale);
    }

    
   

}
