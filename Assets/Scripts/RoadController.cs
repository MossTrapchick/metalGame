using System.Collections;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    [SerializeField] float smoothTime;
    public enum RoadTypes
    {
        Guitar,
        Bass,
        Drums
    }
    [SerializeField] AudioSource[] roads;
    public void ToggleRoad(RoadTypes type, bool enabled)
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
