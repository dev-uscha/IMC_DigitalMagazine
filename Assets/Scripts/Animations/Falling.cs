using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Falling : MonoBehaviour
{
   public Transform positionStart;
    private Rigidbody rb;

    private bool isFalling = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ResetObject();
    }

    void Update()
    {
        // Optional: Falls du willst, dass es sich beim Fallen nicht bewegt
        if (isFalling && rb.velocity.magnitude < 0.1f)
        {
            // Gelandet (quasi still)
            StopFall();
        }
    }

    public void DropFromSky()
    {
        ResetObject();
        transform.position = positionStart.position;

        rb.isKinematic = false;
        rb.useGravity = true;
        isFalling = true;
    }

    private void ResetObject()
    {
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        isFalling = false;
    }

    private void StopFall()
    {
        rb.isKinematic = true;
        rb.useGravity = false;
        isFalling = false;
    }

    // Optional: Präziser bei Kontakt
    private void OnCollisionEnter(Collision other)
    {
        if (isFalling)
        {
            StopFall();
        }
    }
}
