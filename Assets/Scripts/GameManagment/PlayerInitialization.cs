using Assets.Scripts.GameManagment;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInitialization : MonoBehaviour 
{
    public static PlayerInitialization Instance;


    [SerializeField] InputField playerNicknameHolder;
    [SerializeField] Text ErrorNicknameNotification;


     bool isHost = true;

    [SerializeField]   GameObject player;




    private void Start()
    {
        playerNicknameHolder.onValueChanged.AddListener((value)=> ErrorNicknameNotification.gameObject.SetActive(false));
       
    }


    public void StartGame()
    {
        if (!CheckNickName()) return;
        GameManager.Instance.InitializePlayer(player, isHost);
    }

    public void SetHost(bool state)
    {
        isHost = state;
    }
    
    private bool CheckNickName()
    {
        if (string.IsNullOrWhiteSpace(playerNicknameHolder.text))
        {
            ErrorNicknameNotification.gameObject.SetActive(true);

            return false;
        }
        else
        {
            player.name = playerNicknameHolder.text;

            return true;
        }
    }

   
}
