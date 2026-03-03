using UnityEngine;

public class Lives : MonoBehaviour
{
    public Grid.Quadrant assignedQuadrant;
    private void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        if (collisionInfo.CompareTag("PLAYER"))
        {
            GameManager.Instance.AddLive();
            gameObject.SetActive(false);
        }
    }

}
