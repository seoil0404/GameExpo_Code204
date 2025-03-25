using UnityEngine;
using System.Collections;

public class MinoEffect : MonoBehaviour
{
    private SpriteRenderer sr;
    private float fadeStartTime = 0.2f;
    private float fadeDuration = 0.2f;
    private float rotationSpeed;
    private Transform myTransform;
    private MinoEffectPool pool;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        myTransform = transform;
    }

    public void Init(MinoEffectPool pool)
    {
        this.pool = pool;

        Color c = sr.color;
        sr.color = new Color(c.r, c.g, c.b, 1f);

        float dir = Random.value > 0.5f ? 1f : -1f;
        rotationSpeed = Random.Range(90f, 270f) * dir;

        StartCoroutine(FadeAndRecycle());
    }

    public void Launch(Vector2 direction, float power)
    {
        if (TryGetComponent(out Rigidbody2D rb))
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.AddForce(direction * power, ForceMode2D.Impulse);
        }
    }

    void Update()
    {
        myTransform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    IEnumerator FadeAndRecycle()
    {
        yield return new WaitForSeconds(fadeStartTime);

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            Color c = sr.color;
            sr.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        gameObject.SetActive(false);
        pool.ReturnToPool(this);
    }
}