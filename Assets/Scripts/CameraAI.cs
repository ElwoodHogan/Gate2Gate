using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAI : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float baseCamSpeed =.1f;
    [SerializeField] Vector3 mouseLastFrame;
    [SerializeField] public Transform background;

    private void Update()
    {
        if (!Application.isFocused) return;
        Vector2 mouseChange = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouseLastFrame);
        if (Input.GetMouseButton(2)) transform.Translate(-mouseChange);
        mouseLastFrame = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.mouseScrollDelta.y != 0) cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - ((Input.mouseScrollDelta.y / 1.5f) * 1), 2, 20);
        transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * 1 * baseCamSpeed, Input.GetAxisRaw("Vertical") * 1 * baseCamSpeed, 0));
        background.position = transform.position.Change(0, 0, 20);
    }
}
