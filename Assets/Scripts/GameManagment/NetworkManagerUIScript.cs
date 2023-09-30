using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUIScript : MonoBehaviour
{
 

    [SerializeField]
    private Button hostBtn;

    [SerializeField]
    private Button playerBtn;
    private void Awake()
    {
        hostBtn.onClick.AddListener(() => 
        {
            NetworkManager.Singleton.StartHost();

            gameObject.SetActive(false);
        });

        playerBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();

            gameObject.SetActive(false);

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
