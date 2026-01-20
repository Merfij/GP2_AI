using UnityEngine;

public class CameraEyes : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Detection Settings")]
    [SerializeField] private float viewLength = 10f;
    [SerializeField, Range(0, 360)] private float detectionAngle = 90f;

    [Header("Line of Sight")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform eyePosition;

    [Header("Audio")]
    [SerializeField] private AudioClip sightedSFX;
    [SerializeField] private AudioSource audioSource;

    private Transform cachedTarget;

    [SerializeField] private PlayerMovement playerScript;

    public bool isPlayerInSight;

    public float ViewLength => viewLength;
    public float DetectionAngle => detectionAngle;

    private Transform EyesTransform => eyePosition != null ? eyePosition : transform;

    private void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        playerScript = playerObj.GetComponent<PlayerMovement>();
        cachedTarget = playerObj != null ? playerObj.transform : null;
        isPlayerInSight = false;
    }



    // Update is called once per frame
    void Update()
    {
        if (CanSeeTarget())
        {
            isPlayerInSight = true;
        }
        else
        {
            isPlayerInSight = false;
        }
    }

    public bool CanSeeTarget()
    {
        if (cachedTarget == null)
            return false;
        Vector3 directionToTarget = (cachedTarget.position - EyesTransform.position).normalized;
        float distanceToTarget = Vector3.Distance(EyesTransform.position, cachedTarget.position);
        if (distanceToTarget <= viewLength)
        {
            float angleToTarget = Vector3.Angle(EyesTransform.forward, directionToTarget);
            if (angleToTarget <= detectionAngle / 2f)
            {
                RaycastHit hit;
                if (Physics.Raycast(EyesTransform.position, directionToTarget, out hit, viewLength, playerLayer))
                {
                    if (hit.transform == cachedTarget)
                    {
                        audioSource.PlayOneShot(sightedSFX, 0.3f);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewLength);
        Vector3 leftBoundary = Quaternion.Euler(0, -detectionAngle / 2f, 0) * transform.forward * viewLength;
        Vector3 rightBoundary = Quaternion.Euler(0, detectionAngle / 2f, 0) * transform.forward * viewLength;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }
}
