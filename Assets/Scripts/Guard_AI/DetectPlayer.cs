using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Detection Settings")]
    [SerializeField] private float viewLength;
    [SerializeField, Range(0, 360)] private float detectionAngle = 90f;
    private float increasedDetectionAngle = 120f;
    private float normalDetectionAngle = 90f;

    [Header("Line of Sight")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform eyePosition;

    private Transform cachedTarget;

    public float ViewLength => viewLength;
    public float DetectionAngle => detectionAngle;

    private Transform EyesTransform => eyePosition != null ? eyePosition : transform;

    public bool canSeePlayer;

    private void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        cachedTarget = playerObj != null ? playerObj.transform : null;
    }

    private void Update()
    {
        canSeePlayer = CanSeeTarget();

        Debug.DrawRay(EyesTransform.position, EyesTransform.forward * viewLength, Color.red);
        Debug.Log(canSeePlayer.ToString());
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
                        detectionAngle = increasedDetectionAngle;
                        canSeePlayer = true;
                        return true;
                    }
                }
            }
        }
        detectionAngle = normalDetectionAngle;
        canSeePlayer = false;
        return false;
    }
}
