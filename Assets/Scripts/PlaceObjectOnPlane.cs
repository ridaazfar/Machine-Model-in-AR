using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class PlaceObjectOnPlane : MonoBehaviour
{
    public GameObject objectToPlace;
    public MachineController machineController;
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private GameObject placedObject; // tracks the currently placed instance

    void Awake()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        // Skip placement entirely once something's already placed
        if (placedObject != null)
            return;

        Vector2 touchPosition;
        bool tapped = false;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            touchPosition = Input.mousePosition;
            tapped = true;
        }
        else
        {
            touchPosition = default;
        }
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchPosition = Input.GetTouch(0).position;
            tapped = true;
        }
        else
        {
            touchPosition = default;
        }
#endif

        if (tapped && raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            placedObject = Instantiate(objectToPlace, hitPose.position, hitPose.rotation);

            Animator placedAnimator = placedObject.GetComponent<Animator>();
            if (placedAnimator != null && machineController != null)
            {
                machineController.animation = placedAnimator;
            }
        }
    }
}