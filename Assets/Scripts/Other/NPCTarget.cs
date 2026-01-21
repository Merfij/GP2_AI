using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class NPCTarget : MonoBehaviour
{
    public float health;
    public LayerMask stunMask;
    public LayerMask etherealMask;
    public LayerMask shiftingMask;
    public UnityEvent stunEnemy;
    public UnityEvent resumeEnemy;
    public UnityEvent etherealEnemy;

    public UnityEvent hitShiftingEnemy;

    public bool useLightBeam;

    private void Start()
    {
        useLightBeam = false;
    }

    private void Update()
    {
        if (health <= 0) 
        {
            Destroy(gameObject);
        }

        if (useLightBeam)
        {
            RaycastHit hitStun;
            if (Physics.Raycast(transform.position, transform.forward, out hitStun, Mathf.Infinity, stunMask))
            {
                stunEnemy.Invoke();
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitStun.distance, Color.yellow);
                Debug.Log("Did Hit");
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.darkRed);
                Debug.Log("Did not Hit");
                resumeEnemy.Invoke();
            }

            RaycastHit hitEthereal;
            if (Physics.Raycast(transform.position, transform.forward, out hitEthereal, Mathf.Infinity, etherealMask))
            {
                etherealEnemy.Invoke();
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitEthereal.distance, Color.yellow);
                Debug.Log("Did Hit");
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.darkRed);
                Debug.Log("Did not Hit");
            }

            RaycastHit hitShifting;
            if (Physics.Raycast(transform.position, transform.forward, out hitShifting, Mathf.Infinity, shiftingMask))
            {
                hitShiftingEnemy.Invoke();
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitShifting.distance, Color.yellow);
                Debug.Log("Did Hit");
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.darkRed);
                Debug.Log("Did not Hit");
            }
        }

    }

}
