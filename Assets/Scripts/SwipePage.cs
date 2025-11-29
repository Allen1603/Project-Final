using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipePage : MonoBehaviour
{
    public int maxPage;
    private int currentPage;
    private Vector3 targetPos;
    public Vector3 pageStep;
    public RectTransform levelPageRect;
    public float tweenTime;
    public LeanTweenType tweenType;
    public Camera cameraMovement;

    private void Awake()
    {
        currentPage = 1;
        targetPos = levelPageRect.localPosition;
    }

    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();
            MoveCamera();
        }
    }

    public void Previous()
    {
        if (currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
            MoveCamera();
        }
    }

    void MovePage()
    {
        // Moves the UI page left or right smoothly
        levelPageRect.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);
    }

    void MoveCamera()
    {
        Vector3 camPos = cameraMovement.transform.position;

        if (currentPage == 3)
            camPos.x = 8.3f;

        else if (currentPage == 2)
            camPos.x = 2.7f;

        else if (currentPage == 1)
            camPos.x = -3.2f;

        LeanTween.move(cameraMovement.gameObject, camPos, tweenTime).setEase(tweenType);
    }
}
