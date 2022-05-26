using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField]
    private GemType gemType;

    public enum GemType
    {
        Purple,
        Red,
        Blue,
    }

    public GemType GetGemType()
    {
        return gemType;
    }



    public void SetActive()
    {
        gameObject.SetActive(true);
    }

    public void SetDisable()
    {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
