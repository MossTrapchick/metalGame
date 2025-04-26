using UnityEngine;
using UnityEngine.Events;

public class CameraMovementTracker : MonoBehaviour
{
    private float oldPosition;
    public UnityEvent<float> OnCameraChanged;

    private void Start()
    {
        oldPosition = transform.position.x;
    }

    private void Update()
    {
        if (transform.position.x != oldPosition)
        {
            OnCameraChanged?.Invoke(transform.position.x);
            oldPosition = transform.position.x;
            // Debug.Log("camera x pos changed");
        }
    }
}
