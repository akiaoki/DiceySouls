﻿using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class RigidbodySpawnImpulse : MonoBehaviour
{
    //public Vector3 impulse;
    //public float diceWeaponChargeModifier = 0.35f;

    private Rigidbody _rigidbody;
    private MMPoolableObject _poolable;

    private int _defaultLayer;
    
    private void Awake()
    {
        _defaultLayer = gameObject.layer;
        _rigidbody = GetComponent<Rigidbody>();
        _poolable = GetComponent<MMPoolableObject>();
        if (_poolable != null)
        {
            _poolable.OnSpawnComplete += ResetRigidbody;
        }
    }

    private void ResetRigidbody()
    {
        gameObject.layer = _defaultLayer;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.AddRelativeForce(DiceWeapon.DiceImpulse * DiceWeapon.CurrentCharge, ForceMode.Impulse);
        DiceWeapon.CurrentDice = gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.layer = LayerMask.NameToLayer("IgnoreEntities");
    }

    private IEnumerator ChangeLayerLater()
    {
        yield return new WaitForSeconds(0.01f);
        gameObject.layer = LayerMask.NameToLayer("IgnoreEntities");
    }
}
