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
            // Always enable gravity
            rb.useGravity = enableGravity;

            // Prevent particles from sleeping
            rb.sleepThreshold = 0f;

            // Ensure collision detection is continuous
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            // Add some drag to prevent excessive velocity
            rb.drag = 0.5f;
        }
    }
}