using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "Bonus", menuName = "ScriptableObject/New bonus")]
public class BonusInfo : ScriptableObject, INetworkSerializable
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
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref title);
        // serializer.SerializeValue(ref sprite);
        serializer.SerializeValue(ref lifetimeSec);
        serializer.SerializeValue(ref type);
        serializer.SerializeValue(ref value);
        serializer.SerializeValue(ref bonusDurationSec);
    }
}
