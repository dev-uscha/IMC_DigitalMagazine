using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Diese Klasse steuert das Umblättern von Seiten in einer 3D-Ansicht
public class PageManager : MonoBehaviour
{
    // Referenz zum Elternelement, das alle Seiten als Kindobjekte enthält
    public Transform pagesParent;

    // Dauer einer einzelnen Seitenrotation in Sekunden
    public float rotationDuration = 1.0f;

    // Array mit allen Seiten (Transform-Komponenten)
    private Transform[] pages;

    // Index der aktuellen Seite (beginnend bei 0)
    private int currentPageIndex = 0;

    // Schutzvariable, um zu verhindern, dass mehrere Rotationen gleichzeitig laufen
    private bool isRotating = false;

    private void Start()
    {
        // Initialisiere das Seiten-Array mit allen direkten Kindern von pagesParent
        int count = pagesParent.childCount;
        pages = new Transform[count];

        // Fülle das Array mit den Seiten (Kindobjekte von pagesParent)
        for (int i = 0; i < count; i++)
        {
            pages[i] = pagesParent.GetChild(i);
        }
    }

    // Methode zum Vorblättern
    public void FlipForward()
    {
        Debug.Log("Flip Forward in Manager");

        // Nur ausführen, wenn keine Rotation läuft und man nicht am Ende angekommen ist
        if (!isRotating && currentPageIndex < pages.Length)
        {
            Debug.Log("Starting Flip Forward in Manager");

            // Starte die Routine zum gleichzeitigen Blättern
            StartCoroutine(FlipPagesSimultaneously("forward"));
        }
    }

    // Methode zum Zurückblättern
    public void FlipBackward()
    {
        Debug.Log("Flip Backward in Manager");

        // Nur ausführen, wenn keine Rotation läuft und man nicht auf Seite 0 ist
        if (!isRotating && currentPageIndex > 0)
        {
            Debug.Log("Starting Flip Backward in Manager");

            // Starte die Routine zum gleichzeitigen Rückblättern
            StartCoroutine(FlipPagesSimultaneously("backward"));
        }
    }

   private IEnumerator FlipPagesSimultaneously(string direction)
{
    Debug.Log("Flip Pages Simultaneously in Manager in direction: " + direction);
    isRotating = true; // Blockiert weitere Rotationen während der laufenden Animation

    // Liste, um mehrere laufende Coroutines zu verwalten
    List<Coroutine> rotations = new List<Coroutine>();

    if (direction == "forward")
    {
        Debug.Log("Current Page Index - forward: " + currentPageIndex);

        // Vorherige Seite schließen, wenn nicht auf der allerersten Seite
        if (currentPageIndex > 0)
        {
            rotations.Add(StartCoroutine(RotatePage(pages[currentPageIndex - 1], 180f)));
        }

        // Neue Seite öffnen (aktueller Index)
        rotations.Add(StartCoroutine(RotatePage(pages[currentPageIndex], 180f)));

        // Warte, bis alle Rotationen abgeschlossen sind
        foreach (Coroutine rot in rotations)
        {
            yield return rot;
        }

        // Index aktualisieren
        currentPageIndex++;
        Debug.Log("Updated Page Index - forward: " + currentPageIndex);
    }
    else if (direction == "backward")
    {
        Debug.Log("Current Page Index - backward: " + currentPageIndex);

        // Index aktualisieren, _bevor_ die Animationen starten
        currentPageIndex--;
        Debug.Log("Updated Page Index - backward: " + currentPageIndex);

        // Aktuelle Seite zurückklappen
        if (currentPageIndex < pages.Length && pages[currentPageIndex] != null)
        {
            rotations.Add(StartCoroutine(RotatePage(pages[currentPageIndex], -180f)));

            // Vorherige Seite wieder aufklappen
            if (currentPageIndex > 0) //Stell sicher, dass es eine vorherige Seite gibt
            {
                rotations.Add(StartCoroutine(RotatePage(pages[currentPageIndex - 1], -180f))); //Vorherige Seite aufklappen
            }
        }

        // Warte, bis alle Rotationen abgeschlossen sind
        foreach (Coroutine rot in rotations)
        {
            yield return rot;
        }
    }

    isRotating = false; // Rotation abgeschlossen → weitere Eingaben möglich
}


    // Einzelne Seite rotieren (Auf- oder Zuklappen)
    private IEnumerator RotatePage(Transform page, float angle)
    {
        // Starte von der aktuellen Rotation
        Quaternion startRot = page.rotation;

        // Zielrotation: Drehe um den angegebenen Winkel (meist 180 oder -180 Grad um die X-Achse)
        Quaternion endRot = startRot * Quaternion.Euler(angle, 0f, 0f);

        float elapsed = 0f;

        // Übergang über "rotationDuration" Sekunden hinweg
        while (elapsed < rotationDuration)
        {
            page.rotation = Quaternion.Slerp(startRot, endRot, elapsed / rotationDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Am Ende die Rotation exakt setzen, um Ungenauigkeiten zu vermeiden
        page.rotation = endRot;
    }
<<<<<<< HEAD

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
=======
>>>>>>> 54f56820b8070bcb71dffc9babebb3e2fc8a0fc3
}