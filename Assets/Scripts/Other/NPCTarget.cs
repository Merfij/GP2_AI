using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class NPCTarget : MonoBehaviour
{
    public float health;
    public LayerMask enemyMask;
    public UnityEvent removeSphere;

    private void Update()
    {
        if (health <= 0) 
        {
            Destroy(gameObject);
        }

        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, enemyMask))
        //{
        //    removeSphere.Invoke();
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        //    Debug.Log("Did Hit");
        //} else
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        //    Debug.Log("Did not Hit");
        //}
        
    }

}
