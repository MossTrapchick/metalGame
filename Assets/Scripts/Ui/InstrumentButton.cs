using UnityEngine;
using UnityEngine.UI;

public class InstumentButton : MonoBehaviour
{
    [SerializeField] MusicInstrument instrument;
    [SerializeField] Image icon;

    public void Init(MusicInstrument instrument)
    {
        this.instrument = instrument;
        icon.sprite = instrument.itemIcon;
    }
    public void OnClick()
    {
        UIInstruments.OnSelectInstrument.Invoke(instrument);
    }
}
