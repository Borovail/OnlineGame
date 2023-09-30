using Assets.Scripts.GameManagment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInitialization : MonoBehaviour
{
    [SerializeField]
    private InputField nicknameInput;

    [SerializeField]
    private Text errorNickname;

    [SerializeField]
    private Button playButton;









    [SerializeField]
    GameObject player;



  
    AnimationClip[] newAnimations;




     Animator animator;

    [SerializeField]
    AnimationClip[] defaultAnimations;


    private void Awake()
    {
        playButton.onClick.AddListener((CheckNicknameErrors));
        nicknameInput.onValueChange.AddListener(nickNameChanged);
    }



    void CheckNicknameErrors()
    {
        if (string.IsNullOrWhiteSpace(nicknameInput.text))
        {
            errorNickname.gameObject.SetActive(true);

        }
        else
        {
            InitializeSkin();

            GameManager.Instance.Player = player;

            EnterTheGame();
        }
    }

    void EnterTheGame()
    {
        string name = GameManager.Instance.GetRandomId().ToString();

        GameManager.Instance.EnterTheScene(name);
    }


    void nickNameChanged(string text)
    {
        errorNickname.gameObject.SetActive(false);
    }


    public void InitializeSkin ()
    {
        newAnimations = SkinLoader.GetAnimations(PlayerPrefs.GetString("Gender"), PlayerPrefs.GetString("SkinId"));

        ReplaceAnimationClip();
    }

     void ReplaceAnimationClip()
    {
        animator = player.GetComponent<Animator>();

        AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

        for (int i = 0; i <7; i++)
        {
            overrideController[defaultAnimations[i]] = newAnimations[i];
        }

       animator.runtimeAnimatorController = overrideController;

    }
}
