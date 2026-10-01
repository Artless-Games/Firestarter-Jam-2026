using UnityEngine;

public class Card : MonoBehaviour
{
    public CardData data;
    public float pointedHeight = 50f;
    public float pointedScale = 1.5f;
    public float dragScale = 0.5f;
    public float animationSpeed = 10f;

    private RectTransform rectTransform;

    private Vector2 initialPosition;
    private float initialRotation;
    private int originalSiblingIndex;

    private bool isPointed;
    private bool isDragged;

    private Vector2 targetPosition;
    private float targetRotation;
    private float targetScale = 1f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDragged)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(
            rectTransform.anchoredPosition,
            targetPosition,
            Time.deltaTime * animationSpeed
        );

            rectTransform.localRotation = Quaternion.Lerp(
                rectTransform.localRotation,
                Quaternion.Euler(0f, 0f, targetRotation),
                Time.deltaTime * animationSpeed
            );
        }

        rectTransform.localScale = Vector3.Lerp(
            rectTransform.localScale,
            Vector3.one * targetScale,
            Time.deltaTime * animationSpeed
        );
    }

    public void SetHandPosition(Vector2 position, float rotation)
    {
        initialPosition = position;
        initialRotation = rotation;

        UpdateTarget();
    }

    public void SetDragPosition(Vector2 screenPosition)
    {
        RectTransform parentRect =
            rectTransform.parent as RectTransform;

        Canvas canvas =
            GetComponentInParent<Canvas>();

        Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay
            ? null
            : canvas.worldCamera;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            screenPosition,
            cam,
            out Vector2 localPosition))
        {
            rectTransform.anchoredPosition = localPosition;
        }
    }

    public void OnPoint(bool active)
    {
        isPointed = active;
        UpdateTarget();
    }

    public void OnDragging(bool active)
    {
        isDragged = active;

        if (active)
        {
            targetScale = dragScale;
        }
        else
        {
            UpdateTarget();
        }
    }

    public void BeginDragVisual()
    {
        originalSiblingIndex = transform.GetSiblingIndex();
        transform.SetAsLastSibling();
    }

    public void EndDragVisual()
    {
        transform.SetSiblingIndex(originalSiblingIndex);
    }

    private void UpdateTarget()
    {
        targetPosition = initialPosition;
        targetRotation = initialRotation;

        if (isDragged)
        {
            targetScale = dragScale;
            return;
        }

        if (isPointed)
        {
            targetPosition.y += pointedHeight;
            targetScale = pointedScale;
        }
        else
        {
            targetScale = 1f;
        }
    }
}
