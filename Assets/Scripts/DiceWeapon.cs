using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceWeapon : MonoBehaviour
{
    // Never do like that
    public static float CurrentCharge;
    public static Vector3 DiceImpulse;
    public static GameObject CurrentDice = null;
    
    public Transform viewTransform;

    public Vector3 diceImpulse;
    public float minChargeTime = 1.0f;
    public float maxChargeTime = 3.0f;
    public float rollAnimationSpeed = 1.0f;
    public Vector3 rollEuler;
    
    private ProjectileWeapon _weapon;
    private WeaponLaserSight _laser;

    private float _chargeTime;
    private bool _playRotationAnimation = false;
    
    private void Awake()
    {
        _weapon = GetComponent<ProjectileWeapon>();
        _laser = GetComponent<WeaponLaserSight>();
    }

    private void TryFire()
    {
        if (_playRotationAnimation)
        {
            if (_chargeTime >= minChargeTime && !CurrentDiceExists)
            {
                CurrentCharge = Mathf.Min(_chargeTime, maxChargeTime);
                _weapon.WeaponState.ChangeState(Weapon.WeaponStates.WeaponUse);
            }
        }
        
        _chargeTime = 0.0f;
        _playRotationAnimation = false;
    }

    private void Update()
    {
        DiceImpulse = diceImpulse;
        
        if (_playRotationAnimation)
        {
            viewTransform.rotation *= Quaternion.Euler(rollEuler * rollAnimationSpeed * _chargeTime);
        }
        
        if (!CurrentDiceExists)
        {
            if (!viewTransform.gameObject.activeSelf)
            {
                viewTransform.gameObject.SetActive(true);
            }
        }
        if (CurrentDiceExists)
        {
            if (viewTransform.gameObject.activeSelf)
            {
                viewTransform.gameObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        switch (_weapon.WeaponState.CurrentState)
        {
            case Weapon.WeaponStates.WeaponDelayBeforeUse:
                if (CurrentDiceExists)
                {
                    break;
                }
                _playRotationAnimation = true;
                _chargeTime = Mathf.Min(_chargeTime + Time.fixedDeltaTime, maxChargeTime);
                break;
            case Weapon.WeaponStates.WeaponUse:
                TryFire();
                break;
            case Weapon.WeaponStates.WeaponStop:
                TryFire();
                break;
            case Weapon.WeaponStates.WeaponInterrupted:
                TryFire();
                break;
            case Weapon.WeaponStates.WeaponIdle:
                TryFire();
                break;
            case Weapon.WeaponStates.WeaponDelayBetweenUses:
                TryFire();
                break;
        }
    }

    private bool CurrentDiceExists => CurrentDice != null && CurrentDice.activeSelf;


}
