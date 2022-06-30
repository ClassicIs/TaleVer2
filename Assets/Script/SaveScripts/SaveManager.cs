using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SaveManager : MonoBehaviour
{
    //SavePoint startCheckPoint;
    SavePoint lastCheckPoint;
    SavePoint limboCheckPoint;
    GameManagerScript TheGameManagerScript;

    private GameObject PlayerObject;
    private PlayerManager PlayerManager;
    private CharacterOverlap CharacterOverlap;
    private Player Player;
    private Animator PlayerAnimator;

    FadeInScript fade;

    public void AssignValues()
    {
        isAlive = true;
        Debug.LogFormat("Assigning values.");
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        PlayerManager = PlayerObject.GetComponent<PlayerManager>();
        CharacterOverlap = PlayerObject.GetComponent<CharacterOverlap>();
        Player = PlayerObject.GetComponent<Player>();
        TheGameManagerScript = GameObject.FindObjectOfType<GameManagerScript>();
        PlayerAnimator = Player.GetComponent<Animator>();
        fade = GameObject.FindObjectOfType<FadeInScript>();

        if (lastCheckPoint == null)
        {
            lastCheckPoint = new SavePoint(4, 100, 0, new Vector2(PlayerObject.transform.position.x, PlayerObject.transform.position.y), PlayerManager.Inventory);
            //LastCheckPoint = startCheckPoint;
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
        LoadSave();
    }

    private void IfFallen()
    {

        if (PlayerManager.isAlive)
        {
            LoadSave();
        }
    }
    bool isAlive;
    Action tmpAction;
    Action tmpAction2;
    private void IfInkMax()
    {
        //Debug.LogError("Ink death process");
        if (isAlive)
        {
            isAlive = false;
            Player.ToStun(true);
            PlayerAnimator.SetBool("IsDeadInk", true);
            tmpAction = null;
            tmpAction += delegate {
                fade.Fade(true);
                tmpAction2 += delegate
                {
                    PlayerAnimator.SetBool("IsDeadInk", false);
                    SceneManager.LoadScene("Death Level");
                };
                StartCoroutine(WaitTill(3f, tmpAction2));
            };
            StartCoroutine(WaitTill(4f, tmpAction));
        }
    }


    private IEnumerator WaitTill(float seconds, Action somethingToDo)
    {
        //Debug.LogError("Started waiting!");
        yield return new WaitForSeconds(seconds);
        somethingToDo?.Invoke();
        
    }

    public void IfEndDeath()
    {
        Player.ToStun(true);
        tmpAction = null;
        tmpAction += delegate { SceneManager.LoadScene(SceneManager.GetActiveScene().name); };
        StartCoroutine(WaitTill(3, tmpAction));
            
    }

    public void BackToMainLevel()
    {
        Player.ToStun(true);
        fade.Fade(true);

        tmpAction = null;
        tmpAction += delegate { SceneManager.LoadScene(1); };
        StartCoroutine(WaitTill(3, tmpAction));
    }
    /*
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
    */

    private void MakeASave(int PlayerHealth, int InkLevel, int CointCount, InventoryScript PlayerInventory, bool limbo = false)
    {
        //Transform Player = GameObject.FindObjectWithTag()
        if(limbo)            
            limboCheckPoint = new SavePoint(PlayerHealth, InkLevel, CointCount, PlayerObject.transform.position, PlayerInventory);
        else
            lastCheckPoint = new SavePoint(PlayerHealth, InkLevel, CointCount, PlayerObject.transform.position, PlayerInventory);
    }

    public void MakeASave()
    {
        Vector2 PlayerCurrentPosition = PlayerObject.transform.position;
        int PlayerHealth, InkLevel, CointCount;
        InventoryScript PlayerInventory;

        //TODO: Implement saving information list of collected items

        PlayerManager.GetAllValues(out PlayerHealth, out InkLevel, out CointCount, out PlayerInventory);
        if (lastCheckPoint != null)
        {
            lastCheckPoint.SetCheckPoint(PlayerHealth, InkLevel, CointCount, PlayerCurrentPosition, PlayerInventory);
        }
        else
        {
            lastCheckPoint = new SavePoint(PlayerHealth, InkLevel, CointCount, PlayerCurrentPosition, PlayerInventory);
        }
        
    }

    private void SaveToFile()
    {
        //TODO
    }

    public void LoadSave()
    {
        if (SceneManager.GetActiveScene().name == "Death Level")
        {
            MakeASave(4, 100, 0, new InventoryScript(10), true);
            PlayerManager.SetValues(limboCheckPoint);
            limboCheckPoint.PrintCheckPoint();
            return;
        }

        PlayerManager.SetValues(lastCheckPoint);
        lastCheckPoint.PrintCheckPoint();
    }
}
