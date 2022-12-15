using System.Collections;
using UnityEngine;


public class PlayerAnimationController : MonoBehaviour
{
    private Animator m_Animator;

    private void Awake() => m_Animator = GetComponent<Animator>();


    private void Update()
    {
        m_Animator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
        m_Animator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
    }

}