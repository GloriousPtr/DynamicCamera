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

    private float touchPosition;
    private Vector3 targetPosition;
    private Vector3 vel;
    private Feature feature;

    #endregion

    private void Start() { Init(); }
    private void OnEnable() { Init(); }

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

    /// <summary>
    /// For positioning camera, simply call function without Parameter to position camera to base target
    /// </summary>
    /// <param name="id">ID of the Part to focus on</param>
    public void PositionCamera(string id = "")
    {
        // For Base Target
        if (id.Equals(string.Empty))
        {
            if (target == null)
                throw new System.Exception("Kindly Assign Base Target");

            targetPosition = target.position + offset;
            xDeg = angle;
            return;
        }

        // For Parts
        feature = features.Find(x => x.id.Equals(id));

        if (feature == null)
            throw new System.Exception(string.Format("{0} ID Not Found", id));

        if (feature.target == null)
            throw new System.Exception(string.Format("Kindly Assign {0}'s Target", id));

        targetPosition = feature.target.position + feature.offset;
        xDeg = feature.angle;
    }

    /// <summary>
    /// Only for preview mode, Used in editor script
    /// </summary>
    /// <param name="preview"></param>
    /// <param name="id">ID of the Part to focus on</param>
    public void PositionCamera(bool preview, string id = "")
    {
        if (!preview)
            return;

        if (id.Equals(string.Empty))
        {
            if(target == null)
                throw new System.Exception("Kindly Assign Base Target");

            transform.position = target.position + offset;
            target.rotation = Quaternion.Euler(0, angle, 0);
            return;
        }
        
        feature = features.Find(x => x.id.Equals(id));

        if (feature == null)
            throw new System.Exception(string.Format("{0} ID Not Found", id));

        if (feature.target == null)
            throw new System.Exception(string.Format("Kindly Assign {0}'s Target", id));

        transform.position = feature.target.position + feature.offset;
        target.rotation = Quaternion.Euler(0, feature.angle, 0);
    }
}

[System.Serializable]
public class Feature
{
    public string id = "ChangeIdHere";
    public Transform target;
    public Vector3 offset = new Vector3(0,0,-3);
    public float angle;

    // For Editor Only
    public bool toggleFold;
}