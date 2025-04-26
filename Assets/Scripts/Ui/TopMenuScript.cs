using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class TopMenuScript : MonoBehaviour
{
    private InputSystem_Actions inputSystemAction;
    private Instrument ins;
    Instrument.Instr curent;
    private void Awake()
    {
        inputSystemAction=new InputSystem_Actions();
        inputSystemAction.Enable();
        ins = GetComponent<Instrument>();
        inputSystemAction.Player.TopMenu.performed+= context => { change(context); };
    }


    private void change(InputAction.CallbackContext context)
    {
        if (context.control == inputSystemAction.Player.TopMenu.controls[1])
        {
            curent = Instrument.Instr.Drums;
            ins.ToggleInstrument(curent);
        }
        else if (context.control == inputSystemAction.Player.TopMenu.controls[2])
        {
            curent = Instrument.Instr.Guitar;
            ins.ToggleInstrument(curent);
        }



    }






}
