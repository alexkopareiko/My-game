using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour
{
    [Tooltip("Jump Force")]
    public float JumpForce = 10f;

    [Tooltip("Force applied downward when in the air")]
    public float GravityDownForce = 20f;

    [Tooltip("Max movement speed when not grounded")]
    public float MaxSpeedInAir = 10f;

    [Tooltip("Acceleration speed when in the air")]
    public float AccelerationSpeedInAir = 25f;

    [Tooltip("Max movement speed when grounded")]
    public float MaxSpeedOnGround = 10f;

    [Tooltip(
            "Sharpness for the movement when grounded, a low value will make the player accelerate and decelerate slowly, a high value will do the opposite")]
    public float MovementSharpnessOnGround = 15;

    [Tooltip("distance from the bottom of the character controller capsule to test for grounded")]
    public float GroundCheckDistance = 0.05f;

    [Tooltip("Physic layers checked to consider the player grounded")]
    public LayerMask GroundCheckLayers = -1;

    public bool IsGrounded { get; private set; }
    public bool HasJumpedThisFrame { get; private set; }
    public Vector3 CharacterVelocity { get; set; }

    Vector3 m_GroundNormal;
    float m_LastTimeJumped = 0f;

    const float k_GroundCheckDistanceInAir = 0.1f;
    const float k_JumpGroundingPreventionTime = 0.2f;



    private CharacterController _charController;
    void Start()
    {
        _charController = GetComponent<CharacterController>();
    }
    void Update()
    {
        if(!GameManager.gameIsPaused) {
            HasJumpedThisFrame = false;

            GroundCheck();
            Move();
        }
    }

    private void Move()
    {
        bool jumpPressed = Input.GetButtonDown("Jump");

        // converts move input to a worldspace vector based on our character's transform orientation
        Vector3 worldspaceMoveInput = transform.TransformVector(GetMoveInput());


        // jumping
        if (IsGrounded) 
        {
            // calculate the desired velocity from inputs, max speed, and current slope
            Vector3 targetVelocity = worldspaceMoveInput * MaxSpeedOnGround;

            // smoothly interpolate between our current velocity and the target velocity based on acceleration speed
            targetVelocity = GetDirectionReorientedOnSlope(targetVelocity.normalized, m_GroundNormal) *
                                     targetVelocity.magnitude;

            // smoothly interpolate between our current velocity and the target velocity based on acceleration speed
            CharacterVelocity = Vector3.Lerp(CharacterVelocity, targetVelocity,
                MovementSharpnessOnGround * Time.deltaTime);

            if (IsGrounded && jumpPressed)
            {
                // force the crouch state to false
                // start by canceling out the vertical component of our velocity
                CharacterVelocity = new Vector3(CharacterVelocity.x, 0f, CharacterVelocity.z);

                // then, add the jumpSpeed value upwards
                CharacterVelocity += Vector3.up * JumpForce;
                // remember last time we jumped because we need to prevent snapping to ground for a short time
                m_LastTimeJumped = Time.time;
                HasJumpedThisFrame = true;

                // Force grounding to false
                IsGrounded = false;
                m_GroundNormal = Vector3.up;
            }
        }
        else
        {
            // add air acceleration
            CharacterVelocity += worldspaceMoveInput * AccelerationSpeedInAir * Time.deltaTime;

            // limit air speed to a maximum, but only horizontally
            float verticalVelocity = CharacterVelocity.y;
            Vector3 horizontalVelocity = Vector3.ProjectOnPlane(CharacterVelocity, Vector3.up);
            horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, MaxSpeedInAir);
            CharacterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);

            // apply the gravity to the velocity
            CharacterVelocity += Vector3.down * GravityDownForce * Time.deltaTime;
        }
        _charController.Move(CharacterVelocity * Time.deltaTime);

    }

    void GroundCheck()
    {
        // Make sure that the ground check distance while already in air is very small, to prevent suddenly snapping to ground
        float chosenGroundCheckDistance =
            IsGrounded ? (_charController.skinWidth + GroundCheckDistance) : k_GroundCheckDistanceInAir;

        // reset values before the ground check
        IsGrounded = false;
        m_GroundNormal = Vector3.up;
        // only try to detect ground if it's been a short amount of time since last jump; otherwise we may snap to the ground instantly after we try jumping
        if (Time.time >= m_LastTimeJumped + k_JumpGroundingPreventionTime)
        {

            // if we're grounded, collect info about the ground normal with a downward capsule cast representing our character capsule
            if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(),
                _charController.radius, Vector3.down, out RaycastHit hit, chosenGroundCheckDistance))
            {

                // storing the upward direction for the surface found
                m_GroundNormal = hit.normal;

                // Only consider this a valid ground hit if the ground normal goes in the same direction as the character up
                // and if the slope angle is lower than the character controller's limit
                if (Vector3.Dot(hit.normal, transform.up) > 0f &&
                    IsNormalUnderSlopeLimit(m_GroundNormal))
                {

                    IsGrounded = true;

                    // handle snapping to the ground
                    if (hit.distance > _charController.skinWidth)
                    {
                        _charController.Move(Vector3.down * hit.distance);
                    }
                }
            }
        }
    }

    // Gets the center point of the bottom hemisphere of the character controller capsule    
    Vector3 GetCapsuleBottomHemisphere()
    {
        return transform.position + (-transform.up * (_charController.height / 2 - _charController.radius));
    }

    // Gets the center point of the top hemisphere of the character controller capsule    
    Vector3 GetCapsuleTopHemisphere()
    {
        return transform.position + (transform.up * (_charController.height / 2 - _charController.radius));
    }

    // Returns true if the slope angle represented by the given normal is under the slope angle limit of the character controller
    bool IsNormalUnderSlopeLimit(Vector3 normal)
    {
        return Vector3.Angle(transform.up, normal) <= _charController.slopeLimit;
    }

    // Gets a reoriented direction that is tangent to a given slope
    public Vector3 GetDirectionReorientedOnSlope(Vector3 direction, Vector3 slopeNormal)
    {
        Vector3 directionRight = Vector3.Cross(direction, transform.up);
        return Vector3.Cross(slopeNormal, directionRight).normalized;
    }

    public Vector3 GetMoveInput()
    {
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f,
                 Input.GetAxisRaw("Vertical"));

        // constrain move input to a maximum magnitude of 1, otherwise diagonal movement might exceed the max move speed defined
        move = Vector3.ClampMagnitude(move, 1);

        return move;
    }

    void OnDrawGizmosSelected()
    {
        if(_charController)
        {
            // draw bottom sphere of player
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(GetCapsuleBottomHemisphere(), _charController.radius);

            // draw top sphere of player
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(GetCapsuleTopHemisphere(), _charController.radius); 
        }

    }

}