using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_move : MonoBehaviour
{
    [SerializeField] private GameObject _object; //An object camera will follow
    [SerializeField] private Vector3 _distanceFromObject; // Camera's distance from the object

    private void LateUpdate() //Works after all update functions called LateUpdate()
    {
        Vector3 positionToGo = _object.transform.position + _distanceFromObject; //Target position of the camera
        Vector3 smoothPosition = Vector3.Lerp(a: transform.position, b: positionToGo, t: 0.125F); //Smooth position of the camera
        transform.position = smoothPosition;
        transform.LookAt(_object.transform.position); //Camera will look(returns) to the object     
    }

    private void Update()
    {
        _object = GameObject.FindGameObjectWithTag("Player");
    }
}
