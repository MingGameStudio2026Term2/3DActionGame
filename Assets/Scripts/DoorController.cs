using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Setup")]
    public Transform door; // Assign the door object here (with correct pivot)
    public Transform doorFrame; // Assign the door frame child here (optional)
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public bool isOpen = false;
    [Tooltip("Flip the door swing direction")] public bool flipDirection = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        if (door == null)
        {
            Debug.LogError("Door reference not set in DoorController.");
            return;
        }
        closedRotation = door.localRotation;
        float angle = flipDirection ? -openAngle : openAngle;
        openRotation = closedRotation * Quaternion.Euler(0, 0, angle);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpen = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpen = false;
        }
    }

    void Update()
    {
        if (door == null) return;
        if (isOpen)
        {
            door.localRotation = Quaternion.Lerp(door.localRotation, openRotation, Time.deltaTime * openSpeed);
        }
        else
        {
            door.localRotation = Quaternion.Lerp(door.localRotation, closedRotation, Time.deltaTime * openSpeed);
        }
    }
}
