using System.Runtime.CompilerServices;
using UnityEngine;

public class DoorArea_1 : MonoBehaviour
{
    [SerializeField] private MeshRenderer doorMeshRenderer;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private GameObject playerCharacter;
    [SerializeField] private Material doorOpenMaterial;
    [SerializeField] private Material doorClosedMaterial;

    public bool unlockedDoor = false;
    public bool shouldDoorClose;

    // Update is called once per frame
    void Update()
    {
        if (player.playerHasKey == true)
        {
            doorMeshRenderer.material = doorOpenMaterial;
        }
        else
        {
            doorMeshRenderer.material = doorClosedMaterial;
        }

        if (unlockedDoor == true)
        {
            transform.position += Vector3.up * Time.deltaTime * 2f;
            if (transform.position.y >= 7.5f)
            {
                transform.position = new Vector3(transform.position.x, 7.5f, transform.position.z);
            }
        }

        if (shouldDoorClose == true)
        {
            if (transform.position.y >= 7.5f)
            {
                unlockedDoor = false;
                transform.position += Vector3.down * Time.deltaTime * 2f;
                if (transform.position.y <= 2.5f)
                {
                    transform.position = new Vector3(transform.position.x, 2.5f, transform.position.z);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == playerCharacter && player.playerHasKey == true && player.isBeingChased == false)
        {
            unlockedDoor = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerCharacter)
        {
            shouldDoorClose = true;
        }
    }
}
