using UnityEngine;

public class MoonOrbit : MonoBehaviour
{
    public Transform user; // Assign the XR Rig (or Camera)
    public float orbitSpeed = 10f; // Speed of orbit
    public float orbitRadius = 3f; // Distance from the user (Earth)

    private float angle = 0f;

    void Update()
    {
        if (user == null) return; // Safety check

        // Increase angle over time to simulate orbit
        angle += orbitSpeed * Time.deltaTime;

        // Calculate new position around the user (Earth)
        float x = user.position.x + Mathf.Cos(angle) * orbitRadius;
        float z = user.position.z + Mathf.Sin(angle) * orbitRadius;

        // Update Moon position
        transform.position = new Vector3(transform.position.x, x, z);
    }
}
