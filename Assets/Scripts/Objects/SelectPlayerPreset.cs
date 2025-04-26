using UnityEngine;

public class SelectPlayerPreset : MonoBehaviour
{
    [SerializeField] GameObject PlayerPreset_gitar;
    [SerializeField] GameObject PlayerPreset_drum;

    public void SelectDrum()
    {
        Debug.Log("�� ������� ���������� ���������");
       
        PlayerPreset_drum.SetActive(true);
        PlayerPreset_gitar.SetActive(false);
        //instrument.GetComponent<Image>().sprite = Drum.itemIcon;
    }

    public void SelectGitar()
    {
        Debug.Log("�� ������� ������������ ������");
        PlayerPreset_gitar.SetActive(true);
        PlayerPreset_drum.SetActive(false);
        //instrument.GetComponent<Image>().sprite = Gitar.itemIcon;
    }
}
