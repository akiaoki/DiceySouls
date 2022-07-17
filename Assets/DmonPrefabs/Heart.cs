using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;
using System;
using MoreMountains.Feedbacks;
namespace MoreMountains.TopDownEngine
{
    public class Heart : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.MMGetComponentNoAlloc<Health>().GetHealth(1, gameObject);
                gameObject.SetActive(false);
            }
        }
    }
}
