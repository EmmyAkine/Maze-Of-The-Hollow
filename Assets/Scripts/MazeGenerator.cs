using UnityEngine;

public class MazeGenerator : MonoBehaviour 
{
    public Grid grid { get; private set; }
    private Transform wallsParent;


    private int rows;
    private int columns;
    private float cellSize = 2f;
    [SerializeField] private GameObject horizontalWallPrefab;  
    [SerializeField] private GameObject verticalWallPrefab;
    [SerializeField] private GameObject endCellDoorPrefab;

    private GameObject endCellDoor;

    private void Awake()
    {
        wallsParent = new GameObject("Walls Parent").transform;
    }
    private void Start()
    {
        //Set Rows and Columns
        rows = GameManager.Instance.rows;
        columns = GameManager.Instance.columns;

        //Create Grid
        grid = new Grid(rows, columns, cellSize);
        CarveOutStartAndEndPoint();
        PopulateTileMap();
    }


    private void Update()
    {
        
    }


    private void PopulateTileMap()
    {
        if (horizontalWallPrefab == null || verticalWallPrefab == null)
        {
            return;
        }


        for (int x = 0; x < grid.Rows; x++)
        {
            for (int y = 0; y < grid.Columns; y++)
            {
                //Get current cell
                Cell currentCell = grid.GetCell(x, y);
                if (currentCell == null) continue;


                Vector3 cellCenter = grid.GetWorldPosition(x, y) + new Vector3(cellSize / 2, cellSize / 2, 0);
                

                // Place wall tiles where walls exist
                if (currentCell.north)
                {
                    float paddingX = 0.11f;
                    GameObject wall = Instantiate(horizontalWallPrefab, wallsParent);
                    wall.transform.SetPositionAndRotation(cellCenter + new Vector3(paddingX, cellSize / 2, 0), Quaternion.Euler(0, 0, 0));
                    wall.GetComponent<Renderer>().sortingOrder = -y - 1;
                    
                }
                if (currentCell.east)
                {
                    int maxY = grid.Columns - 1;

                    float paddingX = 0.1f;

                    GameObject wall = Instantiate(verticalWallPrefab, wallsParent);
                    wall.transform.SetPositionAndRotation(cellCenter + new Vector3((cellSize / 2) + paddingX, 0, 0), Quaternion.Euler(0, 0, 0));
                    wall.GetComponent<Renderer>().sortingOrder = -y;

                }
            }
        }

        // Add outer boundary walls for a closed maze
        for (int x = 0; x < grid.Rows; x++)
        {
            Cell cell = grid.GetCell(x, 0);
            if (cell != null && cell.south)
            {
                float paddingX = 0.11f;
                Vector3 pos = grid.GetWorldPosition(x, 0) + new Vector3((cellSize / 2) + paddingX, 0, 0);  // Bottom edge of row 0
                GameObject wall = Instantiate(horizontalWallPrefab, wallsParent);
                wall.transform.SetPositionAndRotation(pos, Quaternion.Euler(0, 0, 0));
            }
        }

        for (int y = 0; y < grid.Columns; y++)
        {
            float paddingX = -0.1f;

            Vector3 pos = grid.GetWorldPosition(0, y) + new Vector3(paddingX, cellSize / 2, 0);  // Left edge of column 0
            GameObject wall = Instantiate(verticalWallPrefab, wallsParent);
            wall.transform.SetPositionAndRotation(pos, Quaternion.Euler(0, 0, 0));
            wall.GetComponent<Renderer>().sortingOrder = -y;
           


        }
    }

    private void CarveOutStartAndEndPoint()
    {
        //Start Point
        Cell startCell = grid.GetCell(0, 0);
        if (startCell != null)
        {
            startCell.south = false;

        }


        //End Point
        int endX = grid.Rows - 1;
        int endY = grid.Columns - 1;
        Cell endCell = grid.GetCell(endX, endY);
        if (endCell != null)
        {
            float offsetX = 1.1f;
            float offsetY = 2.05f;
            endCell.north = false;
            Vector3 pos = grid.GetWorldPosition(endX, endY) + new Vector3(offsetX, offsetY, 0);
            endCellDoor = Instantiate(endCellDoorPrefab, wallsParent);
            endCellDoor.transform.SetPositionAndRotation(pos, Quaternion.Euler(0, 0, 0));
            endCellDoor.GetComponent<BoxCollider2D>().enabled = true;
        }
    }


    public void ClearMaze()
    {
        Destroy(wallsParent.gameObject);
        grid = null; 
    }

    public Grid GetGrid()
    {
        return grid;
    }

    public void ActivateEndCellDoor()
    {
        endCellDoor.GetComponent<BoxCollider2D>().isTrigger = true;
    }

}
