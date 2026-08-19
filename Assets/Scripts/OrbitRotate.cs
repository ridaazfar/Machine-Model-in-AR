using UnityEngine;
using UnityEngine.EventSystems;

public class OrbitRotate : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("Target")]
    public Transform modelToRotate;          // Drag your model's root object here

    [Header("Auto Rotate Settings")]
    public float autoRotateSpeed = 20f;      // degrees per second, horizontal only
    public float resumeDelay = 2f;           // seconds after releasing drag before auto-rotate resumes

    [Header("Manual Orbit Settings")]
    public float horizontalSpeed = 0.3f;
    public float verticalSpeed = 0.3f;
    public float minVerticalAngle = -80f;
    public float maxVerticalAngle = 80f;

    private bool isDragging = false;
    private float timeSinceLastDrag = 0f;
    private float currentVerticalAngle = 0f;

    void Start()
    {
        if (modelToRotate != null)
        {
            currentVerticalAngle = modelToRotate.localEulerAngles.x;
        }
    }

    void Update()
    {
        if (modelToRotate == null) return;

        if (!isDragging)
        {
            timeSinceLastDrag += Time.deltaTime;

            if (timeSinceLastDrag >= resumeDelay)
            {
                modelToRotate.Rotate(Vector3.up, autoRotateSpeed * Time.deltaTime, Space.World);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (modelToRotate == null) return;

        // Horizontal drag orbits around world Y
        float horizontalRotation = eventData.delta.x * horizontalSpeed;
        modelToRotate.Rotate(Vector3.up, -horizontalRotation, Space.World);

        // Vertical drag tilts around local X, clamped
        float verticalRotation = eventData.delta.y * verticalSpeed;
        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle - verticalRotation, minVerticalAngle, maxVerticalAngle);

        Vector3 currentEuler = modelToRotate.localEulerAngles;
        modelToRotate.localRotation = Quaternion.Euler(currentVerticalAngle, currentEuler.y, 0f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        timeSinceLastDrag = 0f;
    }
}