using System;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bonus : NetworkBehaviour
{
    public BonusInfo info;
    [SerializeField] ParticleSystem effect_1;
    private BonusInfo[] bonusArray;
    
    [Rpc(SendTo.Everyone)]
    public void BuffSetInfoRpc(int index)
    {
        bonusArray = Resources.LoadAll<BonusInfo>("Bonuses/");
        info = bonusArray[index];
        GetComponent<SpriteRenderer>().sprite = info.sprite;
        
        Destroy(gameObject, info.lifetimeSec);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other is CapsuleCollider2D)
        {
            other.GetComponent<PlayerMovement>().PickupBonus(info);
            Destroy(gameObject);
            effect_1.Play();

        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other is CapsuleCollider2D)
        {
            other.GetComponent<PlayerMovement>().PickupBonus(info);
            Destroy(gameObject);
        }
    }
    
}
