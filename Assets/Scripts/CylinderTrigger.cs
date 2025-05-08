using System.Collections;
using UnityEngine;

public class CylinderTrigger : MonoBehaviour
{
    public GameObject cylinderToRaise;     // Hier ziehst du im Inspector den Zylinder rein
    public float riseHeight = 5f;
    public float riseDuration = 2f;
    public float rotationSpeed = 30f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool rising = false;
    private bool finished = false;

    private void Start()
    {
        if (cylinderToRaise == null)
        {
            Debug.LogError("Cylinder not assigned in Inspector!");
            enabled = false;
            return;
        }

        startPos = cylinderToRaise.transform.position;
        targetPos = startPos + Vector3.up * riseHeight;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!rising && !finished && other.CompareTag("Player"))
        {
            StartCoroutine(RiseAndRotate());
        }
    }

    private IEnumerator RiseAndRotate()
    {
        rising = true;
        float elapsed = 0f;

        while (elapsed < riseDuration)
        {
            float t = elapsed / riseDuration;

            // Steigen
            Vector3 newPosition = Vector3.Lerp(startPos, targetPos, t);
            cylinderToRaise.transform.position = newPosition;

            // Rotation
            cylinderToRaise.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Endposition & Rotation fertigstellen
        cylinderToRaise.transform.position = targetPos;
        rising = false;
        finished = true;
    }
}
