using UnityEngine;
using UnityEngine.InputSystem;

public class Instrument : MonoBehaviour
{

    [SerializeField] instrumentsofviolenceagainstmusic Gitar;
    [SerializeField] instrumentsofviolenceagainstmusic Drum;

    private float radius;

    private float dmg;

    private Sprite image;
    public void SelectDrum()
    {
        Debug.Log("�� ������� ���������� ���������");
    }
    public void SelectGitar()
    {
        Debug.Log("�� ������� ������������� ������");
    }

}
