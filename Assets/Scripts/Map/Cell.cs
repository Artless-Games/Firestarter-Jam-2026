using UnityEngine;

public class Cell : MonoBehaviour
{
    public Transform surface;
    public LineRenderer top;
    public LineRenderer bottom;
    public LineRenderer left;
    public LineRenderer right;

    public Color normalColor = new Color(1f, 1f, 1f, 0f);
    public Color pointedColor = new Color(1f, 1f, 0f, 0.5f);
    public Color selectedColor = new Color(1f, 0f, 0f, 0.6f);
    private Renderer surfaceRenderer;
    private bool isPointed;
    private bool isSelected;

    private float size;
    private Terrain terrain;

    public Vector2Int Coordinates { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetupCell();

        surfaceRenderer = surface.GetComponent<MeshRenderer>();
    }

    public void Initialize(float size, Terrain terrain, Vector2Int coordinates)
    {
        this.size = size;
        this.terrain = terrain;
        Coordinates = coordinates;

        name = "Cell (" + coordinates.x + ", " + coordinates.y + ")"; 
    }

    public void OnPoint(bool active)
    {
        isPointed = active;
        UpdateColor();
    }

    public void OnSelect(bool active)
    {
        isSelected = active;
        UpdateColor();
    }

    private void UpdateColor()
    {
        if (isSelected)
        {
            surfaceRenderer.material.color = selectedColor;
        }
        else if (isPointed)
        {
            surfaceRenderer.material.color = pointedColor;
        }
        else
        {
            surfaceRenderer.material.color = normalColor;
        }
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
        surface.localRotation = Quaternion.Euler(90f, 0f, 0f);
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
