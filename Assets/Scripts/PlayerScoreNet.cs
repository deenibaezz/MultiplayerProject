using Unity.Netcode;
using UnityEngine;

public class PlayerScoreNet : NetworkBehaviour
{
    // replicated score value 
    public NetworkVariable<int> Score = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );
    
    // client requests a score increment then server updates the NetworkVariable
    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void AddPointServerRpc()
    {
        Score.Value += 1;
    }
}