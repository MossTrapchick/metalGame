using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIInstruments : MonoBehaviour
{
    [SerializeField] GameObject ElementPrefab;
    [SerializeField] MusicInstrument[] instruments;
    private int currentId = 0;
    public static UnityEvent<MusicInstrument> OnSelectInstrument = new();
    private void Start()
    {
        InputManager.Input.Player.SelectInstrument.performed += Select;
        foreach (var instrument in instruments)
        {
            GameObject obj = Instantiate(ElementPrefab, transform);
            obj.GetComponent<InstumentButton>().Init(instrument);
        }
    }
    private void OnDisable()
    {
        InputManager.Input.Player.SelectInstrument.performed -= Select;
    }

    void Select(InputAction.CallbackContext ctx)
    {
        int d = (int)InputManager.Input.Player.SelectInstrument.ReadValue<float>();
        if (currentId + d < 0 || currentId + d > instruments.Length - 1) return;
        currentId += d>0? 1: -1;
        Debug.Log(d);
        OnSelectInstrument.Invoke(instruments[currentId]);
    }
}
