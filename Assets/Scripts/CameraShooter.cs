using UnityEngine;

public class CameraShooter : MonoBehaviour
{
    public Camera playerCamera;
    public float shootRange = 100f;
    public float impactForce = 500f;
    public GameObject impactEffect;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, shootRange))
        {
            Rigidbody rb = hit.rigidbody;
            if (rb != null)
            {
                rb.AddForce(ray.direction * impactForce);
            }
            if (impactEffect != null)
            {
                Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }
}
