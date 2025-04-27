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
        foreach (var instrument in instruments)
        {
            GameObject obj = Instantiate(ElementPrefab, transform);
            obj.GetComponent<InstumentButton>().Init(instrument);
        }
    }
    private void OnEnable()
    {
        InputManager.Input.Player.SelectInstrument.performed += Select;
    } 
    private void OnDisable()
    {
        InputManager.Input.Player.SelectInstrument.performed -= Select;
    }

    void Select(InputAction.CallbackContext ctx)
    {
        int d = (int)InputManager.Input.Player.SelectInstrument.ReadValue<float>();
        if (currentId + d < 0 || currentId + d > instruments.Length - 1) return;
        currentId += d;
        OnSelectInstrument.Invoke(instruments[currentId]);
    }
}
