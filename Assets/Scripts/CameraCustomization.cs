using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraCustomization : MonoBehaviour {

    #region PublicVariables

    [Header("Settings")]
    public float smoothTime = 0.2f;
    public float rotationSpeed = 50f;
    public float zoomDampening = 15f;

    [Space]

    [Header("Base Object")]
    public Transform target;
    public Vector3 offset = new Vector3(0f, 0f, -10f);
    public float angle;

    [Space]

    [Header("Parts")]
    public List<Feature> features = new List<Feature>();

    #endregion

    #region PrivateVariables

    private float xDeg;
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    private Quaternion rotation;

    private float touchPosition;
    private Vector3 targetPosition;
    private Vector3 vel;

    #endregion

    private void Start() { Init(); }
    private void OnEnable() { Init(); }

    private void OnValidate()
    {
        if(target == null)
            return;
        // Set Positions
        transform.position = target.position + offset;

        // Set Rotations
        target.rotation = Quaternion.Euler(0, angle, 0);
    }

    private void Init()
    {
        //If there is no target Give Error and return out
        if (!target)
        {
            Debug.LogError("Target not assigned");
            return;
        }

        targetPosition = target.position + offset;
        xDeg = angle;
    }

    private void LateUpdate()
    {
        SetInput();

        // Set Positions
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref vel, smoothTime);

        // Set Rotations
        desiredRotation = Quaternion.Euler(0, xDeg, 0);
        currentRotation = target.rotation;
        target.rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * zoomDampening);
        transform.rotation = rotation;
    }

    private void SetInput()
    {
#if Unity_Android || Unity_IOS
        if ((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            touchPosition = Input.GetTouch(0).deltaPosition.x;
            xDeg -= touchPosition * rotationSpeed * 0.002f;
        }
#else
        // For 
        if (Input.GetMouseButton(0))
        {
	        touchPosition = Input.GetAxis("Mouse X");
	        xDeg -= touchPosition * rotationSpeed * 0.02f;
        }
#endif
	}

    private Feature feature;
    public void PositionCamera(string id)
    {
        feature = features.Find(x => x.id.Equals(id));
        if (feature == null)
            return;

        targetPosition = feature.target.position + feature.offset;
        xDeg = feature.angle;
    }
}

[System.Serializable]
public class Feature
{
    public string id;
    public Transform target;
    public Vector3 offset = new Vector3(0,0,-3);
    public float angle;
}