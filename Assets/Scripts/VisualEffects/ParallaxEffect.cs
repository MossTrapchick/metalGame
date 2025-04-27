using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [Tooltip("0 = not moving; 1 = follows camera")]
    [SerializeField] private float parallaxFactor;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private float oldPosition, spriteSize, zPosition;

    private void Awake()
    {
        // spriteSize = GetComponent<SpriteRenderer>().bounds.size.x;
        spriteSize = spriteRenderer.bounds.size.x;
        zPosition = transform.position.z;

        FindAnyObjectByType<CameraMovementTracker>()?.OnCameraChanged.AddListener(SetNewPosition);
    }

    private void Start()
    {
        oldPosition = transform.position.x;
    }

    private void SetNewPosition(float cameraPosition)
    {
        float newPosition = oldPosition + cameraPosition * parallaxFactor;
        transform.position = new Vector3(newPosition, transform.localPosition.y, zPosition);

        UpdateOldPosition(cameraPosition);
    }

    private void UpdateOldPosition(float cameraPosition)
    {
        float tempPos = cameraPosition * (1 - parallaxFactor);
        float spriteLength = spriteSize / 2;

        if (tempPos > oldPosition + spriteLength)
        {
            oldPosition += spriteSize;
        }
        else if (tempPos < oldPosition - spriteLength)
        {
            oldPosition -= spriteSize;
        }
    }
}