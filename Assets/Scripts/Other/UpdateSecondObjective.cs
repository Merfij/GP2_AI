using UnityEngine;
using TMPro;
using System.Reflection;
using System.Net;
using Unity.VisualScripting;

public class UpdateSecondObjective : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private UpdateObjective updateObjective;
    [SerializeField] public bool playerPassedSecondCheckpoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerPassedSecondCheckpoint = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (updateObjective.playerPassedFirstCheckpoint == true)
        {
            if (other.CompareTag("Player"))
            {
                 playerPassedSecondCheckpoint = true;
                 player.stealText.text = " - Objective: Press the button and escape!";
                 player.stealText.fontStyle = FontStyles.Italic | FontStyles.Underline;
                 return;
            }
        }
    }
}
