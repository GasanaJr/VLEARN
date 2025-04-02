using UnityEngine;

public class ParticlePhysics : MonoBehaviour
{
    public bool enableGravity = true;

    private Rigidbody rb;
    private Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        originalScale = transform.localScale;
        ConfigureRigidbodyPhysics();
    }

    void ConfigureRigidbodyPhysics()
    {
        if (rb != null)
        {
            rb.useGravity = enableGravity;
            rb.sleepThreshold = 0f;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.drag = 0.5f;
        }
    }
}