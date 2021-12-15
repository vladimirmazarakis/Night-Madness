using Mirror;
using Steamworks;
using UnityEngine;

public class SteamLobby : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _lobby;
    [SerializeField] private GameObject _startGameButton;
    private CustomNetworkManager _networkManager;
    private Callback<LobbyCreated_t> _lobbyCreated;
    private Callback<GameLobbyJoinRequested_t> _gameLobbyJoinRequested;
    private Callback<LobbyEnter_t> _lobbyEntered;
    private const string _hostAddressKey = "HostAddress";

    private void Start()
    {
        _networkManager = GetComponent<CustomNetworkManager>();
        if (!SteamManager.Initialized)
        {
            Application.Quit();
            return;
        }
        _lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        _gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        _lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }
    #region Lobby hosting.
    public void HostLobby()
    {
        _menu.SetActive(false);
        _lobby.SetActive(true);
        _startGameButton.SetActive(true);
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, _networkManager.maxConnections);
    }
    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if(callback.m_eResult != EResult.k_EResultOK)
        {
            _menu.SetActive(true);
            _lobby.SetActive(false);
            return;
        }
        LobbyId = new CSteamID(callback.m_ulSteamIDLobby);

        SteamMatchmaking.SetLobbyData(LobbyId,
           _hostAddressKey, 
            SteamUser.GetSteamID().ToString());
        _networkManager.StartHost();
    }
    #endregion
    #region Lobby joining.
    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }
    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        if (NetworkServer.active)
        {
            return;
        }
        string lobbyId = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), _hostAddressKey);
        _networkManager.networkAddress = lobbyId;
        _networkManager.StartClient();
        _menu.SetActive(false);
        _lobby.SetActive(true);
    }
    #endregion
    void Update()
    {
        
    }

    public static CSteamID LobbyId { get; private set; }
}
