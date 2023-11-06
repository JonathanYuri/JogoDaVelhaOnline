using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(EndGameManager))]
[RequireComponent(typeof(StartGameManager))]
public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text[] buttonTexts;
    [SerializeField] TMP_Text turnText;

    EndGameManager endGameManager;
    StartGameManager startGameManager;

    Player localPlayer;
    Player onlinePlayer;

    int playerIdTurn;

    public Player LocalPlayer { get => localPlayer; set => localPlayer = value; }
    public Player OnlinePlayer { get => onlinePlayer; set => onlinePlayer = value; }
    public TMP_Text[] ButtonTexts { get => buttonTexts; set => buttonTexts = value; }
    public int PlayerIdTurn { get => playerIdTurn; set => playerIdTurn = value; }

    void Awake()
    {
        endGameManager = GetComponent<EndGameManager>();
        startGameManager = GetComponent<StartGameManager>();
    }

    void Start()
    {
        localPlayer = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity).GetComponent<Player>();

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            StartCoroutine(WaitForPlayersInScene());
        }
    }

    IEnumerator WaitForPlayersInScene()
    {
        Player[] players = FindObjectsOfType<Player>();
        while (players.Length != 2)
        {
            yield return null;
            players = FindObjectsOfType<Player>();
        }
        startGameManager.TryStartGame();
    }

    [PunRPC]
    void SyncButtonMessages(int idxButton, string symbol)
    {
        ModifyButtonMessageLocal(idxButton, symbol);
    }

    void ModifyButtonMessageLocal(int idxButton, string symbol)
    {
        ButtonTexts[idxButton].text = symbol;
        endGameManager.VerifyEndGame();
    }

    [PunRPC]
    void SyncPlayerTurn(int playerId)
    {
        ModifyTurnLocal(playerId);
    }

    public void ModifyTurnLocal(int playerId)
    {
        turnText.text = playerId == LocalPlayer.Id ? "Sua vez" : "Vez do oponente";

        //Debug.LogError("Sync Player Turn, new Player Turn: " + playerId);
        PlayerIdTurn = playerId;
    }
}
