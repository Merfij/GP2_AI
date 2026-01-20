using TMPro;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    [SerializeField] private AudioClip buttonPressSound;
    [SerializeField] private AudioClip slidingDoorSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PlayerMovement player;
    private bool buttonPressed = false;
    private bool isDoorOpen;
    [SerializeField] private GameObject slidingDoors_1;
    [SerializeField] private GameObject slidingDoors_2;
    public float doorTimer;
    public float resetDoorTextTimer;

    private bool isWithinRange;

    private void Start()
    {
        isWithinRange = false;
        isDoorOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWithinRange && buttonPressed == false)
        {
            player.buttonText.SetActive(true);
            if (player.isPressingButton && player.isBeingChased == false)
            {
                audioSource.PlayOneShot(buttonPressSound);
                player.isPressingButton = false;
                buttonPressed = true;
                isDoorOpen = true;
                player.buttonText.SetActive(false);
            }
        } else {
            player.buttonText.SetActive(false);
        }

        if (isDoorOpen)
        {
            doorTimer -= Time.deltaTime;
            if (doorTimer <= 0f)
                isDoorOpen = false;
            audioSource.PlayOneShot(slidingDoorSound, 0.2f);
            slidingDoors_1.transform.position = new Vector3(slidingDoors_1.transform.position.x - 5f * Time.deltaTime, slidingDoors_1.transform.position.y, slidingDoors_1.transform.position.z);
            slidingDoors_2.transform.position = new Vector3(slidingDoors_2.transform.position.x + 5f * Time.deltaTime, slidingDoors_2.transform.position.y, slidingDoors_2.transform.position.z);
            player.buttonText.SetActive(false);
        }

        if (buttonPressed)
        {
            resetDoorTextTimer -= Time.deltaTime;
            if (resetDoorTextTimer <= 0f)
            {
                resetDoorTextTimer = 10f;
                buttonPressed = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isWithinRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isWithinRange = false;
        }
    }
}
