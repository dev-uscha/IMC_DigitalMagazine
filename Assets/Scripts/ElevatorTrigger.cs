using System.Collections;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    public Transform elevatorPlatform;     // Die Plattform, die sich bewegt
    public Transform bottomPosition;       // Startposition (unten)
    public Transform topPosition;          // Zielposition (oben)
    public float moveDuration = 2f;        // Zeit zum Hoch-/Runterfahren

    private bool isMoving = false;
    private bool atTop = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isMoving || !other.CompareTag("Player")) return;

        if (atTop)
            StartCoroutine(MoveElevator(bottomPosition.position));
        else
            StartCoroutine(MoveElevator(topPosition.position));

        atTop = !atTop;
    }

    private IEnumerator MoveElevator(Vector3 targetPos)
    {
        isMoving = true;

        Vector3 startPos = elevatorPlatform.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            elevatorPlatform.position = Vector3.Lerp(startPos, targetPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        elevatorPlatform.position = targetPos;
        isMoving = false;
    }
}
