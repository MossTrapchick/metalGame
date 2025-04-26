using UnityEngine;

public class Bonus : MonoBehaviour
{
    public BonusInfo info;

    public void SetInfo(BonusInfo bonusInfo)
    {
        info = bonusInfo;
        GetComponent<SpriteRenderer>().sprite = info.sprite;
        Destroy(gameObject, info.lifetimeSec);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().PickupBonus(info);
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().PickupBonus(info);
            Destroy(gameObject);
        }
    }
    
}
