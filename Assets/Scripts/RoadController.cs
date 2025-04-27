using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RoadController : MonoBehaviour
{
    [SerializeField] float smoothTime;
    [SerializeField] AudioSource[] roads;
    public static UnityEvent<MusicInstrument.Type, bool> ToggleRoad = new();
    private void Start()
    {
        ToggleRoad.AddListener(toggleRoad);
    }
    void toggleRoad(MusicInstrument.Type type, bool enabled)
    {
        float target = enabled ? 1f : 0f;

        if (enabled) roads[(int)type].volume = target;
        else StartCoroutine(smoothChangeVolume(roads[(int)type], target, smoothTime));
    }

    IEnumerator smoothChangeVolume(AudioSource source, float target, float time)
    {
        float startVolume = source.volume;
        float t = 0;
        while (source.volume != target)
        {
            t += Time.deltaTime;
            source.volume = Mathf.InverseLerp(startVolume, target, t);
            yield return new WaitForEndOfFrame();
        }
    }
}
