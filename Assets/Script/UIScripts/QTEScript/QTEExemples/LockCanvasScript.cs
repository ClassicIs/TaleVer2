using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockCanvasScript : QTEObject
{
    [SerializeField]
    private GameObject thePoint;
    [SerializeField]
    private GameObject thePlayer;
    private RectTransform thePointTransform;
    private RectTransform thePlayerTransform;

    private Vector3 theRotation;

    [SerializeField]
    int countToWin;
    [SerializeField]
    int countToFail;

    float posOfPl;
    int dirOfRot;
    
    bool hasEnd;

    [SerializeField]
    int tail;
    float SpeedOfCircle;

    void Start()
    {
        thePointTransform = thePoint.GetComponent<RectTransform>();
        thePlayerTransform = thePlayer.GetComponent<RectTransform>();
        TurnSprites(false);
    }

    public override void Activate(HardVariety Hardness)
    {
        switch (Hardness)
        {
            case HardVariety.easy:
                countToWin = 3;
                tail = 3;
                SpeedOfCircle = 0.5f;
                countToFail = 5;
                break;
            case HardVariety.normal:
                countToWin = 4;
                tail = 2;
                SpeedOfCircle = 0.8f;
                countToFail = 4;
                break;
            case HardVariety.hard:
                tail = 0;
                countToWin = 5;
                SpeedOfCircle = 1.0f;
                countToFail = 3;
                break;
            default:
                Debug.LogError("Something wrong with your hardness");
                break;
        }

        theRotation = new Vector3(0f, 0f, 0f);
        posOfPl = 0;
        dirOfRot = 1;
        hasEnd = false;

        TurnSprites(true);
        StartCoroutine(TheCapsule());
    }

    protected override void QTEEnd()
    {
        thePlayerTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        thePointTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        TurnSprites(false);
    }

    void TurnSprites(bool OnOrOff)
    {
        Image[] theSprites = gameObject.GetComponentsInChildren<Image>();
        foreach (Image theSpriteTMP in theSprites)
        {
            theSpriteTMP.enabled = OnOrOff;
        }
    }

    IEnumerator TheCapsule()
    {
        while (!hasEnd)
        {
            if (Mathf.Abs(posOfPl) >= 360)
            {
                posOfPl = 0;
            }
            posOfPl += SpeedOfCircle * dirOfRot;

            thePlayerTransform.rotation = Quaternion.Euler(0, 0, posOfPl);
            if (Input.GetKeyDown(KeyCode.K))
            {
                int currPlayerRot = Mathf.RoundToInt(Mathf.Rad2Deg * thePlayerTransform.rotation.z);
                
                int currCapsuleRot = Mathf.RoundToInt(Mathf.Rad2Deg * thePointTransform.rotation.z);
                if ((Mathf.Abs(currPlayerRot) <= (currCapsuleRot + tail)) && (Mathf.Abs(currPlayerRot) >= (currCapsuleRot - tail)))
                {
                    Debug.Log("Lock Success!");
                    countToWin--;
                    
                    if (countToWin == 0)
                    {
                        Success();
                        hasEnd = true;
                    }
                }
                else
                {
                    Debug.Log("Lock Fail!");
                    countToFail--;                    
                    if (countToFail == 0)
                    {
                        Failed();                        
                        hasEnd = true;
                    }
                }

                if (!hasEnd)
                {
                    theRotation.z = Random.Range(0, 361);
                    dirOfRot *= -1;
                    thePointTransform.rotation = Quaternion.Euler(theRotation);
                }

            }
            yield return null;
        }
        QTEEnd();
    }
    protected override void Failed()
    {
        Debug.Log("Failed to open the lock!");
        base.Failed();

    }

    protected override void Success()
    {
        Debug.Log("Successfully openned the lock!");
        base.Success();
    }
}
