using System.Collections;
using UnityEngine;

public class SolarEclipse : MonoBehaviour
{
    public Transform sun; // Assign the Sun
    public Light directionalLight; // Assign the Sun's Light
    public float moveSpeed = 1f;
    public float eclipseDuration = 30f;
    public float minDistanceToTrigger = 2f;

    // Eclipse light settings
    public Color normalLightColor = Color.white;
    public Color midEclipseLightColor = new Color(0.7f, 0.1f, 0.1f, 1.0f); // Dark red
    public Color peakEclipseLightColor = new Color(0.2f, 0.0f, 0.0f, 1.0f); // Very dark red, almost black
    public float minLightIntensity = 0.2f;

    private Vector3 originalPosition;
    private bool isEclipsing = false;
    private float originalLightIntensity;

    void Start()
    {
        originalPosition = transform.position;
        originalLightIntensity = directionalLight.intensity;
        directionalLight.color = normalLightColor;
    }

    void Update()
    {
        if (!isEclipsing && Mathf.Abs(transform.position.x - sun.position.x) > minDistanceToTrigger)
        {
            StartCoroutine(TriggerEclipse());
        }
    }

    IEnumerator TriggerEclipse()
    {
        isEclipsing = true;
        float targetX = sun.position.x;

        // Move Moon into position
        while (Mathf.Abs(transform.position.x - targetX) > 0.2f)
        {
            // Update position
            transform.position = new Vector3(
                Mathf.MoveTowards(transform.position.x, targetX, moveSpeed * Time.deltaTime),
                transform.position.y,
                transform.position.z
            );

            // Calculate eclipse progress based on position (0 to 1)
            float distance = Mathf.Abs(transform.position.x - targetX);
            float maxDistance = Mathf.Abs(originalPosition.x - targetX);
            float eclipseProgress = 1 - (distance / maxDistance);

            // Update light color and intensity
            UpdateLightSettings(eclipseProgress);

            yield return null;
        }

        // Hold at peak eclipse
        yield return new WaitForSeconds(eclipseDuration);

        // Move Moon back
        while (Mathf.Abs(transform.position.x - originalPosition.x) > 0.2f)
        {
            // Update position
            transform.position = new Vector3(
                Mathf.MoveTowards(transform.position.x, originalPosition.x, moveSpeed * Time.deltaTime),
                transform.position.y,
                transform.position.z
            );

            // Calculate eclipse progress (1 to 0)
            float distance = Mathf.Abs(transform.position.x - originalPosition.x);
            float maxDistance = Mathf.Abs(targetX - originalPosition.x);
            float eclipseProgress = distance / maxDistance;

            // Update light color and intensity
            UpdateLightSettings(eclipseProgress);

            yield return null;
        }

        // Ensure we're fully back to normal
        directionalLight.color = normalLightColor;
        directionalLight.intensity = originalLightIntensity;

        isEclipsing = false;
    }

    void UpdateLightSettings(float progress)
    {
        // Clamp progress to 0-1 range
        progress = Mathf.Clamp01(progress);

        // First half of eclipse: normal to mid-eclipse (dark red)
        // Second half: mid-eclipse to peak (almost black)
        Color targetColor;

        if (progress < 0.5f)
        {
            // Scale progress to 0-1 range for first half
            float adjustedProgress = progress * 2f;
            targetColor = Color.Lerp(normalLightColor, midEclipseLightColor, adjustedProgress);
        }
        else
        {
            // Scale progress to 0-1 range for second half
            float adjustedProgress = (progress - 0.5f) * 2f;
            targetColor = Color.Lerp(midEclipseLightColor, peakEclipseLightColor, adjustedProgress);
        }

        // Update light color
        directionalLight.color = targetColor;

        // Update light intensity (brightest at start/end, dimmest at peak)
        float intensityFactor = 1f - (progress * (1f - (minLightIntensity / originalLightIntensity)));
        directionalLight.intensity = originalLightIntensity * intensityFactor;
    }
}