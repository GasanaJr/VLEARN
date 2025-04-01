using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform sun; 
    public float orbitSpeed = 10f;  

    public float orbitRadius = 6f; 
    private float angle;
    private bool isPaused = false;

    void Update()
    {
        if (!isPaused)
        {

            angle += (orbitSpeed * Time.deltaTime) % 360;


            float x = sun.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * orbitRadius;
            float z = sun.position.z + Mathf.Sin(angle * Mathf.Deg2Rad) * orbitRadius;

            transform.position = new Vector3(x, transform.position.y, z);
        }
    }

    public void TogglePause(bool pause)
    {
        isPaused = pause;
    }
}
