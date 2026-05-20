using UnityEngine;
    
    


public class DroneController : MonoBehaviour
{
    [SerializeField] private Transform followTarget; // The target to follow (e.g., player)
    [SerializeField] private Transform shootTarget;  // The target to shoot (e.g., zombie)
    [SerializeField] private float followDistance = 4f;
    [SerializeField] private float sideOffset = 0f;
    [SerializeField] private float followHeight = 2.5f;
    [SerializeField] private float followSmoothTime = 0.2f;
    [SerializeField] private float bobAmplitude = 0.35f;
    [SerializeField] private float bobFrequency = 1.8f;

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootOrigin;
    [SerializeField] private float shootInterval = 1.0f;
    [SerializeField] private float bulletSpeed = 15f;

    private Vector3 followVelocity;
    private float bobTimeOffset;
    private float nextShootTime;

    private void Awake()
    {
        bobTimeOffset = Random.Range(0f, 100f);

        if (followTarget == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                followTarget = playerObject.transform;
            }
        }
    }

    private void LateUpdate()
    {
        if (followTarget == null)
        {
            return;
        }

        Vector3 followDirection = -followTarget.forward;
        Vector3 sideDirection = followTarget.right;
        Vector3 desiredPosition = followTarget.position + (followDirection * followDistance) + (sideDirection * sideOffset);

        float bobOffset = Mathf.Sin((Time.time + bobTimeOffset) * bobFrequency) * bobAmplitude;
        desiredPosition.y = followTarget.position.y + followHeight + bobOffset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref followVelocity,
            followSmoothTime
        );

        transform.LookAt(followTarget.position + Vector3.up * 1.2f);

        if (shootTarget == null)
        {
            FindAndSetNearestZombie();
        }
        ShootAtTarget();
    }

    public void SetFollowTarget(Transform newFollowTarget)
    {
        followTarget = newFollowTarget;
    }

    public void SetShootTarget(Transform newShootTarget)
    {
        shootTarget = newShootTarget;
    }

    public void ShootAtTarget()
    {
        if (shootTarget == null || bulletPrefab == null || shootOrigin == null)
            return;

        if (Time.time < nextShootTime)
            return;

        nextShootTime = Time.time + shootInterval;

        // Instantiate bullet
        GameObject bullet = Instantiate(bulletPrefab, shootOrigin.position, Quaternion.identity);
        Vector3 direction = (shootTarget.position - shootOrigin.position).normalized;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }
    }
    public void FindAndSetNearestZombie()
    {
        ZombieController[] zombies = GameObject.FindObjectsOfType<ZombieController>();
        Transform nearest = null;
        float minDist = float.MaxValue;
        Vector3 myPos = transform.position;
        foreach (var zombie in zombies)
        {
            float dist = Vector3.Distance(myPos, zombie.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = zombie.transform;
            }
        }
        if (nearest != null)
        {
            SetShootTarget(nearest);
        }
    }
}
