using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public GameObject character;
    private AudioSource footsteps;
    private bool isWalking = false;
    public float speed = 4f;
    private float currentSpeed;
    public float gravity = -10f;
    public float jumpForce = 1.5f;
    public bool isGrounded;
    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;
    private Vector3 velocity;
    private PlayerController player;
    public Animator animator;
    private bool prevState;
    private bool firstPressed = false;
    private bool firstReleased = false;
    private float startY;
    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = speed;
        player = gameObject.GetComponent<PlayerController>();
        prevState = player.canShoot;
        startY = character.transform.localPosition.y;
        footsteps = GetComponent<AudioSource>();
        footsteps.pitch = 1.25f;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if ((x != 0 || z != 0) && !isWalking)
        {
            isWalking = true;
            footsteps.Play(0);
        }
        else
        {
            isWalking = false;
            footsteps.Pause();
        }

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(currentSpeed * Time.deltaTime * move);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -1 * gravity);
        }

        if (Input.GetKey(KeyCode.LeftShift) && isGrounded && isWalking)
        {
            animator.SetBool("isRunning", true);
            footsteps.pitch = 2f;
            currentSpeed = speed * 2;
            if (!firstPressed)
            {
                prevState = player.canShoot;
                firstPressed = true;
                firstReleased = false;
            }
            player.canShoot = false;
        }
        else
        {
            footsteps.pitch = 1.25f;
            animator.SetBool("isRunning", false);
            currentSpeed = speed;
            if (!firstReleased)
            {
                firstReleased = true;
                firstPressed = false;
                player.canShoot = prevState;
            }
            
        }
        
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded)
        {
            character.transform.localPosition = new Vector3(character.transform.localPosition.x, -0.3f, character.transform.localPosition.z);
            currentSpeed = speed / 2;
            controller.height = 1f;
            player.GetComponent<CapsuleCollider>().height = 1;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            character.transform.localPosition = new Vector3(character.transform.localPosition.x, startY, character.transform.localPosition.z);
            currentSpeed = speed;
            controller.height = 2;
            player.GetComponent<CapsuleCollider>().height = 2;
            
        }

        /*
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z + 30f);
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z - 30f);

        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.RotateAround(groundCheck.position, Vector3.right, 30f);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            transform.RotateAround(groundCheck.position, Vector3.right, -30f);
        }
        */
    }
}
