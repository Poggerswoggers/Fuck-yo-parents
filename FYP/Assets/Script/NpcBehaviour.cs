using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcBehaviour : MonoBehaviour
{
    public bool isWalking;
    public float speed;
    public Vector2 dir;

    Vector3 refVel = Vector3.zero;

    Rigidbody2D rb;
    private void Start()
    {
        startWalking();
        rb = GetComponent<Rigidbody2D>();
    }

    void startWalking()
    {
        isWalking = true;
        speed = Random.Range(2, 3);

        dir = Random.insideUnitCircle.normalized;
    }

    private void Update()
    {
        Vector2 targetVel = new Vector2(dir.x * speed, dir.y);

        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVel, ref refVel, 0.5f);
    }
}
