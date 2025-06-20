using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHiding : MonoBehaviour
{
    private bool isHiding = false;
    private CharacterController controller;
    private Camera playerCamera;
    private MonoBehaviour[] movementScripts; // ��: MouseLook, PlayerMovement ��

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

        // ������ ������ ��ũ��Ʈ�� ���� ���
        movementScripts = GetComponents<MonoBehaviour>();
    }

    public void ToggleHiding(Transform hidingPoint)
    {
        if (isHiding)
        {
            Debug.Log("���忡�� ����");

            controller.enabled = false;
            transform.position = hidingPoint.position + hidingPoint.forward * 1.1f; // ��¦ ��
            transform.rotation = hidingPoint.rotation;
            controller.enabled = true;

            SetMovementEnabled(true);
            isHiding = false;
        }
        else
        {
            Debug.Log("���忡 ����");

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

