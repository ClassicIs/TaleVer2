using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Проемы
public class Openings : MonoBehaviour
{
    // Класса проема
    [System.Serializable]
    public class Opening
    {
        // Расположение проема относительно центра
        public Vector2Int Pos;

        // Объект проема (стена, дверь, шторы и т.д.)
        public GameObject GameObject;
    }

    [SerializeField]
    private Opening top;

    [SerializeField]
    private Opening right;

    [SerializeField]
    private Opening bottom;

    [SerializeField]
    private Opening left;

    private List<Opening> list;

    public List<Opening> GetOpenings()
    {
        if (list == null)
        {
            list = new List<Opening>()
            {
                top,
                right,
                bottom,
                left
            };
        }

        return list;
    }
}
