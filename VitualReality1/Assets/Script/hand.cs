using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hand : MonoBehaviour
{
    public float speed = 1.0f;
    Animator _anim;
    private float gripTarget;
    private float triggerTarget;
    private float gripCurrent;
    private float triggerCurrent;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Animatorhand();
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
            gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * speed);
            _anim.SetFloat("Grip", gripCurrent);
            Debug.Log("SET FLOAT:" + gripCurrent);

        }


        if (triggerCurrent != triggerTarget)
        {
            triggerCurrent = Mathf.MoveTowards(triggerCurrent, triggerTarget, Time.deltaTime * speed);
            _anim.SetFloat("Trigger", triggerCurrent);
            Debug.Log("SET FLOAT:" + gripCurrent);

        }

    }
}