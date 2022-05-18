using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsGenerator : MonoBehaviour
{
    [SerializeField]
    private Room roomPrefab;

    private int roomCount = 8;
    //private bool trapsGenerationEnabled = true;
    [SerializeField]
    private int countOfTrapEvents = 0;

    [SerializeField]
    private int maxCountOfTrapEvents = 3;

    private IEnumerator Generate()
    {
        Graph graph = new Graph();
        var infos = graph.Generate2(roomCount);

        int i = 0;
        bool skipEvent = false;

        foreach (var info in infos)
        {
            // спавн комнат
            var room = Instantiate(roomPrefab, new Vector3(info.Pos.x * 18f, info.Pos.y * 14f), Quaternion.identity);
            room.Setup(info.Dirs);
            
            // проверка на спавн темплейтов ловушек
            int chanceToSpawnTrapEvent = Random.Range(0, 10);
            if (i > 0 && i != roomCount - 1 && countOfTrapEvents < maxCountOfTrapEvents)
            {
                if (skipEvent == true)
                {
                    chanceToSpawnTrapEvent = 0;
                    skipEvent = false;
                }

                if (chanceToSpawnTrapEvent >= 3 && skipEvent == false)
                {
                    room.ActivateTrapEvent();
                    countOfTrapEvents += 1;

                    skipEvent = true;
                }

                // если настала очередь предпоследней комнаты перед финалом
                if (i == roomCount - 3 && countOfTrapEvents < maxCountOfTrapEvents)
                {
                    // если не хватает 2 ивента
                    if (countOfTrapEvents + 2 == maxCountOfTrapEvents)
                    {
                        // ставим ивент
                        room.ActivateTrapEvent();
                        countOfTrapEvents += 1;
                    }
                }

                // если не хватает только одного ивента
                if (i == roomCount - 2 && countOfTrapEvents + 1 == maxCountOfTrapEvents)
                {
                    room.ActivateTrapEvent();
                    countOfTrapEvents += 1;
                }
            }

            // проверка на спавн фонтана
            if (i == roomCount - 1)
            {
                room.ActivateFountain();
            }

            else
            {
                room.DestroyFountain();
            }

            i++;


            yield return 0;
        }
    }

    void Start()
    {
        StartCoroutine(Generate());
    }

}
