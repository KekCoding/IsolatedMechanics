using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController m_cc;
    PlayerRail m_rail;

    [SerializeField] Transform cameraBase;

    Vector2 m_moveDir;
    Vector3 m_velocity;

    [SerializeField] LayerMask groundMask;

    public bool IsGrounded => m_isGrounded;
    bool m_isGrounded;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float gravityMultiplier = 3f;


    private void Awake()
    {
        m_cc = GetComponent<CharacterController>();
        m_rail = GetComponent<PlayerRail>();
    }

    private void Update()
    {
        m_isGrounded = IsGroundedB();
        if (m_rail.IsSliding) return;
        ApplyGravity();
        Move();
    }

    void ApplyGravity()
    {
        if (m_isGrounded)
            m_velocity.y = -1.0f;
        else
            m_velocity.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
    }

    void Move()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        m_moveDir = GetMoveDir(new Vector2(horizontal, vertical));

        m_velocity = new Vector3(m_moveDir.x * moveSpeed, m_velocity.y, m_moveDir.y * moveSpeed);


        m_cc.Move(m_velocity * Time.deltaTime);
    }

    Vector2 GetMoveDir(Vector2 dir)
    {
        Vector3 cameraFwd = cameraBase.forward;
        Vector3 cameraRight = cameraBase.right;
        cameraFwd.y = 0;
        cameraRight.y = 0;
        var newVector = cameraFwd * dir.y + cameraRight * dir.x;
        return new Vector2(newVector.x, newVector.z);
    }

    bool IsGroundedB()
    {
        return Physics.CheckSphere(transform.position, .1f, groundMask);
    }

}
