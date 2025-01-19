using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBubble : Bubble
{
    public GameObject BoomPrefabs;
    public float boomTime = 1.0f;
    public float baseboomRadius = 2.0f;
    public bool isInsideSphere = false;

    public override void Start()
    {
        destroyTime = 1.0f;
        StartCoroutine(ExplodeAfterOneSecond());
    }

    private IEnumerator ExplodeAfterOneSecond()
    {
        yield return new WaitForSeconds(destroyTime); 
        Explode(); // 调用爆炸方法
    }

    private void Explode()
    {
        if (BoomPrefabs != null)
        {
            Instantiate(BoomPrefabs, transform.position, Quaternion.identity);
            SphereCollider sphereCollider = BoomPrefabs.GetComponent<SphereCollider>();
            if (sphereCollider != null)
            {
                sphereCollider.radius = baseboomRadius * size;
            }

        }
    }
}
