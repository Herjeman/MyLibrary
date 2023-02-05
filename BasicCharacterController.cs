using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class BasicCharacterController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private CharacterController _characterController;
    private Vector3 _moveAxis;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _moveAxis = Vector2.zero;
    }

    void Update()
    {
        _moveAxis.x = Input.GetAxis("Horizontal");
        _moveAxis.z = Input.GetAxis("Vertical");
        _characterController.Move(_moveAxis * _moveSpeed * Time.deltaTime);
    }
}
