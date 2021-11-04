using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartScript : MonoBehaviour
{
    private GameObject thePlayer;
    [SerializeField]
    private GameObject thisMenu;
    [SerializeField]
    private Transform startPosition;
    [SerializeField]
    private GameObject theGameManager;

    private IEnumerator ToMoveCharacter;

    private GameManagerScript theGMScript;
    
    private Player thePlayerScript;       

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = GameObject.FindGameObjectWithTag("Player");
        thePlayerScript = thePlayer.GetComponent<Player>();
        theGMScript = theGameManager.GetComponent<GameManagerScript>();
    }

    public void RestartTheGame()
    {
        Time.timeScale = 1f;
        thisMenu.SetActive(false);
        StartCoroutine(WaitTilDist(startPosition.position));       
    }

    private IEnumerator WaitTilDist(Vector3 strPos)
    {
        bool isItThere = Vector2.Distance(thePlayer.transform.position, strPos) <= 0.1f;
        while (!isItThere)
        {
            isItThere = Vector2.Distance(thePlayer.transform.position, strPos) <= 0.1f;
            thePlayer.transform.position = Vector2.Lerp(thePlayer.transform.position, strPos, Time.deltaTime);            
            yield return null;
        }
        //theGMScript.RestartGame();        
    }
}
