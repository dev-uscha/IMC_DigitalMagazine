using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlipPageRight : MonoBehaviour
{
    public Transform pagesParent;           // Parent mit allen Seiten
    public float rotationDuration = 1.0f;   // Dauer der Rotation in Sekunden

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isRotating && currentPageIndex < pages.Length)
        {
            StartCoroutine(FlipPagesSimultaneously());
        }
    }

    private IEnumerator FlipPagesSimultaneously()
    {
        isRotating = true;

        List<Coroutine> rotations = new List<Coroutine>();

        // Vorherige Seite schließen (falls vorhanden)
        if (currentPageIndex > 0)
        {
            rotations.Add(StartCoroutine(RotatePage(pages[currentPageIndex - 1], 180f)));
        }

        // Aktuelle Seite öffnen
        rotations.Add(StartCoroutine(RotatePage(pages[currentPageIndex], 180f)));

        // Warten bis alle gleichzeitig gestarteten Rotationen fertig sind
        foreach (Coroutine rot in rotations)
        {
            yield return rot;
        }

        currentPageIndex++;
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
