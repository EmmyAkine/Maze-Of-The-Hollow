using UnityEngine;

public class EndDoorScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        if (collisionInfo.CompareTag("PLAYER"))
        {
            GameManager.Instance.WinLevel();
        }
    }
}
