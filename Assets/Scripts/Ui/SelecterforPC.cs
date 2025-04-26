using UnityEngine;
using UnityEngine.InputSystem;

public class SelecterforPC : MonoBehaviour
{
    private InputSystem_Actions inputSystemAction;

    public Instrument ins;

    
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
    }
    
    private void SelectDrum(InputAction.CallbackContext obj)
    {
        ins.isDrum();
    }
    private void SelectGitar(InputAction.CallbackContext obj)
    {
       ins.isGitar();
    }

    private void OnDisable()
    {
        inputSystemAction.Player.TopMenuNavigationDrum.performed -= SelectDrum;
        inputSystemAction.Player.TopMenuNavigationGitar.performed -= SelectGitar;
    }
}
