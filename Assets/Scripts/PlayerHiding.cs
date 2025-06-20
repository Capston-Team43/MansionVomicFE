using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHiding : MonoBehaviour
{
    private bool isHiding = false;
    private CharacterController controller;
    private Camera playerCamera;
    private MonoBehaviour[] movementScripts; // 예: MouseLook, PlayerMovement 등

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

        // 움직임 제어할 스크립트들 수동 등록
        movementScripts = GetComponents<MonoBehaviour>();
    }

    public void ToggleHiding(Transform hidingPoint)
    {
        if (isHiding)
        {
            Debug.Log("옷장에서 나옴");

            controller.enabled = false;
            transform.position = hidingPoint.position + hidingPoint.forward * 1.1f; // 살짝 앞
            transform.rotation = hidingPoint.rotation;
            controller.enabled = true;

            SetMovementEnabled(true);
            isHiding = false;
        }
        else
        {
            Debug.Log("옷장에 숨음");

            controller.enabled = false;
            transform.SetPositionAndRotation(hidingPoint.position, hidingPoint.rotation);
            controller.enabled = true;

            SetMovementEnabled(false);
            isHiding = true;
        }
    }

    void SetMovementEnabled(bool enabled)
    {
        foreach (var script in movementScripts)
        {
            if (script == this) continue;
            script.enabled = enabled;
        }
    }

    public bool IsHiding()
    {
        return isHiding;
    }
}

