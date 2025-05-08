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
        // Hole alle direkten Kinder als Seiten
        int count = pagesParent.childCount;
        pages = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            pages[i] = pagesParent.GetChild(i);
        }
    }

    public void FlipForward()
    {
        Debug.Log("Flip Froward in Manager");
        if (!isRotating && currentPageIndex < pages.Length){
            Debug.Log("Starting Flip Forward in Manager");
             StartCoroutine(FlipPagesSimultaneously("forward"));
        // currentPageIndex++;
        }
        }
      

    public void FlipBackward()
    {
        Debug.Log("Flip Backward in Manager");
        if(!isRotating && currentPageIndex > 0){
           Debug.Log("Starting Flip Backward in Manager");
            StartCoroutine(FlipPagesSimultaneously("backward"));
        // currentPageIndex--;
        };
        
    }

    private IEnumerator FlipPagesSimultaneously(string direction)
    {
        Debug.Log("Flip Pages Simultaneously in Manager ind direction:" + direction);
        isRotating = true;

        List<Coroutine> rotations = new List<Coroutine>();

        if (direction == "forward")
    {
        if (currentPageIndex < pages.Length)
        {
            // Aktuelle Seite umblättern
            rotations.Add(StartCoroutine(RotatePage(pages[currentPageIndex], 180f)));
        }

        // Optional: Vorherige Seite nochmal schließen (optisch)
        // if (currentPageIndex > 0)
        // {
        //     rotations.Add(StartCoroutine(RotatePage(pages[currentPageIndex - 1], 180f)));
        // }
    }
    else if (direction == "backward")
    {
        if (currentPageIndex > 0)
        {
            // Vorherige Seite wieder sichtbar machen
            rotations.Add(StartCoroutine(RotatePage(pages[currentPageIndex - 1], -180f)));
        }
    }

    foreach (Coroutine rot in rotations)
    {
        yield return rot;
    }

    // Index anpassen nach der Aktion
    if (direction == "forward" && currentPageIndex < pages.Length)
    {
        currentPageIndex++;
    }
    else if (direction == "backward" && currentPageIndex > 0)
    {
        currentPageIndex--;
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

}
