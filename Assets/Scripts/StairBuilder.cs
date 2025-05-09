using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairBuilder : MonoBehaviour
{
    public Transform stairsParent;          // Parent mit den Stufen
    public float stepHeight = 0.3f;         // Höhe pro Stufe
    public float stepDepth = 0.5f;          // Tiefe pro Stufe
    public float buildDelay = 0.1f;         // Zeitverzögerung pro Stufe

    private bool built = false;
    private List<Vector3> originalPositions = new List<Vector3>();

    private void Start()
    {
        // Ursprungspositionen speichern
        foreach (Transform step in stairsParent)
        {
            originalPositions.Add(step.localPosition);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!built && other.CompareTag("Player"))
        {
            StartCoroutine(BuildStairs());
            built = true;
        }
    }

    private IEnumerator BuildStairs()
    {
        for (int i = 0; i < stairsParent.childCount; i++)
        {
            Transform step = stairsParent.GetChild(i);
            Vector3 targetPosition = new Vector3(
                step.localPosition.x,
                i * stepHeight,
                i * stepDepth
            );

            StartCoroutine(MoveStep(step, targetPosition));
            yield return new WaitForSeconds(buildDelay);
        }

        // Warte 2 Minuten (120 Sekunden) und fahre dann zurück
        yield return new WaitForSeconds(20f);
        StartCoroutine(ResetStairs());
    }

    private IEnumerator ResetStairs()
    {
        for (int i = 0; i < stairsParent.childCount; i++)
        {
            Transform step = stairsParent.GetChild(i);
            Vector3 originalPos = originalPositions[i];

            StartCoroutine(MoveStep(step, originalPos));
            yield return new WaitForSeconds(buildDelay);
        }

        built = false; // Optional: Treppe kann erneut gebaut werden
    }

    private IEnumerator MoveStep(Transform step, Vector3 targetPos)
    {
        Vector3 startPos = step.localPosition;
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            step.localPosition = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        step.localPosition = targetPos;
    }
}
