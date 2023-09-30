using Assets.Scripts.GameManagment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SkinSelectionScript : MonoBehaviour
{
    [SerializeField]
    GameObject obj;


    [SerializeField]
    Toggle maleCheck;

    [SerializeField]
    Toggle femaleCheck;


    [SerializeField]
    Button nextSkin;

    [SerializeField]
    Button previousSkin;

    [SerializeField]
    List<Sprite> maleSprites = new List<Sprite>();
    [SerializeField]
    List<Sprite> femaleSprites = new List<Sprite>();


    [SerializeField]
    Image[] carouselImages;

    int currentSkinIndex = 1;


   

    string gender;

    private void Awake()
    {
        LoadSkins();

       
        AddInvents();



    }

   

     void AddInvents()
    {
        foreach (var image in carouselImages)
        {
            image.GetComponent<Button>().onClick.AddListener(() => { PlayerPrefs.SetString("SkinId", image.sprite.name); });
        }

        maleCheck.onValueChanged.AddListener((value) =>
        {
            if (value)
            {
                gender = "male";

                PlayerPrefs.SetString("Gender", gender);

                LoadBaseImages();
            }
            else
                gender = string.Empty;
        });

        femaleCheck.onValueChanged.AddListener((value) =>
        {
            if (value)
            {
                gender = "female";

                PlayerPrefs.SetString("Gender", gender);

                LoadBaseImages();
            }
            else
            {
                gender = string.Empty;
            }
        });


        nextSkin.onClick.AddListener(ScrollToNextSkin);
        previousSkin.onClick.AddListener(ScrollToPreviousSkin);

    }



    void LoadBaseImages()
    {

        Debug.Log("LoadBaseImages");

        for (int i = 0; i <3; i++)
        {
            if (gender == "male")
            {
                carouselImages[i].sprite = maleSprites[i];
            }
            if (gender == "female")
            {
                carouselImages[i].sprite = femaleSprites[i];

            }
        }

        obj.SetActive(true);
    }


     void LoadSkins()
    {
        Debug.Log("LoadSkins");

       List <SpriteHolder> maleSkins = new List<SpriteHolder> ();
        List<SpriteHolder> femaleSkins = new List<SpriteHolder> ();


         SkinLoader.UploadSkins(maleSkins, femaleSkins);

        for (int i = 0; i < maleSkins.Count; i++)
        {
            maleSkins[i].sprites[0][0].name = (i+1).ToString();
            maleSprites.Add(maleSkins[i].sprites[0][0]);

            femaleSkins[i].sprites[0][0].name = (i+1).ToString();
            femaleSprites.Add(femaleSkins[i].sprites[0][0]);
        }
  
    }


    void ScrollToNextSkin()
    {
        Debug.Log("ScrollToNextSkin");
        if (currentSkinIndex < maleSprites.Count - 2)
        {
            currentSkinIndex++;
            var sprites = CheckScroll(currentSkinIndex);
            UpdateCarouselImages(sprites);
        }
    }
    void ScrollToPreviousSkin()
    {
        if (currentSkinIndex > 1)
        {
            currentSkinIndex--;
            var sprites = CheckScroll(currentSkinIndex);
            UpdateCarouselImages(sprites);
        }
    }

    private void UpdateCarouselImages(List<Sprite> sprites)
    {

        Debug.Log("UpdateCarouselImages");
        carouselImages[0].sprite = sprites[currentSkinIndex - 1];
        carouselImages[1].sprite = sprites[currentSkinIndex];
        carouselImages[2].sprite = sprites[currentSkinIndex + 1];
    }

    List<Sprite> CheckScroll(int index)
    {

        Debug.Log("CheckScroll");
        if (index < 0 || index > 30) return null;

        if (gender == "male")
           return maleSprites;
        else
           return femaleSprites;

    }


   


   
}
