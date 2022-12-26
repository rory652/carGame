using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarController : MonoBehaviour
{
    //temp
    public UIManager ui;
    public TrackManager track;
    
    public float maxBreakTorque;
    public float maxSteeringAngle;

    public AnimationCurve powerCurve;
    public float horsePower;
    
    public float idleRevs;
    public float maxRevs;
    public float riseTime;
    public float fallTime;
    public float carMass;
    public float differentialRatio;
    
    public List<float> gears;
    
    private Rigidbody _rigidbody;
    private WheelController _wheelController;

    private float _speed;
    
    private float _upShiftRevs;
    private float _downShiftRevs;
    
    private float _revs;
    private float _riseRevs;
    private float _fallRevs;

    private int _gear;
    private int _gears;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _wheelController = GetComponent<WheelController>();

        _rigidbody.mass = carMass;

        _upShiftRevs = maxRevs * 0.8f;
        _downShiftRevs = maxRevs * 0.5f;
        
        _revs = idleRevs;
        
        _riseRevs = (maxRevs - idleRevs) / riseTime; 
        _fallRevs = (maxRevs - idleRevs) / fallTime;

        _gear = 0;
        _gears = gears.Count;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");
        
        var brake = vertical < 0f ? maxBreakTorque * -vertical : 0f;
        var steering = maxSteeringAngle * horizontal;

        var motor = CalculateWheelTorque(vertical);

        _wheelController.WheelUpdate(motor, brake, steering);

        _speed = _rigidbody.velocity.magnitude * 2.237f;

        ui.writeSpeed(_speed);
        ui.writeRevs(_revs);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            track.CheckpointHit(other.name);
        }
    }

    private float CalculateWheelTorque(float throttle)
    {
        var targetRevs = Mathf.Lerp(idleRevs, maxRevs, throttle);

        if (targetRevs > _revs)
        {
            _revs += _riseRevs * Time.deltaTime;
            if (_revs > targetRevs)
            {
                _revs = targetRevs;
            }
            
            if (_revs > _upShiftRevs)
            {
                shiftUp();
            }
        }
        else if (targetRevs < _revs)
        {
            _revs -= _fallRevs * Time.deltaTime;
            if (_revs < targetRevs)
            {
                _revs = targetRevs;
            }
            
            if (_revs < _downShiftRevs)
            {
                shiftDown();
            }
        }
        
        if (_revs == idleRevs)
        {
            return 0f;
        }

        // Find power in W at current Revs
        var power = powerCurve.Evaluate(_revs / (maxRevs - idleRevs)) * horsePower * 745.7f;

        // Engine Torque
        var engineTorque = 9.5488f * power / _revs;

        // Wheel Torque
        return engineTorque * gears[_gear] * differentialRatio;
    }

    private void shiftUp()
    {
        if (_gear == _gears-1)
        {
            return;
        }

        _revs = _revs * gears[_gear + 1] / gears[_gear];
        _gear++;
    }
    
    private void shiftDown()
    {
        if (_gear == 0)
        {
            return;
        }
        
        _revs = _revs * gears[_gear - 1] / gears[_gear];
        _gear--;
    }
}
