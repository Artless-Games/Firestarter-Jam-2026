using System.Collections;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class Card : MonoBehaviour
{
    public CardData data;
    public float pointedHeight = 50f;
    public float pointedScale = 1.5f;
    public float dragScale = 0.5f;
    public float animationSpeed = 10f;
    public float fullscreenScaleMultiplier = 2.5f;
    public float fullscreenAnimationSpeed = 8f;
    [SerializeField] private Transform visualTransform;

    private RectTransform rectTransform;

    private Vector2 initialPosition;
    private float initialRotation;
    private int originalSiblingIndex;

    private bool isPointed;
    private bool isDragged;
    private bool isFullscreen;

    private Vector2 targetPosition;
    private float targetRotation;
    private float targetScale = 1f;
    private Vector2 fullscreenPosition;
    private Vector3 fullscreenScale;
    private float fullscreenRotation;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFullscreen)
            return;

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

    public void UpdateCard()
    {
        if (data == null || data.image == null)
            return;

        Instantiate(
            data.image,
            visualTransform
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

        if (active)
        {
            originalSiblingIndex = transform.GetSiblingIndex();
            transform.SetAsLastSibling();
        }
        else
        {
            transform.SetSiblingIndex(originalSiblingIndex);
        }

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

    public IEnumerator ShowFullscreen(float duration)
    {
        originalSiblingIndex = transform.GetSiblingIndex();
        transform.SetAsLastSibling();

        Canvas canvas = GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        Vector3 canvasCenterWorld =
            canvasRect.TransformPoint(canvasRect.rect.center);

        Vector2 targetPosition =
            rectTransform.parent.InverseTransformPoint(canvasCenterWorld);

        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector3 startScale = rectTransform.localScale;
        Quaternion startRotation = rectTransform.localRotation;

        Vector3 targetScale =
            Vector3.one * fullscreenScaleMultiplier;

        isFullscreen = true;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);
            t = Mathf.SmoothStep(0f, 1f, t);

            rectTransform.anchoredPosition =
                Vector2.Lerp(startPosition, targetPosition, t);

            rectTransform.localScale =
                Vector3.Lerp(startScale, targetScale, t);

            rectTransform.localRotation =
                Quaternion.Lerp(
                    startRotation,
                    Quaternion.identity,
                    t
                );

            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
        rectTransform.localScale = targetScale;
        rectTransform.localRotation = Quaternion.identity;
    }

    public IEnumerator HideFullscreen(float duration)
    {
        if (!TryGetComponent<CanvasGroup>(out var canvasGroup))
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            canvasGroup.alpha = 1f - t;

            yield return null;
        }

        canvasGroup.alpha = 0f;
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
            targetRotation = 0f;
            targetScale = pointedScale;
        }
        else
        {
            targetScale = 1f;
        }
    }
}
