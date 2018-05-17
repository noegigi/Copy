using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Tween;

public class Rotate : MonoBehaviour {

    [SerializeField]
    float rotateSpeed;

    public delegate void UpdatePath();
    public static UpdatePath updatePath;

    bool isRotating = false;
	
	void Update() {
        if (Input.GetButtonDown("Rotate")&&!isRotating)
        {
            isRotating = true;
            
            TweenFactory.Tween("rotate", transform.eulerAngles.z, transform.eulerAngles.z-90,rotateSpeed,TweenScaleFunctions.Linear,(f)=>{
                Vector3 rotation = transform.eulerAngles;
                rotation.z = f.CurrentValue;
                transform.eulerAngles = rotation;
            },(f)=> {
                isRotating = false;
                if(updatePath!=null)updatePath();
            });
        }
	}
}
