using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBubble : Bubble
{
    public bool isInsideSphere = false;
    public float buoyancyForce = 20f;
    public float buoyancySpeed = 200f;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && isGrowing == false)
        {
            isInsideSphere = true;
        }
    }
    public override void Start()
    {
        destroyTime = 1f;
    }

    public override void Update()
    {
        if (isInsideSphere)
        {
            if (player != null && gameObject.GetComponent<Rigidbody2D>() != null)
            {
                transform.position = player.transform.position;
                if (playerRigidbody.velocity.y >= -buoyancySpeed)
                {
                    Vector2 buoyancy = - Vector2.up * buoyancyForce * size;
                    playerRigidbody.AddForce(buoyancy, ForceMode2D.Force);
                }
                else
                {
                    playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x,-buoyancySpeed);
                }
            }
        }
    }

}
