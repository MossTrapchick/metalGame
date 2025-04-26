using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Instrument;

public class SelecterforPC : MonoBehaviour
{
    private InputSystem_Actions inputSystemAction;

    private Instrument ins;

    Instrument.Instr curent;





    private void Awake()
    {
        inputSystemAction = new InputSystem_Actions();
        inputSystemAction.Enable();
       

    }

    private void Start()
    {
        ins = GetComponent<Instrument>();
    }


    private void OnEnable()
    {
        inputSystemAction.Player.TopMenuNavigationDrum.performed += SelectDrum;
        inputSystemAction.Player.TopMenuNavigationGitar.performed += SelectGitar;
        inputSystemAction.Player.TopMenuNavigationGitar.performed += SelectBass;
    }
    
    private void SelectDrum(InputAction.CallbackContext obj)
    {
        curent = Instrument.Instr.Drums;
        ins.ToggleInstrument(curent);
    }
    private void SelectGitar(InputAction.CallbackContext obj)
    {
        curent = Instrument.Instr.Guitar;
        ins.ToggleInstrument(curent);
    }
    private void SelectBass(InputAction.CallbackContext obj)
    {
        curent = Instrument.Instr.Guitar;
        ins.ToggleInstrument(curent);
    }

    private void OnDisable()
    {
        inputSystemAction.Player.TopMenuNavigationDrum.performed -= SelectDrum;
        inputSystemAction.Player.TopMenuNavigationGitar.performed -= SelectGitar;
    }
}
