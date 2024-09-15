using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Nilai : MonoBehaviour
{
    private int N = DrawWithMouse.jmlN;

    void Update()
    {
        if (N != 0) {
            N = DrawWithMouse.jmlN;
            Debug.Log(DrawWithMouse.jmlN);
        }
    }

}
