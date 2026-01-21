using UnityEngine;
using UnityEngine.Events;
public class LockOnTurret : MonoBehaviour
{
    [Header("References")]
    public Transform turretHead;
    public Transform firePoint;

    [Header("Targeting")]
    public Transform lockedTarget;
    public float detectionRange = 30f;

    [Header("Rotation")]
    public float rotationSpeed = 8f;

    [Header("Firing")]
    public float fireRate = 1f;
    public float range = 50f;

    float fireTimer;

    void Update()
    {
        lockedTarget = GameObject.FindGameObjectWithTag("Enemy").transform;
        if (lockedTarget == null)
        {
            AcquireTarget();
            return;
        }

        TrackTarget();
        FireIfReady();
        ValidateLock();
    }

    // 🔒 Lock onto first valid target
    void AcquireTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                lockedTarget = hit.transform;
                break;
            }
        }
    }

    // 🎯 Always track the same object
    void TrackTarget()
    {
        Vector3 direction = lockedTarget.position - turretHead.position;
        direction.y = 0f; // remove if vertical aiming is needed

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        turretHead.rotation = Quaternion.Slerp(
            turretHead.rotation,
            lookRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    // 🔫 Fire once per second
    void FireIfReady()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireRate)
        {
            FireRay();
            fireTimer = 0f;
        }
    }

    void FireRay()
    {
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log("Locked turret hit: " + hit.collider.name);
            hit.collider.GetComponent<EnemyFSM>()?.TakeDamage(10);
        }

        Debug.DrawRay(firePoint.position, firePoint.forward * range, Color.red, 1f);
    }

    // ❌ Unlock conditions
    void ValidateLock()
    {
        if (Vector3.Distance(transform.position, lockedTarget.position) > detectionRange)
        {
            lockedTarget = null;
        }
    }

    // Optional visual debugging
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
