using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PageManager : MonoBehaviour
{
    public Transform pagesParent;
    public float rotationDuration = 1.0f;
    private Transform[] pages;
    private int currentPageIndex = 0;
    private bool isRotating = false;

    public Transform BeigeBackgroundForPageLeft;
    public Transform BeigeBackgroundForPageRight;
   
    
    private void UpdatePageState(string direction)
    {
        if (currentPageIndex == 0 || currentPageIndex == 1 && direction == "backward")
        {
            BeigeBackgroundForPageLeft.gameObject.SetActive(false);
            BeigeBackgroundForPageRight.gameObject.SetActive(true);
        }
        else if (currentPageIndex > 0 && direction == "forward" && currentPageIndex < pages.Length -2 || currentPageIndex > 1 && direction == "backward" && currentPageIndex < pages.Length - 1)
        {
            BeigeBackgroundForPageLeft.gameObject.SetActive(true);
            BeigeBackgroundForPageRight.gameObject.SetActive(true);
        }

        else if (currentPageIndex == pages.Length)
        {
            BeigeBackgroundForPageLeft.gameObject.SetActive(true);
            BeigeBackgroundForPageRight.gameObject.SetActive(false);
        }
        else if (currentPageIndex == pages.Length - 2 && direction == "forward") // methode könnte kritisch werden, wennn die letzte Seite erreicht wird. Zurzeit gibt es 5 seiten und die 5 wird nicht angezeigt
         {
            BeigeBackgroundForPageLeft.gameObject.SetActive(true);
            BeigeBackgroundForPageRight.gameObject.SetActive(false);
        }
    
    }
   


    private void Start()
    {   BeigeBackgroundForPageLeft.gameObject.SetActive(false);
    BeigeBackgroundForPageRight.gameObject.SetActive(true);
        int count = pagesParent.childCount;
        pages = new Transform[count];

        for (int i = 0; i < count; i++)
        {
            pages[i] = pagesParent.GetChild(i);
        }
        Debug.Log("Pages initialized: " + pages.Length);
        
    }

    public void FlipForward()
    {

        if (!isRotating && currentPageIndex < pages.Length)
        {
            Debug.Log("Starting Flip Forward in Manager");
            StartCoroutine(FlipPagesSimultaneously("forward"));
        }
    }

    public void FlipBackward()
    {

        if (!isRotating && currentPageIndex > 0)
        {
            Debug.Log("Starting Flip Backward in Manager");
            StartCoroutine(FlipPagesSimultaneously("backward"));
        }
    }

    

    private IEnumerator FlipPagesSimultaneously(string direction)
    {
        Debug.Log("Flip Pages Simultaneously in Manager in direction: " + direction);
        isRotating = true;
        UpdatePageState(direction);
        List<Coroutine> rotations = new List<Coroutine>();
        int nextPageIndex = currentPageIndex + 1;
        int prevPageIndex = currentPageIndex - 1;
        
        if (direction == "forward")
        {
            Debug.Log("Current Page Index - forward: " + currentPageIndex);

            if (currentPageIndex >= 0 && currentPageIndex < pages.Length - 1 && pages[currentPageIndex] != null && pages[nextPageIndex] != null)
            {
                rotations.Add(StartCoroutine(AnimatePagePosition(pages[currentPageIndex], 0.005f)));
                rotations.Add(StartCoroutine(AnimatePagePosition(pages[nextPageIndex], 0.0049f)));
                rotations.Add(StartCoroutine(RotatePage(pages[currentPageIndex], 180f)));
                rotations.Add(StartCoroutine(RotatePage(pages[nextPageIndex], 180f)));
                rotations.Add(StartCoroutine(AnimatePagePosition(pages[currentPageIndex], -0.5f)));
                rotations.Add(StartCoroutine(AnimatePagePosition(pages[nextPageIndex], 0.05f)));
            }
            else
            {
                Debug.LogWarning("Current page index is out of bounds or page is null.");
            }

            foreach (Coroutine rot in rotations)
            {
                yield return rot;
            }

            currentPageIndex++;
            Debug.Log("Updated Page Index - forward: " + currentPageIndex);
        }
        else if (direction == "backward")
        {
            Debug.Log("Current Page Index - backward: " + currentPageIndex);

            if (currentPageIndex > 0 && currentPageIndex < pages.Length && pages[currentPageIndex] != null && pages[prevPageIndex] != null)
            {
                rotations.Add(StartCoroutine(AnimatePagePosition(pages[currentPageIndex], 0.005f)));
                rotations.Add(StartCoroutine(AnimatePagePosition(pages[prevPageIndex], 0.0049f)));
                rotations.Add(StartCoroutine(RotatePage(pages[currentPageIndex], -180f)));
                rotations.Add(StartCoroutine(RotatePage(pages[prevPageIndex], -180f)));
                rotations.Add(StartCoroutine(AnimatePagePosition(pages[currentPageIndex], -0.5f)));
                rotations.Add(StartCoroutine(AnimatePagePosition(pages[prevPageIndex], 0.05f)));
            }
            else
            {
                Debug.LogWarning("Current page index is out of bounds or page is null.");
            }

            foreach (Coroutine rot in rotations)
            {
                yield return rot;
            }

            currentPageIndex--;
            Debug.Log("Updated Page Index - backward: " + currentPageIndex);
        }
        
        isRotating = false;
    }

    private IEnumerator RotatePage(Transform page, float angle)
    {
        Quaternion startRot = page.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(angle, 0f, 0f);

        float elapsed = 0f;

        while (elapsed < rotationDuration)
        {
            page.rotation = Quaternion.Slerp(startRot, endRot, elapsed / rotationDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        page.rotation = endRot;
    }
    private IEnumerator AnimatePagePosition(Transform page, float targetYOffset)
    {
        Vector3 startPos = page.localPosition;
        Vector3 endPos = new Vector3(startPos.x, targetYOffset, startPos.z);

        float elapsed = 0f;

        while (elapsed < rotationDuration)
        {
            page.localPosition = Vector3.Lerp(startPos, endPos, elapsed / rotationDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        page.localPosition = endPos;
    }


}