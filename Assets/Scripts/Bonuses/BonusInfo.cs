using UnityEngine;

[CreateAssetMenu(fileName = "Bonus", menuName = "ScriptableObject/New bonus")]
public class BonusInfo : ScriptableObject
{
    [Header("Not picked up")]
    public string title;
    public Sprite sprite;
    public float lifetimeSec;
    [Header("When picked up")]
    public BonusType type;
    public float value;
    public float bonusDurationSec;

    public enum BonusType
    {
        MoveSpeed, ConversionSpeed, InfluenceRadius
    }
}
