using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float mouseSensitivity = 2f;
    public float minY = -60f;
    public float maxY = 60f;
    private float xRotation = 0f;



    void LateUpdate()
    {
        float mouseVertical = Input.GetAxis("Mouse Y");
        xRotation -= mouseVertical * mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, minY, maxY);
        // Only change the X axis (pitch), keep Y and Z as they are
        Vector3 currentAngles = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(xRotation, currentAngles.y, currentAngles.z);
    }
}
