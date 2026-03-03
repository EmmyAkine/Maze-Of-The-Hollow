using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public Grid.Quadrant assignedQuadrant;

    private void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        if (collisionInfo.CompareTag("PLAYER"))
        {
            //Respawn saved!
            //Update as Last Saved respawn

            GameManager.Instance.UpdateRespawnPoint(collisionInfo.transform.position);
            gameObject.SetActive(false);
            
        }
    }


}
