using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class StartGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject loading;

    GameManager gameManager;

    void Awake()
    {
        gameManager = GetComponent<GameManager>();
        loading.SetActive(true);
    }

    public void TryStartGame()
    {
        Player playerToStartTheGame = SelectARandomPlayerToStartTheGame();
        photonView.RPC("StartGame", RpcTarget.All, playerToStartTheGame.Id);
    }

    [PunRPC]
    void StartGame(int playerId)
    {
        Player[] players = FindObjectsOfType<Player>();
        gameManager.OnlinePlayer = players.First(player => player.Id != gameManager.LocalPlayer.Id);
        gameManager.ModifyTurnLocal(playerId);
        loading.SetActive(false);
    }

    Player SelectARandomPlayerToStartTheGame()
    {
        Player[] players = FindObjectsOfType<Player>();
        return players[Random.Range(0, players.Length)];
    }
}