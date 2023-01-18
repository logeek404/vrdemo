using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class HandAnimationController : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionAssset;
    [SerializeField] private string controllerName;//right or left aciotn map
    [SerializeField] private string actionNameTrigger;
    [SerializeField] private string actionNameGrip;

    private InputActionMap _actionMap;
    private InputAction _inputActionTrigger;
    private InputAction _inputActionGrip;

    private Animator _handAnimator;


    // Start is called before the first frame update
    void Start()
    {

        //get all of actions 
        _actionMap = actionAssset.FindActionMap(controllerName);


        _inputActionGrip = _actionMap.FindAction(actionNameGrip);
        _inputActionTrigger = _actionMap.FindAction(actionNameTrigger);


        //get animator
        _handAnimator = GetComponent<Animator>();



    }

    private void OnEnable()
    {
        _inputActionGrip.Enable();
        _inputActionTrigger.Enable();

    }   
    
    private void OnDisable()
    {
        _inputActionGrip.Disable();
        _inputActionTrigger.Disable();

    }

    // Update is called once per frame
    void Update()
    {
       


        //recieve data from controller ,then send it to the animator
       

        var triggerValue = _inputActionTrigger.ReadValue<float>();
        var gripValue = _inputActionGrip.ReadValue<float>();

        

        _handAnimator.SetFloat("Grip", gripValue);
        _handAnimator.SetFloat("Trigger", triggerValue);

    }
}
