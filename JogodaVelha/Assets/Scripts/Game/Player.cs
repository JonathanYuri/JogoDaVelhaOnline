using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PhotonView))]
public class Player : MonoBehaviour
{
    PhotonView photonView;

    public string Symbol { get; set; }

    public int Id { get => photonView.Owner.ActorNumber; }

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (photonView.IsMine)
        {
            Symbol = PhotonNetwork.CurrentRoom.PlayerCount == 1 ? "X" : "O";
        }
    }
}
