using UnityEngine;

public class Keys : MonoBehaviour
{
    public Grid.Quadrant assignedQuadrant;

    private void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        if (collisionInfo.CompareTag("PLAYER"))
        {
            //Checkpoint cleared!
            //Update as Last Saved checkpoint

            GameManager.Instance.KeyCollected();
            gameObject.SetActive(false);

        }
    }

}
