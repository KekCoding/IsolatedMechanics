using UnityEngine;
using UnityEngine.Splines;
using System.Collections;
using Unity.Mathematics;

public class SplineFollower : MonoBehaviour
{
    [SerializeField] SplineContainer m_container;

    Coroutine m_followSplineCoroutine;

    bool m_endReached;

    public float Speed;

    public void StartFollowing(Transform playerTr)
    {
        m_endReached = false;
        FixPos(playerTr);
        m_followSplineCoroutine = StartCoroutine(FollowSpline());
    }

    public void StopFollowing()
    {
        m_endReached = true;
        StopCoroutine(m_followSplineCoroutine);
    }

    void FixPos(Transform playerTr)
    {
        transform.position = playerTr.position;
        SplineUtility.GetNearestPoint(m_container.Spline, m_container.transform.worldToLocalMatrix.MultiplyPoint3x4(transform.position), out float3 pos, out float t);
        transform.position = m_container.transform.localToWorldMatrix.MultiplyPoint3x4(pos);
    }

    IEnumerator FollowSpline()
    {
        if (m_container == null) yield break;
        while (!m_endReached)
        {
            yield return null;
            float distance = SplineUtility.GetNearestPoint(m_container.Spline, m_container.transform.worldToLocalMatrix.MultiplyPoint3x4(transform.position), out float3 pos, out float t);
            HoldPosOnRail((Vector3)pos);
            MoveForward();
            CheckEndRail();
        }
    }

    void HoldPosOnRail(Vector3 pos)
    {
        transform.position = m_container.transform.localToWorldMatrix.MultiplyPoint3x4(pos);
    }

    void MoveForward()
    {
        transform.position += Speed * Time.deltaTime * transform.up;
    }

    void CheckEndRail()
    {
        var endPos = m_container.Spline.EvaluatePosition(1.0f);
        Vector3 worldEndPosition = m_container.transform.localToWorldMatrix.MultiplyPoint3x4((Vector3)endPos);
        if (Vector3.SqrMagnitude(worldEndPosition - transform.position) < .1f)
            m_endReached = true;
    }
    
}
