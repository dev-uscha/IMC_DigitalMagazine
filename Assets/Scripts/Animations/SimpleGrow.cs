using UnityEngine;

public class SimpleGrow : MonoBehaviour
{
    [Header("Referenzen")]
    public Transform positionStart;
    public Transform positionEnd;
    public Transform objectToMove;

    [Header("Einstellungen")]
    public float duration = 2f;

    private float elapsedTime = 0f;
    private bool isMoving = false;
    private Vector3 moveFrom;
    private Vector3 moveTo;

    void Update()
    {
        if (!isMoving || objectToMove == null)
            return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);
        t = t * t * (3f - 2f * t); // Smoothstep für weicheres Starten/Stoppen
        objectToMove.position = Vector3.Lerp(moveFrom, moveTo, t);

        if (t >= 1f)
            isMoving = false;
    }

    /// <summary>
    /// Startet die Bewegung von Start nach Ende
    /// </summary>
    public void MoveToEnd()
    {
        if (positionStart == null || positionEnd == null || objectToMove == null)
            return;

        moveFrom = positionStart.position;
        moveTo = positionEnd.position;
        BeginMove();
    }

    /// <summary>
    /// Startet die Bewegung von Ende zurück zum Start
    /// </summary>
    public void MoveToStart()
    {
        if (positionStart == null || positionEnd == null || objectToMove == null)
            return;

        moveFrom = positionEnd.position;
        moveTo = positionStart.position;
        BeginMove();
    }

    private void BeginMove()
    {
        elapsedTime = 0f;
        isMoving = true;
    }
}
