using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class TopMenuScript : MonoBehaviour
{
    private InputSystem_Actions inputSystemAction;
    private Instrument ins;
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
            ins.isDrum();
        }else if (context.control == inputSystemAction.Player.TopMenu.controls[2])
        {
            ins.isGitar();
        }



    }






}
