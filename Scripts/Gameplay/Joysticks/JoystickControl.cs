using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickControl : MonoBehaviour
{
    public static Vector3 direct;

    private Vector3 screen;

    private Vector3 MousePosition => Input.mousePosition - screen / 2;

    private Vector3 startPoint;
    private Vector3 updatePoint;

    [SerializeField] RectTransform joystickBG;
    [SerializeField] RectTransform joystickControl;
    [SerializeField] float magnitude;

    [SerializeField] RectTransform canvasRect;

    private float maxDistanceFromFixedPosition;

    void Awake()
    {
        screen.x = canvasRect.rect.width;
        screen.y = canvasRect.rect.height;
        maxDistanceFromFixedPosition = joystickBG.rect.width;
        direct = Vector3.zero;



        RestTransformJoysitckControl();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPoint = MousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            updatePoint = MousePosition;

            
            float distance = updatePoint.magnitude;
            
            if (distance <= maxDistanceFromFixedPosition)
            {
                joystickControl.anchoredPosition = Vector3.ClampMagnitude(updatePoint, magnitude);
                direct = updatePoint.normalized;
                direct.z = direct.y;
                direct.y = 0;
            }
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            direct = Vector3.zero;
            RestTransformJoysitckControl();
        }
    }


    private void RestTransformJoysitckControl()
    {
        joystickControl.anchoredPosition = Vector3.zero;
    }


    private void OnDisable()
    {
        direct = Vector3.zero;
    }
}
