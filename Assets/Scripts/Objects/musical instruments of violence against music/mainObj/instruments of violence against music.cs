using UnityEngine;
using UnityEngine.UI;



[CreateAssetMenu(fileName = "New Item",menuName="ScriptableObject/item")] 
public class instrumentsofviolenceagainstmusic : ScriptableObject
{
    public string itemName;
    public string descriptionItem;

    public float itemDamage;
    public float itemRadius;

    public GameObject itemParticle;
    public Sprite itemSprite;
    public Sprite itemIcon;



}
