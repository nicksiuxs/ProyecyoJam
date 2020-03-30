﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GreenNodeElectricBehaviour : ElectricBehaviour
{
    [Header("Green Node")]
    public float chargeTime;
    public float emptyTime;
    public float remainingChargeTime;
    public float remainingEmptyTime;
    private bool isCharging = false;

    public override void toStart()
    {
        remainingChargeTime = chargeTime;
        remainingEmptyTime = 0f;
    }

    public override void toUpdate()
    {
        // checkIfCharging();

        if(isCharging)
        {
            if(remainingChargeTime > 0f)
            {
                remainingChargeTime -= Time.deltaTime;
            }
            else
            {
                remainingChargeTime = 0f;
            }

            remainingEmptyTime = emptyTime * ((chargeTime - remainingChargeTime) / chargeTime);
        }
        else
        {
            if(remainingEmptyTime > 0f)
            {
                remainingEmptyTime -= Time.deltaTime;
            }
            else
            {
                remainingEmptyTime = 0f;
            }

            remainingChargeTime = chargeTime * ((emptyTime - remainingEmptyTime) / emptyTime);
        }

        if(remainingEmptyTime <= 0f && !isCharging)
        {
            base.alwaysOn = false;
        }
    }

    public override void toOnDrawGizmos()
    {
    }

    public override Collider2D[] getCollidedElements()
    {
        return Physics2D.OverlapCircleAll(base.transform.position, base.electricFieldRange, base.electricLayer);
    }

    public override List<ElectricBehaviour> handleActivate()
    {
        base.isOn = true;
        List<ElectricBehaviour> electricElements = new List<ElectricBehaviour>();
        electricElements.Add(this);

        isCharging = true;

        base.activateParticleSystem();

        return electricElements;
    }

    public override void handleDeactivate()
    {
        if(remainingEmptyTime > 0)
        {
            base.alwaysOn = true;
        }
        else
        {
            base.isOn = false;
            base.alwaysOn = false;   
        }
        isCharging = false;
        base.deactivateParticleSystem();
    }

    private void checkIfCharging()
    {
        List<Collider2D> electricElements = Physics2D.OverlapCircleAll(base.transform.position, base.electricFieldRange, base.electricLayer).ToList();

        Debug.Log(electricElements.ToList().Exists(e => e.gameObject.GetComponent<ElectricBehaviour>().isOn));

        if(electricElements.Exists(e => e.GetComponent<ElectricBehaviour>().isOn && e.GetComponent<ElectricBehaviour>() != this))
        {
            Debug.Log("++++++++++++");
            isCharging = true;
        }
        else
        {
            Debug.Log("-------------");
            isCharging = false;
        }
    }
}
