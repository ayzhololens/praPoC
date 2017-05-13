using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class sharedConnectButton : MonoBehaviour {

    public GameObject launchBox;
    public bool isConnected { get; set; }
    public NetworkManager NetworkManagerNull;
    public bool connnectHost;
    public bool disconnectHost;
    public GameObject offsiteWindow;
    public GameObject sharedWindow;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        //clientToggle();
        if (connnectHost)
        {
            connectAsHost();
        }
        else if (disconnectHost)
        {
            disconnectAsHost();
        }
    }

    void connectAsClient()
    {
        NetworkManagerNull.GetComponent<NetworkManager>().networkAddress = "192.168.1.120";
        NetworkManagerNull.GetComponent<NetworkManager>().StartClient();
        launchBox.SetActive(false);
        print(NetworkServer.connections.Count);
    }

    void disconnectAsClient()
    {
        NetworkManagerNull.GetComponent<NetworkManager>().StopClient();
        launchBox.SetActive(true);
    }

    void clientToggle()
    {
        if (isConnected)
        {
            disconnectAsClient();
        }else
        {
            connectAsClient();
        }
    }

    void connectAsHost()
    {
        NetworkManagerNull.GetComponent<NetworkManager>().networkAddress = "localhost";
        NetworkManagerNull.GetComponent<NetworkManager>().StartHost();
        offsiteWindow.SetActive(false);
        sharedWindow.SetActive(true);
        //print(NetworkServer.connections.Count);
    }

    void disconnectAsHost()
    {
        NetworkManagerNull.GetComponent<NetworkManager>().StopHost();
        offsiteWindow.SetActive(true);
        sharedWindow.SetActive(false);
        launchBox.SetActive(true);
    }

    void hostToggle()
    {
        if (isConnected)
        {
            disconnectAsHost();
        }
        else
        {
            connectAsHost();
        }
    }
}
