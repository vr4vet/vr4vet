/* Copyright (C) 2022 IMTEL NTNU - All Rights Reserved
 * Developer: Jorge Garcia
 * animations made by: Daniel Stringer https://www.youtube.com/watch?v=ijcn-mIJL5s
 * Ask your questions by email: jorgeega@ntnu.no
 */



using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class HandAnimation : MonoBehaviour
{
    [SerializeField] private InputActionReference gripAction;
    [SerializeField] private InputActionReference pinchAction;
    private Animator animator;

    private void OnEnable()
    {
        //grip
        gripAction.action.performed += Gripping;
        gripAction.action.canceled += GripRelease;

        //pinch
        pinchAction.action.performed += Pinching;
        pinchAction.action.canceled += PinchRelease;
    }

    private void Start() => animator = GetComponent<Animator>();

    private void Gripping(InputAction.CallbackContext obj)
    {
        if (animator != null)
            animator.SetFloat("Grip", obj.ReadValue<float>());
    }

    private void GripRelease(InputAction.CallbackContext obj)
    {
        if (animator != null)
            animator.SetFloat("Grip", 0f);
    }
    private void Pinching(InputAction.CallbackContext obj)
    {
        if (animator != null)
            animator.SetFloat("Pinch", obj.ReadValue<float>());
    }

    private void PinchRelease(InputAction.CallbackContext obj)
    {
        if (animator != null)
            animator.SetFloat("Pinch", 0f);
    }
}
