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
        yield return new WaitForSeconds(0.1f); // �ȴ� 0.5 ��
        Destroy(gameObject); // ���ٸö���
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "weakywall")
        {
            // �� weakywall ���в���������������
            Destroy(other.gameObject);
        }
    }
}
