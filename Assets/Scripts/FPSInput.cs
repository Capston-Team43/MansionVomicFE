using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]

public class FPSInput : MonoBehaviour
{
    public float speed = 6.0f;
    public float gravity = -9.8f;

    public float runMultiplier = 1.5f;
    public float crouchMultiplier = 0.5f;
    public float crouchHeightFactor = 0.5f;

    private CharacterController _charController;
    private float originalSpeed;
    private bool isCrouching = false;

    private float originalHeight;
    private Vector3 originalCenter;

    private Transform cameraTransform;
    private Vector3 originalCameraLocalPos;

    // Start is called before the first frame update
    void Start()
    {
        _charController = GetComponent<CharacterController>();
        originalSpeed = speed;
        originalHeight = _charController.height;
        originalCenter = _charController.center;

        // 메인 카메라 참조
        cameraTransform = Camera.main.transform;
        originalCameraLocalPos = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (InventoryUI.IsInventoryOpen)
        {
            return;
        }

        HandleCrouch();
        HandleMovement();
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            isCrouching = !isCrouching;

            if (isCrouching)
            {
                _charController.height = originalHeight * crouchHeightFactor;
                //_charController.center = originalCenter * crouchHeightFactor;
                _charController.center = new Vector3(originalCenter.x, originalCenter.y * crouchHeightFactor, originalCenter.z);

                // 카메라 시야 낮추기
                cameraTransform.localPosition = new Vector3(
                    originalCameraLocalPos.x,
                    originalCameraLocalPos.y * crouchHeightFactor,
                    originalCameraLocalPos.z
                );
            }
            else
            {
                transform.position += Vector3.up * 0.5f;
                _charController.height = originalHeight;
                _charController.center = originalCenter;
                //transform.position -= Vector3.up * 0.5f;

                // 카메라 위치 복원
                cameraTransform.localPosition = originalCameraLocalPos;
            }
        }
    }

    void HandleMovement()
    {
        float currentSpeed = originalSpeed;

        if (isCrouching)
        {
            currentSpeed *= crouchMultiplier;
        }
        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed *= runMultiplier;
        }

        float deltaX = Input.GetAxis("Horizontal") * currentSpeed;
        float deltaZ = Input.GetAxis("Vertical") * currentSpeed;

        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, currentSpeed);
        movement.y = gravity;
        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);
        _charController.Move(movement);
    }
}
