using System.Collections;
using UnityEngine;

public class camerashake : MonoBehaviour
{
    public GameObject pos;

    public IEnumerator shake(float duration, float magnitude)
    {
        var orginalpos = transform.position;
        float elapsed = 0;
        while (elapsed < duration)
        {
            var x = Random.Range(-1f, 1f) * magnitude;
            var y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(x, y, orginalpos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = orginalpos;
    }
}