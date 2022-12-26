using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject toFollow;
    public float camAngle;
    public float camDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float angle = toFollow.transform.rotation.eulerAngles.y;

        Vector3 camVector = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward * camDistance;
        camVector = Quaternion.AngleAxis(-camAngle, Vector3.Cross(camVector, Vector3.up).normalized) * camVector;

        transform.rotation = Quaternion.LookRotation(camVector);
        transform.position = toFollow.transform.position - camVector;
    }
}
