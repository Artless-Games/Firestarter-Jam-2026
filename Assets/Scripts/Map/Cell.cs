using UnityEngine;

public class Cell : MonoBehaviour
{
    public Transform surface;
    public LineRenderer top;
    public LineRenderer bottom;
    public LineRenderer left;
    public LineRenderer right;

    private float size;
    private Terrain terrain;

    public Vector2Int Coordinates { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetupCell();
    }

    public void Initialize(float size, Terrain terrain, Vector2Int coordinates)
    {
        this.size = size;
        this.terrain = terrain;
        Coordinates = coordinates;

        name = "Cell (" + coordinates.x + ", " + coordinates.y + ")"; 
    }

    private void SetupCell()
    {
        SetupHeight();
        SetupSurface();
        SetupBorder();
    }

    private void SetupHeight()
    {
        float height = terrain.SampleHeight(transform.position)
                       + terrain.transform.position.y
                       + 0.1f;

        transform.position = new Vector3(
            transform.position.x,
            height,
            transform.position.z
        );
    }

    private void SetupSurface()
    {
        surface.SetLocalPositionAndRotation(
            Vector3.up * 0.01f,
            Quaternion.Euler(90f, 0f, 0f));
        surface.localScale = new Vector3(size, size, 1f);
    }

    private void SetupBorder()
    {
        float half = size / 2f;

        SetLine(top,
            new Vector3(-half, 0f, half),
            new Vector3(half, 0f, half));

        SetLine(bottom,
            new Vector3(-half, 0f, -half),
            new Vector3(half, 0f, -half));

        SetLine(left,
            new Vector3(-half, 0f, -half),
            new Vector3(-half, 0f, half));

        SetLine(right,
            new Vector3(half, 0f, -half),
            new Vector3(half, 0f, half));
    }

    private void SetLine(LineRenderer line, Vector3 start, Vector3 end)
    {
        line.positionCount = 2;
        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }
}
