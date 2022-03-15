using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController chaController;
    
    [Header("Player Rotation")]
    public float mouseSensitivity = 100;
    public Transform playerCamera;
    private float xRotation;

    [Header("Player Movement")] 
    public float movementSpeed = 12f;
    
    [Header("Gravity")]
    public float gravity = -9.81f;
    public Transform checkGround;
    public float groundRadius = .4f;
    public LayerMask groundMask;
    private Vector3 velocity;
    private bool isGrounded;

    [Header("Jump Height")] 
    public float jumpHeight = 1;
    private void Awake()
    {
        chaController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(checkGround.position, groundRadius, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -70f, 70f);
        
        playerCamera.localRotation = Quaternion.Euler(xRotation,0f,0f);
        transform.Rotate(Vector3.up*mouseX);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        Vector3 move = transform.right * x + transform.forward * z;

        chaController.Move(move*movementSpeed*Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        chaController.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(checkGround.position, groundRadius);
    }
}
