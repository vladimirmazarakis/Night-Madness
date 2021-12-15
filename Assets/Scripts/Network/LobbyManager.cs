using Mirror;
using UnityEngine;

public class LobbyManager : NetworkBehaviour
{
    public void StartGame()
    {
        NetworkManager.singleton.ServerChangeScene("Game");
    }
}
