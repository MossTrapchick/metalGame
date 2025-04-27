using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputSystem_Actions Input;
    private void Awake()
    {
        Input = new();
    }
}
