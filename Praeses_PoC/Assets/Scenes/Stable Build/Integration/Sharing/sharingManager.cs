using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Sharing;
using HoloToolkit.Unity.InputModule;
using System;

public class sharingManager : MonoBehaviour {

    SharingStage sharingStage;
    SharingManager sharingMgr;
    RoomManager roomMgr;
    private RoomManagerAdapter listener;

    private static string ROOM_NAME = "AYZ";

    private SessionManager sessionMgr;

    // Use this for initialization
    void Start () {

        sharingStage = SharingStage.Instance;
        sharingMgr = sharingStage.Manager; 
        roomMgr = sharingMgr.GetRoomManager();


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
    }

    private void UserJoinedSession(Session arg1, User arg2)
    {
        Debug.Log("User joined session");
        Invoke("CreateOrJoinRoom", 3);
    }


    public void CreateOrJoinRoom()
    {
        System.Random rnd = new System.Random();

        if (roomMgr.GetRoomCount() < 1)
        {
            Room newRoom = roomMgr.CreateRoom(ROOM_NAME, rnd.Next(), false);
            if (newRoom == null)
            {
                Debug.LogWarning("Cannot create room");
            }
        } else
        {
            Room room = roomMgr.GetRoom(0);
            roomMgr.JoinRoom(room);
        }
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
        }
        else
        {
            Debug.LogFormat("Anchors download failed: {0}", failureReason.GetString());
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

    // Update is called once per frame
    void Update () {
		
	}

    private void OnDestroy()
    {
    }
}
