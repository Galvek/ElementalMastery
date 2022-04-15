using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerDirectionOptions PlayerDirection;
    [SerializeField] InteractDirectionOptions InteractDirection;
    //public MoveDirectionOptions MoveDirection;
    [SerializeField] float MoveSpeed = 3f;
    [SerializeField] Vector3 Direction;
    [SerializeField] Animator Animator;

    CircleCollider2D WallCollider;
    BoxCollider2D InteractCollider;
    Rigidbody2D RigBody;

    // Start is called before the first frame update
    void Start()
    {
        WallCollider = gameObject.GetComponentInChildren<CircleCollider2D>();
        InteractCollider = gameObject.GetComponentInChildren<BoxCollider2D>();
        RigBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        InteractDirection = CalcPlayerInteractDirection(Input.mousePosition.x, Input.mousePosition.y, Screen.width, Screen.height);
        switch (InteractDirection)
        {
            case InteractDirectionOptions.OUT_OF_BOUNDS:
                break;
            case InteractDirectionOptions.UP:
                //moves the collider to the top of the player
                InteractCollider.transform.localPosition = new Vector3(0.2f, 0.3f, 0);
                break;
            case InteractDirectionOptions.DOWN:
                //moves the collider to the bottom of the player
                InteractCollider.transform.localPosition = new Vector3(0.2f, -0.1f, 0);
                break;
            case InteractDirectionOptions.LEFT:
                //sets the player direction left facing
                gameObject.transform.localScale = new Vector3(-1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                //moves the collider to the left of the player
                InteractCollider.transform.localPosition = new Vector3(0.5f, 0.125f, 0);
                break;
            case InteractDirectionOptions.RIGHT:
                //sets the player direction right facing
                gameObject.transform.localScale = new Vector3(1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                //moves the collider to the right of the player
                InteractCollider.transform.localPosition = new Vector3(0.5f, 0.125f, 0);
                break;
        }

        PlayerDirection = CalcPlayerLookDirection(Input.mousePosition.x, Screen.width);
        switch (PlayerDirection)
        {
            case PlayerDirectionOptions.OUT_OF_BOUNDS:
                break;
            case PlayerDirectionOptions.LEFT:
                //sets the player direction left facing
                gameObject.transform.localScale = new Vector3(-1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                break;
            case PlayerDirectionOptions.RIGHT:
                //sets the player direction right facing
                gameObject.transform.localScale = new Vector3(1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                break;
        }

        bool isMoving = false;
        Direction = Vector3.zero;

        Direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        if (Direction != Vector3.zero)
        {
            isMoving = true;
        }
        //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        //{
        //    //MoveDirection = MoveDirectionOptions.UP;
        //    //transform.position += new Vector3(0, MoveSpeed, 0) * Time.deltaTime;
        //    Direction = new Vector3(0, 1, 0);
        //    isMoving = true;
        //    Animator.SetBool("IsMoving", isMoving);
        //}

        //if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        //{
        //    //MoveDirection = MoveDirectionOptions.DOWN;
        //    //transform.position += new Vector3(0, -MoveSpeed, 0) * Time.deltaTime;
        //    Direction = new Vector3(0, -1, 0);
        //    isMoving = true;
        //    Animator.SetBool("IsMoving", isMoving);
        //}

        //if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        //{
        //    //MoveDirection = MoveDirectionOptions.LEFT;
        //    //transform.position += new Vector3(-MoveSpeed, 0, 0) * Time.deltaTime;
        //    Direction = new Vector3(-1, 0, 0);
        //    isMoving = true;
        //    Animator.SetBool("IsMoving", isMoving);
        //}

        //if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        //{
        //    //MoveDirection = MoveDirectionOptions.RIGHT;
        //    //transform.position += new Vector3(MoveSpeed, 0, 0) * Time.deltaTime;
        //    Direction = new Vector3(1, 0, 0);
        //    isMoving = true;
        //    Animator.SetBool("IsMoving", isMoving);
        //}
        RigBody.MovePosition(transform.position + (Direction * MoveSpeed * Time.deltaTime));
        Animator.SetBool("IsMoving", isMoving);
    }

    //public enum MoveDirectionOptions
    //{
    //    NOT_MOVING,
    //    UP,
    //    DOWN,
    //    LEFT,
    //    RIGHT
    //}

    PlayerDirectionOptions CalcPlayerLookDirection(float x, float width)
    {
        return (x > width / 2) ? PlayerDirectionOptions.RIGHT : PlayerDirectionOptions.LEFT;
    }

    public enum PlayerDirectionOptions
    {
        OUT_OF_BOUNDS,
        LEFT,
        RIGHT
    }

    /// <summary>
    /// Calculates the direction the player is looking.
    /// </summary>
    /// <param name="x">X coordinate of the point</param>
    /// <param name="y">Y corrdinate of the point</param>
    /// <param name="width">Width of the screen</param>
    /// <param name="height">Height of the screen</param>
    /// <returns>Player Look Direction</returns>
    InteractDirectionOptions CalcPlayerInteractDirection(float x, float y, float width, float height)
    {
        float y1 = height * x / width;
        float y2 = height - y1;
        return (
            x < 0 || width <= x || y < 0 || height <= y
                ? InteractDirectionOptions.OUT_OF_BOUNDS
                : y < y1
                    ? (y < y2
                        ? InteractDirectionOptions.DOWN
                        : InteractDirectionOptions.RIGHT)
                    : (y < y2
                        ? InteractDirectionOptions.LEFT
                        : InteractDirectionOptions.UP)
          );
    }

    public enum InteractDirectionOptions
    {
        OUT_OF_BOUNDS,
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
}
