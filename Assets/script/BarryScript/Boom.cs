using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DestroyAfterHalfSecond());
    }

    private IEnumerator DestroyAfterHalfSecond()
    {
        yield return new WaitForSeconds(0.1f); // 等待 0.5 秒
        Destroy(gameObject); // 销毁该对象
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "weakywall")
        {
            // 对 weakywall 进行操作，例如销毁它
            Destroy(other.gameObject);
        }
    }
}
