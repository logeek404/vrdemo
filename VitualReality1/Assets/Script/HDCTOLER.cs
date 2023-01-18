using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedController))]
public class handController : MonoBehaviour
{
    ActionBasedController controller;
    public hand _hand;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<ActionBasedController>();
    }

    // Update is called once per frame
    void Update()
    {
        _hand.SetGrip(controller.selectAction.action.ReadValue<float>()); 
        _hand.SetTrigger(controller.activateAction.action.ReadValue<float>());

    }


}