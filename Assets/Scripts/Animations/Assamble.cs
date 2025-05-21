using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assamble : MonoBehaviour
{
    [System.Serializable]
    public class MovingElement
    {
        public Transform cube;
        public Transform positionStart;
        public Transform positionEnd;
    }

    public Transform objectsRoot; // Reference to the "Objects" parent in the Prefab
    public float moveDuration = 2f;
    public float delayBetweenElements = 0.2f;
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [HideInInspector]
    public List<MovingElement> elements = new List<MovingElement>();

    private Coroutine currentRoutine;

    void Awake()
    {
        AutoFillElements();
    }

    private void AutoFillElements()
    {
        elements.Clear();

        if (objectsRoot == null)
        {
            Debug.LogError("GroupedMover: objectsRoot (Objects folder) is not assigned!");
            return;
        }

        foreach (Transform child in objectsRoot)
        {
            Transform cube = child.Find("Cube");
            Transform start = child.Find("PositionStart");
            Transform end = child.Find("PositionEnd");

            if (cube != null && start != null && end != null)
            {
                MovingElement e = new MovingElement
                {
                    cube = cube,
                    positionStart = start,
                    positionEnd = end
                };
                elements.Add(e);
            }
            else
            {
                Debug.LogWarning($"GroupedMover: Child '{child.name}' is missing Cube, PositionStart or PositionEnd.");
            }
        }

        Debug.Log($"GroupedMover: {elements.Count} elements auto-filled from '{objectsRoot.name}'.");
    }

    public void MoveToEnd()
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(MoveAll(true));
    }

    public void MoveToStart()
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(MoveAll(false));
    }

    private IEnumerator MoveAll(bool toEnd)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            StartCoroutine(MoveSingle(elements[i], toEnd));
            yield return new WaitForSeconds(delayBetweenElements);
        }
    }

    private IEnumerator MoveSingle(MovingElement element, bool toEnd)
    {
        Vector3 from = toEnd ? element.positionStart.position : element.positionEnd.position;
        Vector3 to = toEnd ? element.positionEnd.position : element.positionStart.position;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            float curvedT = moveCurve.Evaluate(t);
            element.cube.position = Vector3.Lerp(from, to, curvedT);
            yield return null;
        }

        element.cube.position = to;
    }
}
