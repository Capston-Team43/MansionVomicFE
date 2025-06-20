using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    private Animation anim;
    private bool isOpen = false; // ���� ���� ���� �ִ� ����
    private string closeAnimName;
    private string openAnimName;
    private bool canInteract = false; // �÷��̾ �� ��ó�� �ִ��� ����

    void Start()
    {
        anim = GetComponent<Animation>();
        if (anim == null)
        {
            Debug.LogError("Cannot find the Animation" + gameObject.name);
            return;
        }

        // �ִϸ��̼� Ŭ�� �̸� ��������
        int index = 0;
        foreach (AnimationState state in anim)
        {
            if (index == 0) closeAnimName = state.name;
            else if (index == 1) openAnimName = state.name;
            index++;
        }

        if (string.IsNullOrEmpty(closeAnimName) || string.IsNullOrEmpty(openAnimName))
        {
            Debug.LogError("Unavailable Animation clip" + gameObject.name);
        }
    }

    void Update()
    {
        // �÷��̾ �� ��ó�� ���� ���� E Ű �Է� ����
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            ToggleObject();
        }
    }

    void ToggleObject()
    {
        if (anim == null || string.IsNullOrEmpty(closeAnimName) || string.IsNullOrEmpty(openAnimName)) return;

        if (isOpen)
        {
            anim.Play(closeAnimName); // Close �ִϸ��̼� ����
        }
        else
        {
            anim.Play(openAnimName); // Open �ִϸ��̼� ����
        }

        DrawerSoundManager.Instance?.PlayDrawerSound();
        isOpen = !isOpen; // ���� ����
    }

    // �÷��̾ �� ��ó�� ���� ��
    private void OnTriggerEnter(Collider other)
    {
        // CharacterController�� �����ϰ� Collider�� ����
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log(other.gameObject.name + "��(��) �� Ʈ���ſ� ����!");
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log(other.gameObject.name + "��(��) �� Ʈ���ſ��� ����!");
            canInteract = false;
        }
    }

}
