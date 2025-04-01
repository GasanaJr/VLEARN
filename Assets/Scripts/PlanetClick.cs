using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlanetClick : MonoBehaviour
{
    public GameObject infoPanel;
    public Transform user;
    private Orbit orbitScript;
    public float moveSpeed = 50f;
    public float closeDistance = 5f;
    public MonoBehaviour rotator;
    private Vector3 originalPosition;
    public bool isMoving = false;
    public bool isReturning = false;

    void Start()
    {
        orbitScript = GetComponent<Orbit>();
        infoPanel.SetActive(false);
        if (rotator != null)
        {
            rotator.enabled = false;
        }

    }


    public void ShowInfoPanel()
    {
        if (!isMoving && !isReturning)
        {
            originalPosition = transform.position;
            StartCoroutine(MoveToUser());
        }
    }

    IEnumerator MoveToUser()
    {
        isMoving = true;
        orbitScript.TogglePause(true);
        Vector3 targetPosition = user.position + (transform.position - user.position).normalized * closeDistance;

        while(Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        infoPanel.SetActive(true);
        infoPanel.transform.LookAt(user.position);
        infoPanel.transform.Rotate(0, 180, 0);
        isMoving = false;
        
    }

    public void HideInfoPanel()
    {
        if (!isMoving && !isReturning)
        {
            StartCoroutine(MoveBack());
        }
    }

    IEnumerator MoveBack()
    {
        isReturning = true;
        infoPanel.SetActive(false);

        while (Vector3.Distance(transform.position, originalPosition) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isReturning = false;
        orbitScript.TogglePause(false);
        if (rotator != null)
        {
            rotator.enabled = false;
        }
    }

    public void HandleRotation()
    {
        if (rotator != null)
        {
            rotator.enabled = true;
        }
    }

}
