using Mirror;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerInfoDisplay : NetworkBehaviour
{
    [SyncVar(hook = nameof(HandleSteamIdUpdated))]
    private ulong _steamId;
    [SyncVar(hook = nameof(HandleParentNetIdUpdated))]
    private uint _parentNetId;
    [SerializeField] private RawImage _avatar = null;
    [SerializeField] private TMP_Text _nickname = null;
    private Callback<AvatarImageLoaded_t> _avatarImageLoaded;
    public override void OnStartClient()
    {
        base.OnStartClient();
        _avatarImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnAvatarImageLoaded);
    }
    #region Server
    public void SetSteamId(ulong steamId)
    {
        this._steamId = steamId;
    }
    public void SetParentNetId(uint netId)
    {
        _parentNetId = netId;
    }
    #endregion
    #region Client
    private void HandleSteamIdUpdated(ulong oldSteamId, ulong newSteamId)
    {
        var cSteamId = new CSteamID(newSteamId);
        Debug.Log(newSteamId);
        _nickname.text = SteamFriends.GetFriendPersonaName(cSteamId);
        int imageId = SteamFriends.GetLargeFriendAvatar(cSteamId);
        if(imageId == -1)
        {
            return;
        }
        _avatar.texture = GetSteamImageAsTexture(imageId);
    }
    private void HandleParentNetIdUpdated(uint oldParentNetId, uint newParentNetId)
    {
        GameObject parent = GameObject.FindObjectsOfType<NetworkIdentity>().FirstOrDefault(id => id.netId == newParentNetId).gameObject;
        transform.SetParent(parent.transform, false);
    }
    private Texture2D GetSteamImageAsTexture(int imageId)
    {
        Texture2D texture = null;
        bool IsValid = SteamUtils.GetImageSize(imageId, out uint width, out uint height);
        if (IsValid)
        {
            byte[] imageByteArray = new byte[width * height * 4];
            IsValid = SteamUtils.GetImageRGBA(imageId, imageByteArray, (int)(width * height * 4));
            if (IsValid)
            {
                texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                texture.LoadRawTextureData(imageByteArray);
                texture.Apply();
            }
        }
        return texture;
    }
    private void OnAvatarImageLoaded(AvatarImageLoaded_t callback)
    {
        if(callback.m_steamID.m_SteamID != _steamId)
        {
            return;
        }
        _avatar.texture = GetSteamImageAsTexture(callback.m_iImage);
    }
    #endregion
    public void ChangeParent(Transform parent)
    {
        this.transform.parent = parent;
    }
}
