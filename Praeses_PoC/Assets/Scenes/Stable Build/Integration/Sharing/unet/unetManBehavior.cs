using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class unetManBehavior : MonoBehaviour {

    NetworkManager man;
    private NetworkStartPosition[] spawnPoints;
    private int spawnPointIndex;
    public GameObject hologramCollection;
    public InputField ipAddressField;

    private void Start()
    {
        man = gameObject.GetComponent<NetworkManager>();

    }

    public void createRoom()
    {
        man.StartHost();
        spawnPlayer();
    }

    public void joinRoom()
    {
        man.networkAddress = ipAddressField.text;
        man.StartClient();
    }

    public void spawnPlayer()
    {
        GameObject Trainer = null;
        spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        spawnPointIndex = 0;
        Trainer = (GameObject)GameObject.Instantiate(man.playerPrefab, spawnPoints[spawnPointIndex].transform.position, spawnPoints[spawnPointIndex].transform.rotation);

        //Trainer.transform.SetParent(hologramCollection.transform);

        NetworkServer.Spawn(Trainer);
    }

    //private void OnServerInitialized()
    //{
    //    print("server initialized");
    //}
}
