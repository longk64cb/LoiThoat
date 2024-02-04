using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ColliderEventTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent clickEvent;

    private Vector2 orgMousPos;

    private DateTime startClickTime;
    private float minMouseDistSqr;

    private void Start()
    {
        minMouseDistSqr = Mathf.Pow(Screen.width * 0.05f, 2);
    }

    private void OnMouseDown()
    {
        orgMousPos = Input.mousePosition;
        startClickTime = DateTime.Now;
        Debug.Log(orgMousPos);
    }

    private void OnMouseUpAsButton()
    {
        if (IsNotPointerOverUIObject() 
            && (DateTime.Now - startClickTime).TotalSeconds <= 0.4f
            && Vector2.SqrMagnitude(orgMousPos - (Vector2)Input.mousePosition) <= minMouseDistSqr)
            clickEvent.Invoke();
    }

    public static bool IsPointerOverGameObject()
    {
#if UNITY_EDITOR
        // Check mouse
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
#elif UNITY_ANDROID
        // Check touches
        for (int i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    return true;
                }
            }
        }
#endif

        return false;
    }

    //https://answers.unity.com/questions/1115464/ispointerovergameobject-not-working-with-touch-inp.html
    private static bool IsNotPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count == 0;
    }
}
