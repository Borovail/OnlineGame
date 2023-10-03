
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    public static bool isInventoryOpen=false;


    [SerializeField]
    private GameObject mainInventoryHolder;
    [SerializeField]
    private Button inventoryBackgroundBtn;
    [SerializeField]
    private GameObject itemInventoryHolder;
    [SerializeField]
    private Button openCloseInventory;




   [SerializeField]
    private List<GameObject>   gameLoot;

    [SerializeField]
    private List<GameObject> inventorySlots;




    private bool isPickUpAllowed = false;

    private GameObject targetLoot;

    private GameObject selectedSlot;

    private int selectedButtonId = 0;



    private void Awake()
    {
        openCloseInventory.onClick.AddListener(()=>SetActivity(true));
        inventoryBackgroundBtn.onClick.AddListener(()=> SetActivity(false));

    }

  void  SetActivity(bool state) 
        {
        isInventoryOpen = state;

        openCloseInventory.gameObject.SetActive(!state);

        mainInventoryHolder.SetActive(state);

        itemInventoryHolder.SetActive(state);
    } 



    private void LateUpdate()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(isInventoryOpen)
            SetActivity(false);
            else SetActivity(true);

        }

        if (isPickUpAllowed && Input.GetKeyDown(KeyCode.F))
        {

            gameLoot.Add(targetLoot);

            //Destroy(targetItem);

            AddToInventory(targetLoot);
        }

        SelectItem();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(selectedSlot != null)
            {
                ThrowOutItem(selectedSlot);
            }
        }

    }

    void SelectItem()
    {

        if (EventSystem.current.currentSelectedGameObject == null) return;
        selectedSlot = null;



        if (!int.TryParse(EventSystem.current.currentSelectedGameObject.transform.parent.name, out selectedButtonId)) return;

        selectedSlot = inventorySlots[selectedButtonId]?.transform.GetChild(0).gameObject ;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GameItem")
        {
            isPickUpAllowed = true;

            targetLoot = collision.gameObject;

           

        }
        
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GameItem")
        {

            isPickUpAllowed = false;

            targetLoot = null;

        }
    }



    void ThrowOutItem(GameObject item)
    {
        if (item.tag == "Bullet") return;


        var player = GameObject.FindGameObjectWithTag("Player").transform.position;

        GameObject newObj = Instantiate(item, player, Quaternion.identity);

        var spriteRender = newObj.GetComponent<SpriteRenderer>();

        spriteRender.sprite = spriteRender.GetComponent<Image>().sprite;

        spriteRender.sortingOrder = 3;

        gameLoot.Remove(item);

        Destroy(inventorySlots[selectedButtonId].transform.GetChild(0).gameObject);

    }


    void AddToInventory(GameObject gameObject)
    {
        gameObject.transform.SetParent(inventorySlots.First(i=>i.transform.childCount==0).transform,false);
    }


    void UpdateInventory()
    {
        foreach (var inventoryItem in inventorySlots)
        {
            foreach (var gameItem in gameLoot)
            {
                inventoryItem.GetComponent<Image>().sprite = gameItem.GetComponent<SpriteRenderer>().sprite;
            }
        }
    }





}
