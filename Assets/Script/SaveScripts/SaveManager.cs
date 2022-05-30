using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    SavePoint LastCheckPoint;
    GameManagerScript TheGameManagerScript;

    GameObject PlayerObject;
    PlayerManager PlayerManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        PlayerManagerScript = PlayerObject.GetComponent<PlayerManager>();
        TheGameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            MakeASave();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            LoadSave(); 
        }
    }

    public void MakeASave()
    {
        Vector2 PlayerCurrentPosition = PlayerObject.transform.position;
        int PlayerHealth, InkLevel, CointCount;
        InventoryScript PlayerInventory;
        
        //TODO: Implement inventory
        //TODO: Implement saving information list of collected items

        PlayerManagerScript.GetAllValues(out PlayerHealth, out InkLevel, out CointCount, out PlayerInventory);
        if (LastCheckPoint)
        {
            LastCheckPoint.SetCheckPoint(PlayerHealth, InkLevel, CointCount, PlayerCurrentPosition, PlayerInventory);
        }
        else
        {
            LastCheckPoint = new SavePoint(PlayerHealth, InkLevel, CointCount, PlayerCurrentPosition, PlayerInventory);
        }
        
    }

    private void SaveToFile()
    {
        //TODO
    }

    public void LoadSave()
    {
        PlayerManagerScript.SetValues(LastCheckPoint);
        LastCheckPoint.PrintCheckPoint();
        
    }
}
