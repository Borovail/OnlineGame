using Assets.Scripts.GameManagment;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


    public static GameManager Instance;


    private ulong clientId;

    private GameObject player;




    [SerializeField] AnimationClip[] defaultAnimations;

    AnimationClip[] newAnimations;

    private void Start()
    {
     

        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);


        NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
        NetworkManager.Singleton.OnServerStarted += Singleton_OnServerStarted;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void Singleton_OnServerStarted()
    {
        clientId = NetworkManager.Singleton.LocalClientId;
    }

    private void Singleton_OnClientConnectedCallback(ulong obj)
    {
        clientId = obj;
    }

    public void InitializePlayer(GameObject player,bool isHost)
    {
        this.player = player;


        if (isHost == true)
        {
            NetworkManager.Singleton.StartHost();

        }
        else
        {
            NetworkManager.Singleton.StartClient();
        }

        SceneManager.LoadScene("1");
    }


    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        var pl = Instantiate(player, GetRandomSpawnPosition(), Quaternion.identity);

        pl.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);

        InitializeSkin(pl.GetComponent<Animator>());
    }

    public  void InitializeSkin(Animator animator)
    {
        Debug.Log("Gender:   " + PlayerPrefs.GetString("Gender"));
        Debug.Log("SkinId:    " + PlayerPrefs.GetString("SkinId"));

        newAnimations = SkinLoader.GetAnimations(PlayerPrefs.GetString("Gender"), PlayerPrefs.GetString("SkinId"));

        ReplaceAnimationClip(animator);
    }

    private  void ReplaceAnimationClip(Animator animator)
    {



        if (animator.runtimeAnimatorController != null)
        {
            AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

            for (int i = 0; i < 7; i++)
            {
                overrideController[defaultAnimations[i]] = newAnimations[i];
            }

            animator.runtimeAnimatorController = overrideController;
        }
        else
        {
            Debug.LogError("Animator Controller is not set!");
        }
    }











    public int GetRandomId() => Random.Range(1, 6);

   public Vector2 GetRandomSpawnPosition()
    {
        var tempHolder = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponentsInChildren<Transform>();

        int index = Random.Range(0, tempHolder.Length - 1);

        return tempHolder[index].position;
    }


}
