using UnityEngine;


public class RailDetector : MonoBehaviour
{
    [SerializeField] LayerMask railMask;

    public GameObject RailCol;
    private void OnTriggerEnter(Collider other)
    {
        if ((railMask & (1 << other.gameObject.layer)) != 0)
            RailCol = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if ((railMask & (1 << other.gameObject.layer)) != 0)
            RailCol = null;
    }
}
