using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class sharedConnectButton : MonoBehaviour {

    public NetworkManager NetworkManagerNull;
    public bool connnectPC;
    public bool disconnectPC;
    public GameObject offsiteWindow;
    public GameObject sharedWindow;
    public InputField IPaddressField;

    public GameObject IPaddressText;
    public GameObject hostButton;
    public GameObject stophostButton;

    public GameObject avatarObj;
    public GameObject focusGuidedTarget;
    public GameObject focusTarget;

    private void OnMouseDown()
    {
        if (connnectPC)
        {
            connectAsPCClient();
        }
        else if (disconnectPC)
        {
            disconnectAsPCClient();
        }
    }

    public void connectAsPCClient()
    {
        string IPAddress = IPaddressField.text;
        NetworkManagerNull.GetComponent<NetworkManager>().networkAddress = IPAddress;
        NetworkManagerNull.GetComponent<NetworkManager>().StartClient();
        offsiteWindow.SetActive(false);
        sharedWindow.SetActive(true);
    }

    public void disconnectAsPCClient()
    {
        NetworkManagerNull.GetComponent<NetworkManager>().StopClient();
        offsiteWindow.SetActive(true);
        sharedWindow.SetActive(false);
    }

    public void connectAsHololensHost()
    {
        NetworkManagerNull.GetComponent<NetworkManager>().networkAddress = "localhost";
        NetworkManagerNull.GetComponent<NetworkManager>().StartHost();
        IPaddressText.GetComponent<TextMesh>().text = "IP is : 10.10.13.94";// + Network.player.ipAddress;
        hostButton.SetActive(false);
        stophostButton.SetActive(true);
        IPaddressText.SetActive(true);
        //print(NetworkServer.connections.Count);
    }

    public void disconnectAsHololensHost()
    {
        NetworkManagerNull.GetComponent<NetworkManager>().StopHost();
        hostButton.SetActive(true);
        stophostButton.SetActive(false);
        IPaddressText.SetActive(false);
    }

    public void avatarFocus()
    {
        avatarObj.GetComponent<RPCfunctions>().focus();
    }

    public void avatarOpen()
    {
        avatarObj.GetComponent<RPCfunctions>().RpcOpen();
    }

    public void avatarPlay()
    {
        avatarObj.GetComponent<RPCfunctions>().RpcPlay();
    }
}
