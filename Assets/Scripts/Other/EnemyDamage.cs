using System.Collections;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float givenDamage;
    public NPCTarget target;

    public bool canAttack;

    private void Awake()
    {
        canAttack = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canAttack == true)
        {
            target.health -= givenDamage;
            canAttack = false;
            StartCoroutine(ResetAttack());
        }
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1);
        canAttack = true;
    }
}
