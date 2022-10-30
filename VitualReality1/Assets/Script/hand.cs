using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hand : MonoBehaviour
{



    //animation
    public float animationspeed=1.0f;
    Animator _anim;
    private float gripTarget;
    private float triggerTarget;
    private float gripCurrent;
    private float triggerCurrent;

    //physics movement
    [SerializeField] private GameObject followObject;
    [SerializeField] private float followSpeed = 30f;
    [SerializeField] private float rotateSpeed= 100f;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;

    private Transform _followTarget;
    private Rigidbody _body;

    // Start is called before the first frame update
    void Start()
    {
        //animation
        _anim = GetComponent<Animator>();

        // physics movement
        _followTarget = followObject.transform;
        _body = GetComponent<Rigidbody>();
        _body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _body.interpolation = RigidbodyInterpolation.Interpolate;
        _body.mass = 20f;

        //teleport hands
        _body.position = _followTarget.position;
        _body.rotation = _followTarget.rotation;


    }

    // Update is called once per frame
    void Update()
    {
        Animatorhand();

        Physicsmove();
    }

    private void Physicsmove()
    {
        //position
        // offset
        var postionwithOffset = _followTarget.position + positionOffset;
        var distance = Vector3.Distance(postionwithOffset, transform.position);
        _body.velocity = (postionwithOffset - transform.position).normalized * followSpeed * distance;


        //rotation
        // offset
        var rotationwithOffset = _followTarget.rotation * Quaternion.Euler(rotationOffset);

      
            
        Debug.Log("rotation: " + rotationwithOffset.eulerAngles.z);
        var q = rotationwithOffset * Quaternion.Inverse(_body.rotation);
        q.ToAngleAxis(out float angle, out Vector3 axis);
        if (angle > 180f)
            angle -= 360f;
        _body.angularVelocity = axis * (angle * Mathf.Deg2Rad * rotateSpeed);



    }

    internal void SetGrip(float readValue)
    {
        gripTarget = readValue;
        Debug.Log("grip value :" + readValue);

    }

    internal void SetTrigger(float readValue)
    {
        triggerTarget = readValue;
        Debug.Log("trigger value :" + readValue);
    }


    void Animatorhand()
    {
        if (gripCurrent != gripTarget)
        {
            gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * animationspeed);
            _anim.SetFloat("Grip", gripCurrent);
            Debug.Log("SET FLOAT:" + gripCurrent);

        }


        if (triggerCurrent != triggerTarget)
        {
            triggerCurrent = Mathf.MoveTowards(triggerCurrent, triggerTarget, Time.deltaTime * animationspeed);
            _anim.SetFloat("Trigger", triggerCurrent);
            Debug.Log("SET FLOAT:" + gripCurrent);

        }

    }
}
