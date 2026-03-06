using Unity.Netcode;
using UnityEngine;

public class NetworkButtons : MonoBehaviour
{
    void OnGUI()
    {
        if (NetworkManager.Singleton == null) return;

        int w = 200, h = 50, pad = 10;

        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            if (GUI.Button(new Rect(pad, pad, w, h), "Start Host"))
                NetworkManager.Singleton.StartHost();

            if (GUI.Button(new Rect(pad, pad + h + pad, w, h), "Start Client"))
                NetworkManager.Singleton.StartClient();
        }
    }
}