using Unity.VisualScripting;
using UnityEngine;

public class NPCTarget : MonoBehaviour
{
    public float health;

    private void Update()
    {
        if (health <= 0) 
        {
            Debug.Log("Target died");
            Destroy(gameObject);
        }
    }

}
