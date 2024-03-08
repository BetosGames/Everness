using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float speed;

    private void Start()
    {
        StartCoroutine(DeleteCloud());
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private IEnumerator DeleteCloud()
    {
        yield return new WaitUntil(() => transform.localPosition.x >= 0.16f);
        Destroy(gameObject);
    }
}
