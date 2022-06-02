using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartScript : MonoBehaviour
{
    private GameObject player;
    [SerializeField]
    private GameObject thisMenu;
    [SerializeField]
    private Transform startPosition;
  

    private IEnumerator ToMoveCharacter;

    private GameManagerScript GameManagerScript;
    
    private CharacterOverlap CharacterOverlap;
    private PlayerManager PlayerManager;
    private Player Player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        CharacterOverlap = player.GetComponent<CharacterOverlap>();
        PlayerManager = player.GetComponent<PlayerManager>();
        Player = player.GetComponent<Player>();

        GameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();

        //CharacterOverlap.OnFalling += IfFallen;
        //PlayerManager.OnInkDeath += IfInkMax;
    }

    public void RestartTheGame()
    {
        Time.timeScale = 1f;
        thisMenu.SetActive(false);
        StartCoroutine(WaitTilDist(startPosition.position));       
    }

    private IEnumerator WaitTilDist(Vector3 strPos)
    {
        bool isItThere;
        do
        {
            isItThere = Vector2.Distance(player.transform.position, strPos) <= 0.1f;
            player.transform.position = Vector2.Lerp(player.transform.position, strPos, Time.deltaTime);
            yield return null;
        }
        while (!isItThere);
    }


    
}
