using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    SavePoint startCheckPoint;
    SavePoint LastCheckPoint;
    GameManagerScript TheGameManagerScript;

    private GameObject PlayerObject;
    private PlayerManager PlayerManager;
    private CharacterOverlap CharacterOverlap;
    private Player Player;

    public void AssignValues()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        PlayerManager = PlayerObject.GetComponent<PlayerManager>();
        CharacterOverlap = PlayerObject.GetComponent<CharacterOverlap>();
        Player = PlayerObject.GetComponent<Player>();
        TheGameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
        if(LastCheckPoint == null)
        {
            startCheckPoint = new SavePoint(4, 100, 0, new Vector2(PlayerObject.transform.position.x, PlayerObject.transform.position.y), PlayerManager.Inventory);
            LastCheckPoint = startCheckPoint;
            //LoadSave(true);
        }
        AssignActions();
    }

    /*public void SetLastCheckPoint()
    {

    }*/

    void AssignActions()
    {
        CharacterOverlap.OnFalling += IfFallen;
        PlayerManager.OnInkDeath += IfInkMax;
        PlayerManager.OnHealthNull += IfHealthNull;
        
    }

    private void IfHealthNull()
    {
        LoadSave(true);
    }

    private void IfFallen()
    {

        if (PlayerManager.isAlive)
        {
            LoadSave(true);
        }
    }

    private void IfInkMax()
    {
        Player.ToStun(true);
        SceneManager.LoadScene(2);
        AssignValues();
        TheGameManagerScript.AssigningValues();
    }

    public void IfEndDeath()
    {
        Player.ToStun(true);
        SceneManager.LoadScene(1);
        AssignValues();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            MakeASave();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            LoadSave(true); 
        }
    }

    public void MakeASave()
    {
        Vector2 PlayerCurrentPosition = PlayerObject.transform.position;
        int PlayerHealth, InkLevel, CointCount;
        InventoryScript PlayerInventory;

        //TODO: Implement inventory
        //TODO: Implement saving information list of collected items

        PlayerManager.GetAllValues(out PlayerHealth, out InkLevel, out CointCount, out PlayerInventory);
        if (LastCheckPoint != null)
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

    public void LoadSave(bool checkpoint)
    {
        if (checkpoint)
        {
            Debug.Log("Loading...");
            PlayerManager.SetValues(LastCheckPoint);
            LastCheckPoint.PrintCheckPoint();
        }
        else
        {
            PlayerManager.SetValues(LastCheckPoint);
            LastCheckPoint.PrintCheckPoint();
        }        
    }
}
