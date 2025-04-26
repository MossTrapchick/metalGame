using UnityEngine;
using UnityEngine.UI;

public class Instrumentselect_test : MonoBehaviour
{
    [SerializeField] instrumentsofviolenceagainstmusic instrument;
    private void Start()
    {
        GetComponent < Image >().sprite = instrument.itemIcon;
    }
    public void acousticGuitarSelect()
    {
        Debug.Log("Выбрана акустическая гитара");
    }

    public void drumSetSelect()
    {
        Debug.Log("Выбрана барабанная установка");
    }
}
