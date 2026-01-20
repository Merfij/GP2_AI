using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Player_Inputs inputActions;
    [SerializeField] private float moveSpeed;
    [SerializeField] private AudioClip whistle;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private DetectPlayer detectPlayerComponent;
    [SerializeField] private List<DetectPlayer> detectPlayerList;
    [SerializeField] private GameObject stealIcon;
    [SerializeField] float stealDistance;

    public bool playerHasKey;
    [SerializeField] public GameObject keyIcon;
    [SerializeField] public TextMeshProUGUI stealText;
    [SerializeField] public GameObject buttonText;

    public bool hasWhistled;
    [SerializeField] public GameObject whistlePOS;

    public bool isPressingButton;

    [SerializeField] private GameObject firstCheckpoint;

    public bool isBeingChased;

    float moveX;
    float moveY;

    private void Awake()
    {
        buttonText.SetActive(false);
        stealIcon.SetActive(false);
        inputActions = new Player_Inputs();
        inputActions.Player.Enable();
        isBeingChased = false;
    }

    private void OnEnable()
    {
        inputActions.Player.Interact.performed += InteractPerformed;
        inputActions.Player.Whistle.performed += WhistlePerformed;
        inputActions.Player.PressButton.performed += PressButtonPerformed;
    }

    private void PressButtonPerformed(InputAction.CallbackContext context)
    {
        isPressingButton = true;
        stealText.fontStyle = FontStyles.Strikethrough;
        StartCoroutine(ResetPressButton());
    }

    private void WhistlePerformed(InputAction.CallbackContext context)
    {
        audioSource.PlayOneShot(whistle);
        hasWhistled = true;
        Instantiate(whistlePOS, transform.position, Quaternion.identity);
        whistlePOS.transform.position = transform.position;
        StartCoroutine(ResetWhistle());
    }

    private void InteractPerformed(InputAction.CallbackContext context)
    {
        if (IsNearKey())
        {
            keyIcon.SetActive(true);
            stealText.fontStyle = FontStyles.Strikethrough;
            playerHasKey = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        foreach (DetectPlayer detectPlayer in detectPlayerList)
        {
            if (detectPlayer.canSeePlayer)
            {
                isBeingChased = true;
                break;
            }
            else
            {
                isBeingChased = false;
            }
        }

        if (IsNearKey())
        {
            stealIcon.SetActive(true);
        }
        else
        {
            stealIcon.SetActive(false);
        }

        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();
        moveX = inputVector.x;
        moveY = inputVector.y;

    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(moveX, 0, moveY).normalized * moveSpeed * Time.deltaTime;
    }

    private void OnDisable()
    {
        inputActions.Player.Interact.performed -= InteractPerformed;
        inputActions.Player.Whistle.performed -= WhistlePerformed;
        inputActions.Player.PressButton.performed -= PressButtonPerformed;
        inputActions.Disable();
    }

    private bool IsNearKey()
    {
        Vector3 distanceToEnemy = transform.position - detectPlayerComponent.transform.position;
        if (distanceToEnemy.magnitude <= stealDistance)
        {
            return true;
        }
        return false;
    }

    IEnumerator ResetWhistle()
    {
        yield return new WaitForSeconds(2f);
        hasWhistled = false;
    }

    IEnumerator ResetPressButton()
    {
        yield return new WaitForSeconds(0.5f);
        isPressingButton = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
