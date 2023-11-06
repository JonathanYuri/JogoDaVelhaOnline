using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneConstants
{
    public static string GAME_LEVEL = "Game";
    public static string LOBBY_LEVEL = "Lobby";
}

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ChangeScene(string scene)
    {
        PhotonNetwork.LoadLevel(scene);
    }
}
