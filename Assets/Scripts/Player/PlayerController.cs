using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Yarn.Unity;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Vector3 movement;
    private bool canMove;

    private float jumpForce = 8.0f;
    private CharacterController CharacterController;
    public float Speed = 5.0f;
    public float RotationSpeed = 240.0f;
    private float Gravity = 20.0f;
    private Vector3 moveDir = Vector3.zero;
    private bool hasCollided = false;
    private float timeSinceCollided = 0f;

    #region Sound
    [FMODUnity.EventRef]
    public string GrassFootsteps;
    protected FMOD.Studio.EventInstance GrassFootstepsSound;

    public string NormalFootsteps;
    protected FMOD.Studio.EventInstance NormalFootstepsSound;

    private float stepTimer = 0.0f, footstepSpeed = 0.3f;
    #endregion

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GrassFootstepsSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        NormalFootstepsSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GrassFootsteps != null && GrassFootsteps != "")
        {
            GrassFootstepsSound = FMODUnity.RuntimeManager.CreateInstance(GrassFootsteps);
        }
        if (NormalFootsteps != null && NormalFootsteps != "")
        {
            NormalFootstepsSound = FMODUnity.RuntimeManager.CreateInstance(NormalFootsteps);
        }

        canMove = true;
        animator = GetComponentInChildren<Animator>();
        CharacterController = GetComponent<CharacterController>();
        this.SetInitialPos();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void SetInitialPos()
    {
        if (GameController.instance.GetState() == "exploration"
            || (GameController.instance.GetState() == "dialogue"
            && GameController.instance.GetPreviousState() == "exploration"))
        {
            gameObject.transform.position = GameController.instance.playerLastPos;
        }
        else
        {
            gameObject.transform.position = new Vector3(0f, 2f, 0f);
        }
    }

    private void Update()
    {
        InteractionInput();

        if (hasCollided) {
            timeSinceCollided += Time.deltaTime;
            if (timeSinceCollided >= 1) {
                hasCollided = false;
                timeSinceCollided = 0f;
            }
        }

        if ((canMove && GameController.instance.decisions["found_backpack"]) || GameController.instance.testing)
        {
            // Get Input for axis
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // Calculate the forward vector
            Vector3 camForward_Dir = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 move = v * camForward_Dir + h * Camera.main.transform.right;

            if (move.magnitude > 1f) move.Normalize();

            // Calculate the rotation for the player
            move = transform.InverseTransformDirection(move);

            // Get Euler angles
            float turnAmount = Mathf.Atan2(move.x, move.z);

            transform.Rotate(0, turnAmount * RotationSpeed * Time.deltaTime, 0);

            if (CharacterController.isGrounded)
            {
                animator.SetBool("isJumping", false);
                animator.SetFloat("speed", Mathf.Abs(move.magnitude * 100));

                moveDir = transform.forward * move.magnitude;
                moveDir *= Speed;

                if (Input.GetButton("Jump"))
                {
                    animator.SetBool("isJumping", true);
                    moveDir.y = jumpForce;
                }

                PlayFootsteps();
            }

            moveDir.y -= Gravity * Time.deltaTime;
            CharacterController.Move(moveDir * Time.deltaTime);
        }
        else
        {
            GrassFootstepsSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            NormalFootstepsSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    private void PlayFootsteps()
    {
        string currentState = GameController.instance.GetState();

        if (animator.GetFloat("speed") > 0.1f && !animator.GetBool("isJumping"))
        {
            if (stepTimer > footstepSpeed)
            {
                PlaySelectedSound(currentState);
                stepTimer = 0.0f;
            }

            stepTimer += Time.deltaTime;
        }
        else StopSelectedSound(currentState);
    }

    private void PlaySelectedSound(string state)
    {
        if (state == "exploration")
            GrassFootstepsSound.start();
        else
        {
            NormalFootstepsSound.start();
            NormalFootstepsSound.setVolume(70f);
        }

    }

    private void StopSelectedSound(string state)
    {
        if (state == "exploration")
            GrassFootstepsSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        else NormalFootstepsSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    void InteractionInput()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (GameController.instance.GetState() == "dialogue")
            {
                if (GameObject.Find("ContinueButton") != null)
                {
                    GameObject.Find("DialogueRunner").GetComponent<DialogueUI>().MarkLineComplete();
                }
            }
            else
            {
                GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Object");
                for (int i = 0; i < gameObjects.Length; i++)
                {
                    if (gameObjects[i].GetComponent<Interactable>().IsActive())
                    {
                        gameObjects[i].GetComponent<Interactable>().Interact();
                    }
                }
            }
        }
        if (!GameController.instance.testing && (!GameController.instance.decisions["found_backpack"] &&
            GameController.instance.GetState() == "exploration" &&
           (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)))
        {
            GameObject.Find("DialogueRunner").GetComponent<YarnConfigurations>().RunDialogue("Tutorial.Enforcement");
        }
    }

    public void allowMovement(bool move)
    {
        canMove = move;
        animator.SetFloat("speed", 0);
    }

    private void OnControllerColliderHit(ControllerColliderHit other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("Away");
            }
            else if (!hasCollided)
            {
                hasCollided = true;
                GameController.instance.TakeDamage(5);
                Debug.Log("Oh no, I've been hit");
            }
        }
    }

}
