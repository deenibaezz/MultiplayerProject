using Unity.Netcode;
using UnityEngine;

public class PlayerScoreNet : NetworkBehaviour
{
    public NetworkVariable<int> Score = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void AddPointServerRpc()
    {
        Score.Value += 1;
    }
}