using UnityEngine;

public class Bonus : MonoBehaviour
{
    public BonusInfo info;
    [SerializeField] ParticleSystem effect_1;


    public void SetInfo(BonusInfo bonusInfo)
    {
        info = bonusInfo;
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
