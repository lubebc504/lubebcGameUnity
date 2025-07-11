using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject PlayerCamera;
    public Vector3 Offset;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        this.transform.position = PlayerCamera.transform.position + Offset;
    }
}