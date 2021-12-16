using Mirror;
using Steamworks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private Transform _list;
    private List<NetworkConnection> _connectionsList = new List<NetworkConnection>();
    private int? _killerConnectionId = null; 
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if(SceneManager.GetActiveScene().name == "Game")
        {
            if(conn.connectionId == _killerConnectionId)
            {
                GameObject gameObject = Instantiate(spawnPrefabs[1], GetStartPosition().position, Quaternion.identity);
                gameObject.name = $"{spawnPrefabs[2].name} [connId={conn.connectionId}]";
                NetworkServer.AddPlayerForConnection(conn, gameObject);
            }
            else
            {
                GameObject gameObject = Instantiate(spawnPrefabs[2], GetStartPosition().position, Quaternion.identity);
                gameObject.name = $"{spawnPrefabs[2].name} [connId={conn.connectionId}]";
                NetworkServer.AddPlayerForConnection(conn, gameObject);
            }
        }
        else
        {
            GameObject gameObject = Instantiate(spawnPrefabs[0]);
            gameObject.name = $"{spawnPrefabs[0].name} [connId={conn.connectionId}]";
            NetworkServer.AddPlayerForConnection(conn, gameObject);
            _connectionsList.Add(conn);
            CSteamID steamId = SteamMatchmaking.GetLobbyMemberByIndex(SteamLobby.LobbyId, numPlayers - 1);
            var playerInfoDisplay = gameObject.GetComponent<PlayerInfoDisplay>();
            playerInfoDisplay.SetSteamId(steamId.m_SteamID);
            playerInfoDisplay.SetParentNetId(_list.GetComponent<NetworkIdentity>().netId);
        }
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        if (!clientLoadedScene)
        {
            if (!NetworkClient.ready) 
            {
                NetworkClient.Ready();
                NetworkClient.AddPlayer();
            }
        }
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        _connectionsList.Remove(conn);
        base.OnServerDisconnect(conn);
    }
    public override void OnServerSceneChanged(string sceneName)
    {
        if(sceneName == "Game")
        {
            System.Random random = new System.Random();
            int index = random.Next(_connectionsList.Count);
            _killerConnectionId = _connectionsList[index].connectionId;
            Debug.Log($"Killer Connection Id: {_killerConnectionId} - Mine connection Id: {_connectionsList[index].connectionId}");
        }
        base.OnServerSceneChanged(sceneName);
    }
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        if (!NetworkClient.ready)
        {
            NetworkClient.Ready();
            NetworkClient.AddPlayer();
            
        }
    }
    
}
