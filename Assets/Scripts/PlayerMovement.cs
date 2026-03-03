using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameInputs gameInputs;
    [SerializeField] private int moveSpeed = 7;

    private void Update()
    {
        Vector2 inputVector = gameInputs.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, 0);

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }


    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }

    public void SetPlayerPosition(Vector3 position)
    {
        transform.position = position;
    }
}
