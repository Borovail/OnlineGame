using Assets.Scripts.GunFolder;
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

    [SerializeField]
    private List<GameObject>   gameItems;

    [SerializeField]
    private List<GameObject> inventoryItems;


    [SerializeField]
    GameObject inventoryItem;



    private bool isPickUpAllowed = false;

    private GameObject targetItem;

    private GameObject selectedItem;

    private int selectedButtonId = 0;


    private void LateUpdate()
    {
        if (isPickUpAllowed && Input.GetKeyDown(KeyCode.E))
        {

            gameItems.Add(targetItem);

            //Destroy(targetItem);

            AddToInventory(targetItem);
        }

        SelectItem();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(selectedItem != null)
            {
                ThrowOutItem(selectedItem);
            }
        }

    }

    void SelectItem()
    {

        if (EventSystem.current.currentSelectedGameObject == null) return;
        selectedItem = null;



        if (!int.TryParse(EventSystem.current.currentSelectedGameObject.transform.parent.name, out selectedButtonId)) return;

        selectedItem = gameItems[selectedButtonId];
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GameItem")
        {
            isPickUpAllowed = true;

            targetItem = collision.gameObject;

           

        }
        
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GameItem")
        {

            isPickUpAllowed = false;

            targetItem = null;

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

        gameItems.Remove(item);

        Destroy(inventoryItems[selectedButtonId].transform.GetChild(0).gameObject);

    }


    void AddToInventory(GameObject gameObject)
    {
        gameObject.transform.SetParent(inventoryItems.First(i=>i.transform.childCount==0).transform,false);
    }


    void UpdateInventory()
    {
        foreach (var inventoryItem in inventoryItems)
        {
            foreach (var gameItem in gameItems)
            {
                inventoryItem.GetComponent<Image>().sprite = gameItem.GetComponent<SpriteRenderer>().sprite;
            }
        }
    }





}
