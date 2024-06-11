using System.Collections;
using UnityEngine;

public class PlayerRail : MonoBehaviour
{
    CharacterController m_cc;
    SplineFollower m_currentSplineFollower;
    [SerializeField] RailDetector leftDetector;
    [SerializeField] RailDetector rightDetector;

    [SerializeField] LayerMask railMask;

    public bool IsSliding => m_isSliding;
    bool m_isSliding;

    [SerializeField] float railSpeed = 5f;
    [SerializeField] float switchRailDuration = .5f;
    [SerializeField] float switchRailHeight = 2f;

    private void Awake()
    {
        m_cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if(IsOnRail() && !m_isSliding)
        {
            GetRail();
            StartSliding();
        }

        if(m_isSliding)
        {
            ChangeLeftRail();
            ChangeRightRail();
        }
    }

    void ChangeLeftRail()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            if(rightDetector.RailCol != null)
            {
                m_currentSplineFollower.StopFollowing();
                m_currentSplineFollower = rightDetector.RailCol.GetComponentInChildren<SplineFollower>();
                StartSliding();
                StartCoroutine(ChangeRailAnim(false));
                Debug.Log("ChangeRail");
            }
        }
    }

    void ChangeRightRail()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (leftDetector.RailCol != null)
            {
                m_currentSplineFollower.StopFollowing();
                m_currentSplineFollower = leftDetector.RailCol.GetComponentInChildren<SplineFollower>();
                StartSliding();
                StartCoroutine(ChangeRailAnim(true));
                Debug.Log("ChangeRail");
            }
        }
    }

    IEnumerator ChangeRailAnim(bool leftOrRight)
    {
        float targetTime = switchRailDuration;
        float elapsedTime = 0;
        var curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(switchRailDuration / 2, switchRailHeight), new Keyframe(switchRailDuration, 0));
        curve.preWrapMode = WrapMode.Clamp;
        curve.postWrapMode = WrapMode.Clamp;
        float speed = (m_currentSplineFollower.transform.position - transform.position).magnitude / targetTime;
        while (elapsedTime < targetTime)
        {
            yield return null;
            transform.position += Time.deltaTime * speed * (leftOrRight ? -transform.right : transform.right);
            elapsedTime += Time.deltaTime;
            var newPos = transform.position;
            newPos.y = curve.Evaluate(elapsedTime) + transform.position.y * Time.deltaTime;
            transform.position = newPos;
        }
    }

    void GetRail()
    {
        var cols = Physics.OverlapSphere(transform.position, .1f, railMask);
        m_currentSplineFollower = cols[0].transform.GetComponentInChildren<SplineFollower>();
    }

    void StartSliding()
    {
        m_isSliding = true;
        m_cc.enabled = false;
        m_currentSplineFollower.Speed = railSpeed;
        m_currentSplineFollower.StartFollowing(transform);
        transform.SetParent(m_currentSplineFollower.transform);
    }

    public bool IsOnRail()
    {
        return Physics.CheckSphere(transform.position, .1f, railMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, .1f);
    }
}
