using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelScaler : MonoBehaviour
{
    public Transform user;  
    public float minScale = 0.5f;  
    public float maxScale = 3f;  
    public float scaleFactor = 10f;  

    void Update()
    {
        float distance = Vector3.Distance(transform.position, user.position);
        float scale = Mathf.Clamp(distance / scaleFactor, minScale, maxScale);
        transform.localScale = Vector3.one * scale;
    }
}
