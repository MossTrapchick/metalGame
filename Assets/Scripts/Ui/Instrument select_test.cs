using UnityEngine;
using UnityEngine.UI;

public class Instrumentselect_test : MonoBehaviour
{
    [SerializeField] instrumentsofviolenceagainstmusic instrument;
    private Instrument ins;
    private void Start()
    {
        //ins = new Instrument();
        GetComponent <Image>().sprite = instrument.itemIcon;
    }
    public void acousticGuitarSelect()
    {
        Debug.Log("Выбрана акустическая гитара");
       // ins.isGitar();
    }

    public void drumSetSelect()
    {
        Debug.Log("Выбрана барабанная установка");
        //ins.isDrum();

    }
}
