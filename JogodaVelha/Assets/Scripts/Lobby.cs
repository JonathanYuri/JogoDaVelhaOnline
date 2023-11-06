using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class Lobby : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField createRoomInputField;
    [SerializeField] TMP_InputField joinRoomInputField;

    [SerializeField] TMP_Text message;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public void CreateRoom()
    {
        message.text = $"Criando a sala '{createRoomInputField.text}'...";
        PhotonNetwork.CreateRoom(createRoomInputField.text, new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
    }

    public void JoinRoom()
    {
        message.text = $"Entrando na sala '{joinRoomInputField.text}'...";
        PhotonNetwork.JoinRoom(joinRoomInputField.text);
    }

    public override void OnJoinedRoom()
    {
        GameSceneManager.Instance.ChangeScene(SceneConstants.GAME_LEVEL);
        //gameManager.LocalPlayer = player;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        this.message.text = "Erro ao entrar na sala: " + message;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        this.message.text = "Erro ao criar a sala: " + message;
    }
}
