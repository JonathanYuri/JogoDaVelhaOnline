using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
[RequireComponent(typeof(PhotonView))]
public class EventClickManager : MonoBehaviourPunCallbacks
{
    GameManager gameManager;

    void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    // player local clicou
    public void OnButtonClicked(int idxButton)
    {
        if (!ToPlayLogic(idxButton, gameManager.LocalPlayer.Id))
        {
            return;
        }

        photonView.RPC("SyncButtonMessages", RpcTarget.All, idxButton, gameManager.LocalPlayer.Symbol);
        photonView.RPC("SyncPlayerTurn", RpcTarget.All, gameManager.OnlinePlayer.Id);
    }

    bool ToPlayLogic(int idxButton, int playerIdTurn)
    {
        if (!PhotonNetwork.InRoom)
        {
            return false;
        }
        if (PhotonNetwork.CurrentRoom.PlayerCount != 2)
        {
            return false;
        }
        if (!IsLocalPlayerTurn(playerIdTurn))
        {
            return false;
        }

        if (gameManager.ButtonTexts[idxButton].text != "")
        {
            return false;
        }

        return true;
    }

    bool IsLocalPlayerTurn(int playerIdTurn)
    {
        if (playerIdTurn != gameManager.PlayerIdTurn)
        {
            return false;
        }

        return true;
    }
}
