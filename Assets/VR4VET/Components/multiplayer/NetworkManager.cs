using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;


[System.Serializable]
public class DefaultRoom
{
    public string Name;
    public int sceneIndex;
    public int maxPlayer;
}

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public List<DefaultRoom> defaultRooms;

    public GameObject roomUI;
    public GameObject joinOrCreateUI;
    public GameObject showCodeUI;
    public List<GameObject> digits;

    private Boolean connectedToServer = false;
    private string scenarioNumber;
    private string roomName;

    // Update is called once per frame
    public void ConnectToServer()
    {
        if (connectedToServer)
        {
            joinOrCreateUI.SetActive(true);
            return;
        }
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting to server");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server");
        connectedToServer = true;
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined Lobby");
        joinOrCreateUI.SetActive(true);
    }

    public void createRoomChosen()
    {
        joinOrCreateUI.SetActive(false);
        roomUI.SetActive(true);
    }

    public void initializeRoom(int defaultRoomIndex)
    {
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];
        // Load scene
        PhotonNetwork.LoadLevel(roomSettings.sceneIndex);

        // Create room
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte) roomSettings.maxPlayer;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        PhotonNetwork.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room");
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player entered the room");
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public void CreateRoom(string scenarioNumberChosen)
    {
        scenarioNumber = scenarioNumberChosen;
        System.Random random = new System.Random();
        roomName = scenarioNumber;
        for (int i = 1; i < digits.Count; i++)
        {
            // Generate a random number between 0 and 9
            int randomNumber = random.Next(0, 10);
            // Concatenate the random number as a string
            roomName += randomNumber.ToString();

            digits[i].GetComponent<TextMeshProUGUI>().text = randomNumber.ToString();
        }
        digits[0].GetComponent<TextMeshProUGUI>().text = scenarioNumberChosen;



        // TODO: Display the roomName/code. Maybe put it somewhere in the tablet?
        Debug.Log("Room name: ");
        Debug.Log(roomName);

        roomUI.SetActive(false);
        showCodeUI.SetActive(true);
    }

    public void joinCreatedRoom()
    {
        // Load scene
        PhotonNetwork.LoadLevel(int.Parse(scenarioNumber));

        // Create room
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)4;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public void JoinRoom(string roomName)
    {
        // Load scene
        PhotonNetwork.LoadLevel(int.Parse(roomName.Substring(0, 1)));

        // Join room
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)4;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }
}
