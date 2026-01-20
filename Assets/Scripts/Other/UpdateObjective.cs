using UnityEngine;
using TMPro;
using System.Reflection;
using System.Net;
using Unity.VisualScripting;

public class UpdateObjective : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] public bool playerPassedFirstCheckpoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerPassedFirstCheckpoint = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        {
            if (other.CompareTag("Player"))
            {
                playerPassedFirstCheckpoint = true;
                player.stealText.fontStyle = FontStyles.Italic | FontStyles.Underline;
                player.stealText.text = " - Press the button to proceed";
                player.keyIcon.SetActive(false);
                return;
            }
        }
    }
}
