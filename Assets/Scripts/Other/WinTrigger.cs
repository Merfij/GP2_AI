using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public GameObject youWin;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("You Win!");
            // Additional win logic can be added here
            youWin.SetActive(true);
        }
    }
}
