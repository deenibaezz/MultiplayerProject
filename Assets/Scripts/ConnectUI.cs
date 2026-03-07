using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConnectUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField ipInput;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    void Start()
    {
        if (ipInput != null) ipInput.text = "";

        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });

        clientButton.onClick.AddListener(() =>
        {
            var ut = NetworkManager.Singleton.GetComponent<UnityTransport>();
            if (ut != null && ipInput != null && !string.IsNullOrEmpty(ipInput.text))
            {
                ut.ConnectionData.Address = ipInput.text.Trim();
            }
            NetworkManager.Singleton.StartClient();
        });
    }

    void Update()
    {
        if (NetworkManager.Singleton == null) return;

        // Hide the whole NetUI once connected (host or client)
        if (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer)
        {
            gameObject.SetActive(false);
        }
    }
}