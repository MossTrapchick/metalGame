using UnityEngine;
using UnityEngine.InputSystem;

public class SelecterforPC : MonoBehaviour
{
    private InputSystem_Actions inputSystemAction;

    private Instrument ins;
    private void Awake()
    {
        inputSystemAction = new InputSystem_Actions();
        inputSystemAction.Enable();
        ins = new Instrument();

    }


    private void OnEnable()
    {
        inputSystemAction.Player.TopMenuNavigationDrum.performed += SelectDrum;
        inputSystemAction.Player.TopMenuNavigationGitar.performed += SelectGitar;
    }
    
    private void SelectDrum(InputAction.CallbackContext obj)
    {
        ins.SelectDrum();
    }
    private void SelectGitar(InputAction.CallbackContext obj)
    {
        ins.SelectGitar();
    }

    private void OnDisable()
    {
        inputSystemAction.Player.TopMenuNavigationDrum.performed -= SelectDrum;
        inputSystemAction.Player.TopMenuNavigationGitar.performed -= SelectGitar;
    }
}
