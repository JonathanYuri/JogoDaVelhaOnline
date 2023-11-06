using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(GameManager))]
public class EndGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject endGame;
    [SerializeField] TMP_Text endGameText;

    GameManager gameManager;

    [SerializeField] float toGoLobbyTime = 2f;

    void Awake()
    {
        gameManager = GetComponent<GameManager>();
        endGame.SetActive(false);
    }

    public void VerifyEndGame()
    {
        string winner = CheckWinner();
        if (winner != "")
        {
            photonView.RPC("SyncEndGame", RpcTarget.All, winner);
            return;
        }

        bool allClicked = gameManager.ButtonTexts.All(buttonText => buttonText.text != "");
        if (allClicked)
        {
            // acabou em empate
            photonView.RPC("SyncEndGame", RpcTarget.All, "");
            return;
        }
    }

    string CheckWinner()
    {
        // Verifica linhas
        for (int i = 0; i < 3; i++)
        {
            if (gameManager.ButtonTexts[i].text == "X" && gameManager.ButtonTexts[i + 3].text == "X" && gameManager.ButtonTexts[i + 6].text == "X")
            {
                return "X";
            }
            if (gameManager.ButtonTexts[i].text == "O" && gameManager.ButtonTexts[i + 3].text == "O" && gameManager.ButtonTexts[i + 6].text == "O")
            {
                return "O";
            }
        }

        // Verifica colunas
        for (int i = 0; i < 7; i += 3)
        {
            if (gameManager.ButtonTexts[i].text == "X" && gameManager.ButtonTexts[i + 1].text == "X" && gameManager.ButtonTexts[i + 2].text == "X")
            {
                return "X";
            }
            if (gameManager.ButtonTexts[i].text == "O" && gameManager.ButtonTexts[i + 1].text == "O" && gameManager.ButtonTexts[i + 2].text == "O")
            {
                return "O";
            }
        }

        // Verifica diagonais
        if ((gameManager.ButtonTexts[0].text == "X" && gameManager.ButtonTexts[4].text == "X" && gameManager.ButtonTexts[8].text == "X") ||
            (gameManager.ButtonTexts[2].text == "X" && gameManager.ButtonTexts[4].text == "X" && gameManager.ButtonTexts[6].text == "X"))
        {
            return "X";
        }
        if ((gameManager.ButtonTexts[0].text == "O" && gameManager.ButtonTexts[4].text == "O" && gameManager.ButtonTexts[8].text == "O") ||
            (gameManager.ButtonTexts[2].text == "O" && gameManager.ButtonTexts[4].text == "O" && gameManager.ButtonTexts[6].text == "O"))
        {
            return "O";
        }

        return "";
    }

    [PunRPC]
    void SyncEndGame(string winner)
    {
        endGame.SetActive(true);
        if (winner == "")
        {
            endGameText.text = "Empate";
        }
        else if (gameManager.LocalPlayer.Symbol == winner)
        {
            endGameText.text = "Você ganhou";
        }
        else
        {
            endGameText.text = "Você perdeu";
        }

        StartCoroutine(GoToLobby());
    }

    IEnumerator GoToLobby()
    {
        yield return new WaitForSeconds(toGoLobbyTime);
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom && PhotonNetwork.NetworkClientState != ClientState.Leaving)
        {
            PhotonNetwork.LeaveRoom();
            GameSceneManager.Instance.ChangeScene(SceneConstants.LOBBY_LEVEL);
        }
    }
}
