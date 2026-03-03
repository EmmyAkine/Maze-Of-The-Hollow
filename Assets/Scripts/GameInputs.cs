using System.Security.Cryptography;
using UnityEngine;

public class GameInputs : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }
    private void OnEnable()
    {
        playerInputActions.GamePlay.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.GamePlay.Disable();
    }
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.GamePlay.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }
}
