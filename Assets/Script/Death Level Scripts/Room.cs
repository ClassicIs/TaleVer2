using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private Openings doors;

    [SerializeField]
    private Openings walls;

    [SerializeField]
    private GameObject fountain;

    [SerializeField]
    private GameObject trapEvent;

    public void Setup(List<Vector2Int> config)   // передаем по каким направлениям есть проходы
    {
        foreach (var wall in walls.GetOpenings())
        {

            bool contains = config.Contains(wall.Pos);
            wall.GameObject.SetActive(!contains);

        }
        
        foreach (var door in doors.GetOpenings())
        {
            bool contains = config.Contains(door.Pos);
            door.GameObject.SetActive(contains);
        }
        
    }

    public void ActivateFountain()
    {
        fountain.SetActive(true);
    }

    public void DestroyFountain()
    {
        Destroy(fountain);
    }

    public void ActivateTrapEvent()
    {
        trapEvent.SetActive(true);
    }
}
