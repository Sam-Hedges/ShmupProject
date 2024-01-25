using UnityEngine;
using Cinemachine;

public class DriveDollyCamera : MonoBehaviour
{
    public float speed = 1f;
    private CinemachineTrackedDolly vcam;

    // Start is called before the first frame update
    void Start()
    {
        var cam = GetComponent<CinemachineVirtualCamera>();
        vcam = cam.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    // Update is called once per frame
    void Update() { vcam.m_PathPosition += speed * Time.deltaTime; }
}
