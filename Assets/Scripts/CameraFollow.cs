using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float cameraBoundaryX = 4f;
    private float cameraBoundaryY = 1.5f;
    private float smoothness = 2f;
    private float getCameraZPosition;

    [SerializeField] private Transform target;


    private void Awake()
    {
        getCameraZPosition = transform.position.z;
    }
    // Update is called once per frame
    private void LateUpdate()
    {
        if (target != null)
        {
            FollowPlayer();
        }
        
    }
    private void FollowPlayer()
    {
        Vector3 nextposition = target.position;
        if (nextposition.x < cameraBoundaryX)
        {
            nextposition.x = cameraBoundaryX;
        }
        if (nextposition.y < cameraBoundaryY)
        {
            nextposition.y = cameraBoundaryY;
        }

        nextposition.z = getCameraZPosition;
        transform.position = Vector3.Lerp(transform.position, nextposition, smoothness * Time.deltaTime);
    }
}
