using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ShadowCharacterLightBehavior : LightElement
{
    [SerializeField] private float _wallExitWidthModifier;

    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public override bool EnterLight()
    {
        if (Physics.CheckSphere(transform.position, _characterController.skinWidth * _wallExitWidthModifier, unlitLayer))
        {
            return false;
        }
        base.EnterLight();
        return true;
    }
}
