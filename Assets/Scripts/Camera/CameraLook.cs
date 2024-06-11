using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] float sensitivityX;
    [SerializeField] float sensitivityY;
    float m_rotY;
    float m_rotX;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivityY;

        m_rotY += mouseX * Time.deltaTime;
        m_rotX -= mouseY * Time.deltaTime;

        var localRotation = Quaternion.Euler(m_rotX, m_rotY, 0.0f);
        transform.rotation = localRotation;
    }
}
