using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TMPro;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class WheelController : MonoBehaviour
{
    public List<AxleInfo> axleInfos;

    private float _motors;

    public void Start()
    {
        _motors = 0f;
        
        foreach (var axleInfo in axleInfos)
        {
            if (axleInfo.motor)
            {
                _motors++;
            }
        }

        if (_motors == 0f)
        {
            Debug.LogError("No motors found");
        }
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
    private void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        var visualWheel = collider.transform.GetChild(0);
        
        collider.GetWorldPose(out var position, out var rotation);

        visualWheel.transform.SetPositionAndRotation(position, rotation);
    }

    public void WheelUpdate(float motor, float brake, float steering)
    {
        motor = motor / _motors;
        
        foreach (var axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }

            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }

            axleInfo.leftWheel.brakeTorque = brake; 
            axleInfo.rightWheel.brakeTorque = brake;


            axleInfo.leftWheel.GetGroundHit(out WheelHit leftWheelHit);
            axleInfo.rightWheel.GetGroundHit(out WheelHit rightWheelHit);
            
            if (Math.Abs(leftWheelHit.sidewaysSlip) > 0.1 || Math.Abs(rightWheelHit.sidewaysSlip) > 0.1)
            {
                Debug.Log($"Side Slip - Left: {leftWheelHit.sidewaysSlip}, Right: {rightWheelHit.sidewaysSlip}, {axleInfo.motor}");   
            }

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }
}