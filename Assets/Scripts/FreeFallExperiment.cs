using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class FreeFallExperiment : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI infoText;
    private Rigidbody rb;
    private Vector3 startPosition;
    private float fallTime;
    private bool isFalling = false;
    private bool useAirResistance = false;
    public TMP_Dropdown gravityDropdown;

    private float[] gravityValues = { 9.81f, 1.62f, 24.79f, 0f };

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        ResetObject();

        if (gravityDropdown != null)
        {
            gravityDropdown.onValueChanged.AddListener(OnGravityDropdownChanged);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (isFalling)
        {
            fallTime += Time.deltaTime;
            if (infoText)
                infoText.text = $"Time: {fallTime:F2} sec \nVelocity: {rb.velocity.magnitude:F2} m/s";
        }
    }

    void OnGravityDropdownChanged(int index) 
    {
        Debug.Log(index);
        if (index >= 0 && index < gravityValues.Length)
        {
            float selectedGravity = gravityValues[index];
            SetGravity(selectedGravity);
        }
    }

    public void StartFreeFall()
    {
        isFalling = true;
        fallTime = 0;
        rb.useGravity = true;
        rb.drag = useAirResistance ? 1.5f : 0f;
    }

    public void ResetObject()
    {
        isFalling = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;
        transform.position = startPosition;
        fallTime = 0;
        if (infoText)
            infoText.text = "Ready to Drop";
    }

    public void SetGravity(float gravityValue)
    {
        Physics.gravity = new Vector3(0, -gravityValue, 0);
        Debug.Log($"Gravity set at {gravityValue}");
    }

    public void ToggleAirResistance()
    {
        useAirResistance = !useAirResistance;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isFalling = false;
        }
    }
}
