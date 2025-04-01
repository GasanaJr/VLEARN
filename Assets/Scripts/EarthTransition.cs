using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EarthTransition : MonoBehaviour
{
    public Transform earth;
    public float moveSpeed = 2.0f;
    public string nextScene = "Volcanoes";

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    public void MoveToEarth()
    {
        StartCoroutine(EnterEarth());
    }

    private IEnumerator EnterEarth()
    {
        Vector3 startPosition = mainCamera.transform.position;
        Vector3 targetPosition = earth.position;
        Quaternion startRotation = mainCamera.transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(earth.position - startPosition);

        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;

        while ((Time.time - startTime) * moveSpeed / journeyLength < 1.0f)
        {
            float fraction = (Time.time - startTime) * moveSpeed / journeyLength;
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, fraction);
            mainCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, fraction);
            yield return null;
        }

        SceneManager.LoadScene(nextScene);
    }
}
