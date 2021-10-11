using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class isQtePassed : MonoBehaviour
{
    public float fillImage = 0;
    public float timePassed = 0;
    public bool qteSuccess = false;
    public bool qteActive = false;

    Image QTECircle;
    Image UnderCircle;
    Image RButton;

    [SerializeField]
    GameObject QTEUnderCircle;

    [SerializeField]
    GameObject R_Button;

    [SerializeField]
    Lever lev;

    //[SerializeField]
    //BearTrap trap;

    [SerializeField]
    float countDown = 1;

    // Start is called before the first frame update
    void Start()
    {
        QTECircle = GetComponent<Image>();
        UnderCircle = QTEUnderCircle.GetComponent<Image>();
        RButton = R_Button.GetComponent<Image>();

        QTEUnderCircle.SetActive(false);
        R_Button.SetActive(false);
        countDown = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (qteActive == true)
        {
            QTESuccess();
        }
    }

    public void QTESuccess()
    {
        gameObject.SetActive(true);
        QTEUnderCircle.SetActive(true);
        R_Button.SetActive(true);

        if (Input.GetKeyDown(KeyCode.R))
        {
            fillImage += .2f;
            Debug.Log("R is pressed");
        }

        timePassed += Time.deltaTime;

        if (timePassed > .05)
        {
            timePassed = 0;
            fillImage -= .02f;
           Debug.Log("AFK");
        }

        QTECircle.fillAmount = fillImage;

        if (fillImage < 0 && countDown <= 0)
        {
            fillImage = 0;

            countDown = 1;

            qteSuccess = false;

            qteActive = false;

            gameObject.SetActive(false);
            QTEUnderCircle.SetActive(false);
            R_Button.SetActive(false);
            Debug.Log("not passed");
        }

        if (countDown > 0)
        {
            countDown -= .5f * Time.deltaTime;
        }

        if (fillImage >= 1)
        {
            qteSuccess = true;
            lev.OpenDoor();

            qteActive = false;

            gameObject.SetActive(false);
            QTEUnderCircle.SetActive(false);
            R_Button.SetActive(false);
            Debug.Log("passed");
        }
    }
}