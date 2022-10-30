using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class hand : MonoBehaviour
{



    

    //physics movement
    [Space]
    [SerializeField] private ActionBasedController controller;
    [SerializeField] private float followSpeed = 30f;
    [SerializeField] private float rotateSpeed= 100f;
    [Space]
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;
    [Space]
    [SerializeField] private Transform palm;
    [SerializeField] private float reachDistance = 0.1f, joinDisctance = .05f; //reach distance for grabbing things joinDistance for visual 
    [SerializeField] LayerMask grabbableLayer;



    private Transform _followTarget;
    private Rigidbody _body;
    private bool _isGrabbing;
    private GameObject _heldObject;
    private Transform _grabPoint;
    private FixedJoint _joint1, _joint2;
    // Start is called before the first frame update
    void Start()
    {
        

        // physics movement
        _followTarget = controller.gameObject.transform;
        _body = GetComponent<Rigidbody>();
        _body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _body.interpolation = RigidbodyInterpolation.Interpolate;
        _body.mass = 20f;
        _body.maxAngularVelocity = 20f; // default by 7 causing hand delay

        //input setup
        controller.selectAction.action.started += Grab;
        controller.selectAction.action.canceled+= Release;


        //teleport hands
        _body.position = _followTarget.position;
        _body.rotation = _followTarget.rotation;


    }

    private void Grab(InputAction.CallbackContext context)
    {
        if (_isGrabbing || _heldObject)
            return;
        Collider[] grabbablecolliders = Physics.OverlapSphere(palm.position, reachDistance, grabbableLayer);
        if (grabbablecolliders.Length < 1)
            return;

        var objectToGrab = grabbablecolliders[0].transform.gameObject;

        var ObjectBody = objectToGrab.GetComponent<Rigidbody>();

        if (ObjectBody != null)
        {
            _heldObject = ObjectBody.gameObject;
        }
        else
        {
             ObjectBody = objectToGrab.GetComponentInParent<Rigidbody>();
            if (ObjectBody != null)
                _heldObject = ObjectBody.gameObject;
            else
                return;

        }


        StartCoroutine(GrabObject(grabbablecolliders[0],ObjectBody));
    }
    private IEnumerator GrabObject(Collider collider, Rigidbody targetBody)
    {
        _isGrabbing = true;
        //Create a grab point
        _grabPoint = new GameObject().transform;
        _grabPoint.position = collider.ClosestPoint(palm.position);
        _grabPoint.parent = _heldObject.transform;
        //move hand to grab point
        _followTarget = _grabPoint;
        //wait for hand to reach grab point
        while(_grabPoint != null && Vector3.Distance(_grabPoint.position,palm.position)> joinDisctance && _isGrabbing)
        {
            yield return new WaitForEndOfFrame();

        }
        //freeze hand and object motion
        _body.velocity = Vector3.zero;
        _body.angularVelocity = Vector3.zero;
        targetBody.velocity = Vector3.zero;
        targetBody.angularVelocity = Vector3.zero;

        targetBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        targetBody.interpolation = RigidbodyInterpolation.Interpolate;

        // attach joints 
        _joint1 = gameObject.AddComponent<FixedJoint>();
        _joint1.connectedBody = targetBody;
        _joint1.breakForce = float.PositiveInfinity;
        _joint1.breakTorque = float.PositiveInfinity;
        
        _joint1.connectedMassScale = 1;
        _joint1.massScale = 1;
        _joint1.enableCollision = false; // disable collision
        _joint1.enablePreprocessing = false;

        //from the object to the hand
        _joint2 = _heldObject.AddComponent<FixedJoint>();
        _joint2.connectedBody = targetBody;
        _joint2.breakForce = float.PositiveInfinity;
        _joint2.breakTorque = float.PositiveInfinity;

        _joint2.connectedMassScale = 1;
        _joint2.massScale = 1;
        _joint2.enableCollision = false; // disable collision
        _joint2.enablePreprocessing = false;

        //reset follow target
        _followTarget = controller.gameObject.transform;

    }

    private void Release(InputAction.CallbackContext context)
    {
        if (_joint1 != null)
            Destroy(_joint1);
        if (_joint1 != null)
            Destroy(_joint2);
        if (_grabPoint != null)
            Destroy(_grabPoint.gameObject);

        if(_heldObject != null)
        {
            var targetBody = _heldObject.GetComponent<Rigidbody>();
            targetBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            targetBody.interpolation = RigidbodyInterpolation.None;
            _heldObject = null;
        }
        _isGrabbing = false;
        _followTarget = controller.gameObject.transform;

    }
    // Update is called once per frame
    void Update()
    {
    

        Physicsmove();
    }

    private void Physicsmove()
    {
        //position
        // offset
        var postionwithOffset = _followTarget.TransformPoint(positionOffset);
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



    
}
