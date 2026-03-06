using Unity.Netcode;
using UnityEngine;

public class NetworkButtons : MonoBehaviour
{
    void OnGUI()
    {
        if (NetworkManager.Singleton == null) return;

        int w = 180;
        int h = 45;
        int pad = 15;

        // Position buttons on top-right
        float x = Screen.width - w - pad;
        float y = pad;

        GUIStyle style = new GUIStyle(GUI.skin.button);
        style.fontSize = 16;

        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            if (GUI.Button(new Rect(x, y, w, h), "Start Host", style))
                NetworkManager.Singleton.StartHost();

            if (GUI.Button(new Rect(x, y + h + pad, w, h), "Start Client", style))
                NetworkManager.Singleton.StartClient();
        }
    }
}