using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float run_speed = 100f;
    [SerializeField] private float jump_force = 10f;

    [Header("Components")]
    private Rigidbody2D _rigid_body;
    private Animator _animator;
    [SerializeField] private Transform ground_check;
    [SerializeField] private LayerMask what_is_ground;


    [Header("Internal Movement")]
    [SerializeField] private float movement = 0f;
    [SerializeField] private bool movement_enabled = true;
    [SerializeField] private bool jumping = false;
    [SerializeField] private bool grounded = false;
    [SerializeField] private float grounded_radius = .2f;


    // Start is called before the first frame update
    void Start()
    {
        _rigid_body = GetComponent<Rigidbody2D>();
        movement_enabled = true;
    }

    void handle_user_input()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumping = true;
        }

        // Touchscreen
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    jumping = true;
                }
            }
        }
    }

    void check_grounded()
    {
        bool was_grounded = grounded;
        grounded = false;

        var colliders = Physics2D.OverlapCircleAll(ground_check.position, grounded_radius, what_is_ground);
        foreach (var col in colliders)
        {
            if (col.gameObject != gameObject)
            {
                grounded = true;
                if (!was_grounded)
                {
                    // TODO: Landing event
                    // TODO: Particles
                    // TODO: Sound
                }
            }
        }
    }

    void handle_jump(){
        if (grounded && jumping){
            jumping = false;
            grounded = false;
            // TODO: Particle for jump
            _rigid_body.velocity = new Vector2(_rigid_body.velocity.x, 0f);
            _rigid_body.velocity += Vector2.up * jump_force;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!movement_enabled)
        {
            return;
        }

        handle_user_input();
        check_grounded();
        // print("Position: " + transform.position.x.ToString() + " - Velocity: " + _rigid_body.velocity.ToString());
        // _rigid_body.velocity = new Vector2(movement, _rigid_body.velocity.y);

        // _rigid_body.velocity = Vector3.SmoothDamp(_rigid_body.velocity, targetVelocity, ref sg, 0.05f);
    }

    void FixedUpdate()
    {
        if (movement_enabled)
        {
            movement = run_speed;
        }
        else
        {
            movement = 0f;
        }
        
        // TODO: Play animation based on speed
        // Move
        // print("Position: " + transform.position.x.ToString() + " - Velocity: " + _rigid_body.velocity.ToString());
        // var targetVelocity = new Vector2(5f, _rigid_body.velocity.y);
        // var sg = Vector2.zero;
        // _rigid_body.velocity = Vector2.SmoothDamp(_rigid_body.velocity, targetVelocity, ref sg, 0.05f);
        _rigid_body.velocity = new Vector2(movement, _rigid_body.velocity.y);
        handle_jump();
        // Jump
    }
}
