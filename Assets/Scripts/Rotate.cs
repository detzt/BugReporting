using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotate : MonoBehaviour
{
    public GameObject targetObject; // set this in the inspector
    public float speed = 10f;
    public Text fpsText;    // Drag and drop your UI Text here in inspector.
    private float deltaTime = 0.0f;

    void Update() {
        transform.RotateAround(targetObject.transform.position, Vector3.up, speed * Time.deltaTime);
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = string.Format("FPS: {0:0.}", fps);
    }
}
