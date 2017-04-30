using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Sharing;
using HoloToolkit.Unity.InputModule;
using System;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Sharing;
using UnityEngine.VR.WSA.Persistence;

public class pocSharingManager : MonoBehaviour {

    SharingStage sharingStage;

    SharingManager sharingMgr;

    RoomManager roomMgr;

    public GameObject anchorObject;

    private WorldAnchor worldAnchor;

    private RoomManagerAdapter listener;

    private static string ROOM_NAME = "AYZ";

    private static string ANCHOR_NAME = "GameRootAnchor";

    private SessionManager sessionMgr;

    private int retryCount = 10;

    private List<byte> exportingAnchorBytes = new List<byte>();

    private WorldAnchorStore anchorStore = null;

    private bool isMaster = false;

    // Use this for initialization
    void Start () {

        sharingStage = SharingStage.Instance;
        sharingMgr = sharingStage.Manager; 
        roomMgr = sharingMgr.GetRoomManager();
        worldAnchor = anchorObject.AddComponent<WorldAnchor>();

        if (roomMgr != null)
        {
            listener = new RoomManagerAdapter();
            listener.RoomAddedEvent += OnRoomAdded;
            listener.RoomClosedEvent += OnRoomClosed;
            listener.UserJoinedRoomEvent += OnUserJoinedRoom;
            listener.UserLeftRoomEvent += OnUserLeftRoom;
            listener.AnchorsChangedEvent += OnAnchorsChanged;
            listener.AnchorsDownloadedEvent += OnAnchorsDownloaded;
            listener.AnchorUploadedEvent += OnAnchorUploadComplete;

            roomMgr.AddListener(listener);
        }

        sessionMgr = SharingStage.Instance.Manager.GetSessionManager();

        SessionManagerAdapter sessListener = new SessionManagerAdapter();
        sessListener.UserJoinedSessionEvent += UserJoinedSession;
        sessionMgr.AddListener(sessListener);

        WorldAnchorStore.GetAsync(AnchorStoreReady);

        //PocSharingMessages.Instance.MessageHandlers[PocSharingMessages.TestMessageID.ChangeColor] = ChangeColor;

    }

    //private void ChangeColor(NetworkInMessage msg)
    //{
    //    if (!isMaster)
    //    {
    //        GetComponent<Renderer>().material.color = Color.red;
    //    }
    //}

    private void AnchorStoreReady(WorldAnchorStore store)
    {
        Debug.Log("Anchor store initialized");
        anchorStore = store;
    }

    private void UserJoinedSession(Session arg1, User arg2)
    {
        Debug.Log("User joined session");
        Invoke("CreateOrJoinRoom", 3);
    }

    //public void TestYo()
    //{
    //    anchorObject.transform.localScale *= 2;
    //}


    public void CreateOrJoinRoom()
    {

        if (roomMgr.GetRoomCount() < 1)
        {
            createRoom();
        } else
        {
            joinRoom();
        }
    }
    
    public void createRoom()
        {
        System.Random rnd = new System.Random();

        Room newRoom = roomMgr.CreateRoom(ROOM_NAME, rnd.Next(), false);
            if (newRoom == null)
            {
                Debug.LogWarning("Cannot create room");
            }
        }

    public void joinRoom()
    {
        Room room = roomMgr.GetRoom(0);
        roomMgr.JoinRoom(room);
    }

    private void OnRoomAdded(Room newRoom)
    {
        Debug.LogFormat("Room {0} added", newRoom.GetName().GetString());
    }

    private void OnRoomClosed(Room room)
    {
        Debug.LogFormat("Room {0} closed", room.GetName().GetString());
    }

    private void OnUserJoinedRoom(Room room, int user)
    {
        User joinedUser = SharingStage.Instance.SessionUsersTracker.GetUserById(user);
        Debug.LogFormat("User {0} joined Room {1}",
            joinedUser.GetName(),
            room.GetName()
            .GetString());
    }

