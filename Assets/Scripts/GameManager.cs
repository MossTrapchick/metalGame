using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] GameObject prefab, bonusPrefab;
    [SerializeField] private Vector3[] bonusLocations;
    private BonusInfo[] bonusArray;
    
    private void Start()
    {
        SpawnPlayerServerRpc(NetworkManager.LocalClientId);
        if (!IsServer) return;
        bonusArray = Resources.LoadAll<BonusInfo>("Bonuses");
        Invoke(nameof(SpawnBuffs), 5f);
    }
    [Rpc(SendTo.Server)]
    void SpawnPlayerServerRpc(ulong id)
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.GetComponent<NetworkObject>().SpawnWithOwnership(id);
    }

    private void SpawnBuffs()
    {
        GameObject buff = Instantiate(bonusPrefab, bonusLocations[Random.Range(0, bonusLocations.Length - 1)], Quaternion.identity);
        buff.GetComponent<Bonus>().SetInfo(bonusArray[Random.Range(0, bonusArray.Length - 1)]);
        buff.GetComponent<NetworkObject>().Spawn();
    }
}
