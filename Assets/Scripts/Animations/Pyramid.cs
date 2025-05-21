using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pyramid : MonoBehaviour
{
    [Header("Referenzen")]
    public Transform positionStart;
    public Transform positionEnd;
    public Transform objectsParent;
    public float durationPerMove = 1f;

    private List<Transform> cubes = new List<Transform>();
    private bool isAnimating = false;

    private void CacheCubes()
    {
        cubes.Clear();
        foreach (Transform child in objectsParent)
        {
            cubes.Add(child);
        }
    }

    public void StartPyramidAnimation()
    {
        if (isAnimating) return;

        CacheCubes();
        StartCoroutine(AnimatePyramidStaggered());
    }

    public void ReversePyramidAnimation()
    {
        if (isAnimating) return;

        CacheCubes();
        StartCoroutine(AnimatePyramidBackToStartParallel());
    }
//Gleichzeitiger aufbau
private IEnumerator AnimatePyramidStaggered()
{
    isAnimating = true;

    Vector3 endBase = positionEnd.position;
    float currentY = endBase.y;
    float delayBetweenCubes = 0.2f;

    List<Vector3> targetPositions = new List<Vector3>();

    // Zielpositionen vorberechnen
    foreach (Transform cube in cubes)
    {
        float cubeHeight = cube.GetComponent<Renderer>().bounds.size.y;
        targetPositions.Add(new Vector3(endBase.x, currentY, endBase.z));
        currentY += cubeHeight;
    }

    // Alle Cubes an Startposition setzen
    foreach (Transform cube in cubes)
    {
        cube.position = positionStart.position;
    }

    // Bewegung leicht versetzt starten
    for (int i = 0; i < cubes.Count; i++)
    {
        StartCoroutine(MoveOverTime(cubes[i], cubes[i].position, targetPositions[i], durationPerMove));
        yield return new WaitForSeconds(delayBetweenCubes);
    }

    // Warten, bis letzte Bewegung vorbei ist
    yield return new WaitForSeconds(durationPerMove + delayBetweenCubes);

    isAnimating = false;
}


// Nacheinander Aufbau der Pyramide
    private IEnumerator AnimatePyramidSequential()
    {
        isAnimating = true;

        Vector3 endBase = positionEnd.position;
        float currentY = endBase.y;

        for (int i = 0; i < cubes.Count; i++)
        {
            Transform cube = cubes[i];
            float cubeHeight = cube.GetComponent<Renderer>().bounds.size.y;

            Vector3 targetPos = new Vector3(endBase.x, currentY, endBase.z);
            cube.position = positionStart.position;

            yield return StartCoroutine(MoveOverTime(cube, cube.position, targetPos, durationPerMove));

            currentY += cubeHeight;
        }

        isAnimating = false;
    }

    private IEnumerator AnimatePyramidBackToStartParallel()
    {
        isAnimating = true;

        List<Vector3> startPositions = new List<Vector3>();
        foreach (var cube in cubes)
        {
            startPositions.Add(cube.position);
        }

        float elapsed = 0f;

        while (elapsed < durationPerMove)
        {
            float t = elapsed / durationPerMove;
            t = t * t * (3f - 2f * t); // Smoothstep

            for (int i = 0; i < cubes.Count; i++)
            {
                cubes[i].position = Vector3.Lerp(startPositions[i], positionStart.position, t);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Am Ende exakt auf Ziel setzen
        foreach (var cube in cubes)
        {
            cube.position = positionStart.position;
        }

        isAnimating = false;
    }

    private IEnumerator MoveOverTime(Transform obj, Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            t = t * t * (3f - 2f * t); // Smoothstep
            obj.position = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.position = to;
    }
}
