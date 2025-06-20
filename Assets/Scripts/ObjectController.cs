using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    private Animation anim;
    private bool isOpen = false; // 현재 문이 열려 있는 상태
    private string closeAnimName;
    private string openAnimName;
    private bool canInteract = false; // 플레이어가 문 근처에 있는지 여부

    void Start()
    {
        anim = GetComponent<Animation>();
        if (anim == null)
        {
            Debug.LogError("Cannot find the Animation" + gameObject.name);
            return;
        }

        // 애니메이션 클립 이름 가져오기
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
        // 플레이어가 문 근처에 있을 때만 E 키 입력 감지
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
            anim.Play(closeAnimName); // Close 애니메이션 실행
        }
        else
        {
            anim.Play(openAnimName); // Open 애니메이션 실행
        }

        DrawerSoundManager.Instance?.PlayDrawerSound();
        isOpen = !isOpen; // 상태 변경
    }

    // 플레이어가 문 근처에 왔을 때
    private void OnTriggerEnter(Collider other)
    {
        // CharacterController를 제외하고 Collider만 감지
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log(other.gameObject.name + "이(가) 문 트리거에 들어옴!");
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log(other.gameObject.name + "이(가) 문 트리거에서 나감!");
            canInteract = false;
        }
    }

}
