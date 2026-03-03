using UnityEngine;
using static Grid;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Roaming,
        ChaseTarget,
        GoingBackToStart


    }

    [SerializeField] private State state;

    private EnemyPathFindingMovement pathFindingMovement;
    private Vector3 startingPosition;
    private Vector3 roamPosition;

    private PlayerMovement playerMovement;

    public Grid.Quadrant assignedQuadrant;

    private MazeGenerator mazeGenerator;

    private Grid grid;


    private void Awake()
    {
        state = State.Roaming;
    }
    private void Start()
    {
        pathFindingMovement = GetComponent<EnemyPathFindingMovement>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        mazeGenerator = FindFirstObjectByType<MazeGenerator>();
        if (mazeGenerator != null)
        {
            grid = mazeGenerator.GetGrid();
        }

        startingPosition = transform.position;
        roamPosition = GetRoamingPosition();
    }

    // Update is called once per frame
    private void Update()
    {
        switch(state)
        {
            default:
            case State.Roaming:
                pathFindingMovement.MoveTo(roamPosition);
                float reachedPositionDistance = 1f;
                if (Vector3.Distance(transform.position, roamPosition) < reachedPositionDistance)
                {
                    // Reached Roam Position
                    roamPosition = GetRoamingPosition();
                }

                FindTarget();

                break;

            case State.ChaseTarget:
                pathFindingMovement.MoveTo(playerMovement.GetPlayerPosition());

                float stopChaseDistance = 5f;
                if (Vector3.Distance(transform.position, playerMovement.GetPlayerPosition()) > stopChaseDistance)
                {
                    state = State.GoingBackToStart;
                }
                break;

            case State.GoingBackToStart:
                pathFindingMovement.MoveTo(startingPosition);
                reachedPositionDistance = 1f;
                if (Vector3.Distance(transform.position, startingPosition) < reachedPositionDistance)
                {
                    state = State.Roaming;
                }
                break;
        }
        
    }

    private void FindTarget()
    {
        float targetRange = 4f;

        if (Vector3.Distance(transform.position, playerMovement.GetPlayerPosition()) < targetRange)
        {
            state = State.ChaseTarget;
        }
    }

    private void CatchTarget()
    {
            GameManager.Instance.PlayerDied();
    }

    private Vector3 GetRoamingPosition()
    {
        Grid grid = mazeGenerator.GetGrid();  // Cache or reference
        QuadrantBounds bounds = grid.GetQuadrantBounds(assignedQuadrant);

        // Random cell in bounds
        int randX = Random.Range(bounds.minX, bounds.maxX + 1);
        int randY = Random.Range(bounds.minY, bounds.maxY + 1);
        //Debug.Log($"int randX is {randX} created from random.range of {bounds.minX} and {bounds.maxX} + 1; Also, int randY is {randY} created from random.range of {bounds.minY} and {bounds.maxY} for {assignedQuadrant}");

        return grid.GetWorldPosition(randX, randY) + new Vector3(grid.CellSize / 2f, grid.CellSize / 2f, 0);
    }

    private void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        if (collisionInfo.CompareTag("PLAYER"))
        {
            CatchTarget();
        }
    }
}
