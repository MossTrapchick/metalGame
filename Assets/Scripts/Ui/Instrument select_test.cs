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
        Debug.Log("������� ������������ ������");
       // ins.isGitar();
    }

    public void drumSetSelect()
    {
        Debug.Log("������� ���������� ���������");
        //ins.isDrum();

    }
}
