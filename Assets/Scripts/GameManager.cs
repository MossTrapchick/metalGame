using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] GameObject prefab;
    private void Start()
    {
        SpawnPlayerServerRpc(NetworkManager.LocalClientId);
    }
    [Rpc(SendTo.Server)]
    void SpawnPlayerServerRpc(ulong id)
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.GetComponent<NetworkObject>().SpawnWithOwnership(id);
    }
}
