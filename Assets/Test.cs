using NaughtyAttributes;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class Test : MonoBehaviour
{

    [SerializeField] Transform target;

    [SerializeField] float amplitude;
    [SerializeField] float jumpHeight = 2;
    [SerializeField] float jumpDuration = .5f;


    [Button]
    public void Jump()
    {
        StartCoroutine(Jumping());
        IEnumerator Jumping()
        {
            float targetTime = jumpDuration;
            float elapsedTime = 0;
            var curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(jumpDuration / 2, jumpHeight), new Keyframe(jumpDuration, 0));
            curve.preWrapMode = WrapMode.Clamp;
            curve.postWrapMode = WrapMode.Clamp;
            while (elapsedTime < targetTime)
            {
                yield return null;
                elapsedTime += Time.deltaTime;
                var newPos = transform.position;
                newPos.y = curve.Evaluate(elapsedTime) + transform.position.y * Time.deltaTime;
                transform.position = newPos;
            }
        }
    }
}
