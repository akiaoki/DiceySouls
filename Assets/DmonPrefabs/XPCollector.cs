using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class XPCollector : MonoBehaviour
{
    [SerializeField]
    private Slider XPslider;
    [SerializeField]
    private Text XPtext;
    private int Lvl=1;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 26)
        {
            collision.gameObject.transform.parent.gameObject.SetActive(false);      
            XPslider.value = XPslider.value + 1;
            if (XPslider.value== XPslider.maxValue)
            {
                Lvl++;
                XPslider.value = 0;
                XPslider.maxValue = XPslider.maxValue + Lvl+1;
            }
            XPtext.text = XPslider.value.ToString();
        }
    }



}
