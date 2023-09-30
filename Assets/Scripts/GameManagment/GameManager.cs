using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


    public static GameManager Instance;

    [HideInInspector]
    public GameObject Player;

    private void Awake()
    {
     

        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

    }

   
   public void EnterTheScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Instantiate(Player, GetRandomSpawnPosition(),Quaternion.identity);
    }




   public int GetRandomId() => Random.Range(1, 6);

    Vector2 GetRandomSpawnPosition()
    {
        var tempHolder = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponentsInChildren<Transform>();

        int index = Random.Range(0, tempHolder.Length - 1);

        return tempHolder[index].position;
    }


}
