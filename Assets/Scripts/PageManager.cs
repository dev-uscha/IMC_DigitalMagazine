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

    private void Start()
    {
        int count = pagesParent.childCount;
        pages = new Transform[count];

        for (int i = 0; i < count; i++)
        {
            pages[i] = pagesParent.GetChild(i);
        }
    }

    public void FlipForward()
    {
        Debug.Log("Flip Forward in Manager");

        if (!isRotating && currentPageIndex < pages.Length)
        {
            Debug.Log("Starting Flip Forward in Manager");
            StartCoroutine(FlipPagesSimultaneously("forward"));
        }
    }

    public void FlipBackward()
    {
        Debug.Log("Flip Backward in Manager");

        if (!isRotating && currentPageIndex > 0)
        {
            Debug.Log("Starting Flip Backward in Manager");
            StartCoroutine(FlipPagesSimultaneously("backward"));
        }
    }

    //Diese Methoden hinzugefügt, um sie innerhalb der Coroutine aufzurufen und die Rotationen-Liste korrekt zu verwalten
    private Coroutine LiftPageY(Transform page, List<Coroutine> rotations)
    {
        return StartCoroutine(AnimatePagePosition(page, 3f, true, rotations));
    }
     private Coroutine LiftPageY2(Transform page, List<Coroutine> rotations)
    {
        return StartCoroutine(AnimatePagePosition(page, 2f, true, rotations));
    }

    private Coroutine DownPageY(Transform page, List<Coroutine> rotations)
    {
        return StartCoroutine(AnimatePagePosition(page, 0f, false, rotations));
    }

    private IEnumerator FlipPagesSimultaneously(string direction)
    {
        Debug.Log("Flip Pages Simultaneously in Manager in direction: " + direction);
        isRotating = true;

        List<Coroutine> rotations = new List<Coroutine>();

        if (direction == "forward")
        {
            Debug.Log("Current Page Index - forward: " + currentPageIndex);

            if (currentPageIndex > 0)
            {
                rotations.Add(LiftPageY(pages[currentPageIndex - 1], rotations));
                rotations.Add(StartCoroutine(RotatePage(pages[currentPageIndex - 1], 180f)));
                rotations.Add(DownPageY(pages[currentPageIndex - 1], rotations));
            }

            rotations.Add(LiftPageY2(pages[currentPageIndex], rotations));
            rotations.Add(StartCoroutine(RotatePage(pages[currentPageIndex], 180f)));
            rotations.Add(DownPageY(pages[currentPageIndex], rotations));

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

            currentPageIndex--;
            Debug.Log("Updated Page Index - backward: " + currentPageIndex);

            if (currentPageIndex < pages.Length && pages[currentPageIndex] != null)
            {
                rotations.Add(LiftPageY2(pages[currentPageIndex], rotations));
                rotations.Add(StartCoroutine(RotatePage(pages[currentPageIndex], -180f)));
                rotations.Add(DownPageY(pages[currentPageIndex], rotations));

                if (currentPageIndex > 0)
                {
                    rotations.Add(LiftPageY(pages[currentPageIndex - 1], rotations));
                    rotations.Add(StartCoroutine(RotatePage(pages[currentPageIndex - 1], -180f)));
                    rotations.Add(DownPageY(pages[currentPageIndex - 1], rotations));
                }
            }

            foreach (Coroutine rot in rotations)
            {
                yield return rot;
            }
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

    //Methode zur Entgegennahme der Rotationsliste, um sie zu erweitern
    private IEnumerator AnimatePagePosition(Transform page, float targetYOffset, bool raise, List<Coroutine> rotations)
    {
        if (page == null) yield break;

        float startY = page.localPosition.y;
        float targetY = startY + targetYOffset * (raise ? 1 : -1);
        float elapsed = 0f;

        float animationDuration = rotationDuration / 2f;

        while (elapsed < animationDuration)
        {
            float newY = Mathf.Lerp(startY, targetY, elapsed / animationDuration);
            page.localPosition = new Vector3(page.localPosition.x, newY, page.localPosition.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
