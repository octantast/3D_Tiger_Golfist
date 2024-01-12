using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleController : MonoBehaviour
{
    public MeshRenderer circle1;
    public MeshRenderer circle2;

    public Material unactive;

    void Start()
    {
        circle1.material = unactive;
        circle2.material = unactive;

       transform.GetComponent<Collider>().enabled = false;
    }

}
