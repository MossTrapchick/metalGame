using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;



[CreateAssetMenu(fileName = "New Item",menuName="ScriptableObject/item")] 
public class MusicInstrument : ScriptableObject, INetworkSerializable
{
    public enum Type
    {
        Guitar,
        Bass,
        Drums
    }
    public Type type;

    public string itemName;
    public string descriptionItem;

    public float conversionSpeed ;
    public Vector3 itemRadius;

  //  public GameObject itemParticle;
    public Sprite itemSprite;
    public Sprite itemIcon;
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref type);
        serializer.SerializeValue(ref itemName);
        serializer.SerializeValue(ref descriptionItem);
        serializer.SerializeValue(ref conversionSpeed);
        serializer.SerializeValue(ref itemRadius);
    }
}
