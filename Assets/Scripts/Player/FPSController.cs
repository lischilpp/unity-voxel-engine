using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public Camera playerCamera;
    public float movementSpeed = 5f;
    public float jumpHeight = 0.8f;
    public float gravity = 9.81f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public bool freeFlyMode = false;
    public float freeFlySpeedMultiplier = 4f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;

    float rotationX = 0;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        UpdateMovement();
        UpdateCameraRotation();
    }

    private void UpdateMovement() {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        if (freeFlyMode) {
            if (Input.GetKey(KeyCode.Space)) {
				controller.Move(Vector3.up * movementSpeed * freeFlySpeedMultiplier * Time.deltaTime);
			}else if (Input.GetKey(KeyCode.LeftShift)) {
				controller.Move(Vector3.down * movementSpeed * freeFlySpeedMultiplier * Time.deltaTime);
			}
            move *= freeFlySpeedMultiplier;
        }
        else {
            // Handle jumping
            if (Input.GetButton("Jump") && controller.isGrounded)
            {
                playerVelocity.y = Mathf.Sqrt(2 * jumpHeight * gravity);
            }

            // Apply gravity
            playerVelocity.y -= gravity * Time.deltaTime;

            if (controller.isGrounded && playerVelocity.y < 0)
            {
                // Reset velocity
                playerVelocity.y = 0f;
            }
        }
        controller.Move(move * movementSpeed * Time.deltaTime + playerVelocity * Time.deltaTime);
    }

    private void UpdateCameraRotation() {
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
}
