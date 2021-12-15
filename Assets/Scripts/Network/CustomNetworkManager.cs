using Mirror;
using Steamworks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private Transform _list;
    private List<NetworkConnection> _connectionsList = new List<NetworkConnection>();
    private int? _killerId = null; 
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if(SceneManager.GetActiveScene().name == "Game")
        {
            GameObject gameObject = Instantiate(spawnPrefabs[2]);
            gameObject.name = $"{spawnPrefabs[2].name} [connId={conn.connectionId}]";
            NetworkServer.AddPlayerForConnection(conn, gameObject);
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
