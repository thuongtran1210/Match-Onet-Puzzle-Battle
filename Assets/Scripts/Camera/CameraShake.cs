using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    private Vector3 _originalLocalPos;

    private void Awake()
    {
        _originalLocalPos = transform.localPosition;
    }

    public void Shake(float duration, float magnitude)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, _originalLocalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = _originalLocalPos;
    }
}