    private void OnUserLeftRoom(Room room, int user)
    {
        User leftUser = SharingStage.Instance.SessionUsersTracker.GetUserById(user);
        Debug.LogFormat("User {0} left Room {1}", leftUser.GetName(), room.GetName().GetString());
    }

    private void OnAnchorsChanged(Room room)
    {
        Debug.LogFormat("Anchors changed for Room {0}", room.GetName().GetString());
    }

    private void OnAnchorsDownloaded(bool successful, AnchorDownloadRequest request, XString failureReason)
    {
        if (successful)
        {
            Debug.LogFormat("Anchors download succeeded for Room {0}", request.GetRoom().GetName().GetString());
            byte[] anchorData = new byte[request.GetDataSize()];
            Debug.Log("Created array");
            int dataSize = request.GetDataSize();
            Debug.Log("Array size " + dataSize.ToString());
            if (request.GetData(anchorData, dataSize))
            {
                Debug.Log("Importing anchor");
                WorldAnchorTransferBatch.ImportAsync(anchorData, OnImportComplete);
            } else
            {
                Debug.Log("Failed to get anchor data");
            }
        }
        else
        {
            Debug.LogFormat("Anchors download failed: {0}", failureReason.GetString());
        }
    }

    private void OnImportComplete(SerializationCompletionReason completionReason, WorldAnchorTransferBatch deserializedTransferBatch)
    {
        if (completionReason != SerializationCompletionReason.Succeeded)
        {
            Debug.Log("Serialization failed");
            return;
        } else
        {
            string[] ids = deserializedTransferBatch.GetAllIds();
            foreach (string id in ids)
            {
                if (gameObject != null)
                {
                    deserializedTransferBatch.LockObject(id, anchorObject);
                    Debug.Log("Import anchor complete!");
                }
                else
                {
                    Debug.Log("Failed to find object for anchor id: " + id);
                }
            }
        }
    }

    private void OnAnchorUploadComplete(bool successful, XString failureReason)
    {
        if (successful)
        {
            Debug.Log("Anchors upload succeeded");
        }
        else
        {
            Debug.LogFormat("Anchors upload failed: {0}", failureReason.GetString());
        }
    }

    /*
     * Anchor export / importing
     *
     */
    public void ExportWorldAnchor()
    {
        isMaster = true;
        Debug.Log("Exporting world anchor.");
        if (anchorStore != null)
        {
            anchorStore.Save(ANCHOR_NAME, worldAnchor);
            Debug.Log("export world anchor voice sent");
            WorldAnchorTransferBatch transferBatch = new WorldAnchorTransferBatch();
            transferBatch.AddWorldAnchor(ANCHOR_NAME, worldAnchor);
            WorldAnchorTransferBatch.ExportAsync(transferBatch, OnExportDataAvailable, OnExportComplete);
        }
        else
        {
            Debug.Log("Anchor store not ready.");
        }
    }

    public void ImportWorldAnchor()
    {
        roomMgr.DownloadAnchor(roomMgr.GetCurrentRoom(), ANCHOR_NAME);
    }

    private void OnExportComplete(SerializationCompletionReason completionReason)
    {
        if (completionReason != SerializationCompletionReason.Succeeded)
        {
            // If we have been transferring data and it failed, 
            // tell the client to discard the data
            Debug.Log("Failed to export anchor");
        }
        else
        {
            // Tell the client that serialization has succeeded.
            // The client can start importing once all the data is received.
            Debug.Log("Uploading anchor");
            roomMgr.UploadAnchor(roomMgr.GetCurrentRoom(), ANCHOR_NAME, exportingAnchorBytes.ToArray(), exportingAnchorBytes.Count);
        }
    }

    private void OnExportDataAvailable(byte[] data)
    {
        // Send the bytes to the client.  Data may also be buffered.
        exportingAnchorBytes.AddRange(data);
    }
    // Update is called once per frame
    void Update () {
		
	}

    private void OnDestroy()
    {
    }
}
