using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineEffectManager : MonoBehaviour
{
    // Singleton
    public static LineEffectManager Instance { get; private set; }

    public float cellDelay = 0.2f;

    private class LineEffectData
    {
        public Cell origin;
        public Vector2Int direction;
        public DamageData damage;
        public CardDamage damageType;
        public GameObject vfx;

        public LineEffectData(
            Cell origin,
            Vector2Int direction,
            DamageData damage,
            CardDamage damageType,
            GameObject vfx)
        {
            this.origin = origin;
            this.direction = direction;
            this.damage = damage;
            this.damageType = damageType;
            this.vfx = vfx;
        }
    }

    private readonly List<LineEffectData> pendingEffects = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddEffect(
        Cell origin,
        Vector2Int direction,
        DamageData damage,
        CardDamage damageType,
        GameObject vfx)
    {
        if (origin == null || vfx == null)
            return;

        pendingEffects.Add(
            new LineEffectData(
                origin,
                direction,
                damage,
                damageType,
                vfx));
    }

    public IEnumerator ProcessEffects()
    {
        foreach (LineEffectData effect in pendingEffects)
        {
            yield return ProcessEffect(effect);
        }

        pendingEffects.Clear();
    }

    private IEnumerator ProcessEffect(LineEffectData effect)
    {
        Cell current = GetStartCell(
            effect.origin.Coordinates,
            effect.direction);

        if (current == null)
            yield break;

        effect.vfx.transform.position =
            current.transform.position;

        ApplyDamage(current, effect);

        while (true)
        {
            Cell next = GridManager.Instance.GetCell(
                current.Coordinates + effect.direction);

            if (next == null)
                break;

            yield return MoveVFX(
                effect.vfx,
                next.transform.position,
                cellDelay);

            current = next;

            ApplyDamage(current, effect);
        }

        Destroy(effect.vfx);
    }

    private Cell GetStartCell(
        Vector2Int origin,
        Vector2Int direction)
    {
        Vector2Int start = origin;

        if (direction == Vector2Int.right)
            start.x = 0;
        else if (direction == Vector2Int.left)
            start.x = GridManager.Instance.width - 1;
        else if (direction == Vector2Int.up)
            start.y = 0;
        else if (direction == Vector2Int.down)
            start.y = GridManager.Instance.height - 1;

        return GridManager.Instance.GetCell(start);
    }

    private void ApplyDamage(
        Cell cell,
        LineEffectData effect)
    {
        if (!cell.HasBuilding)
            return;

        if (effect.damageType == null)
            return;

        effect.damageType.ApplyDamage(
            cell,
            new List<Cell> { cell },
            effect.damage);
    }

    private IEnumerator MoveVFX(
        GameObject vfx,
        Vector3 targetPosition,
        float duration)
    {
        Vector3 startPosition = vfx.transform.position;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);

            vfx.transform.position =
                Vector3.Lerp(
                    startPosition,
                    targetPosition,
                    t);

            yield return null;
        }

        vfx.transform.position = targetPosition;
    }
}
