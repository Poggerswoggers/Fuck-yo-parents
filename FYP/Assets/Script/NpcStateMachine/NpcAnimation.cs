using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAnimation : MonoBehaviour
{

    public float bounce;
    public float bounceVel;
    public float bounceAccel;
    public float bounceDamp;
    public float hop;
    public float lastHop;
    public float flip;

    Transform spriteTransform;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        spriteTransform = transform.GetChild(0);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void npcAnimation()
    {
        bounceVel += (1 - bounce) * bounceAccel;
        bounce += bounceVel;
        bounceVel *= bounceDamp;
        spriteTransform.localScale = new Vector3(bounce * flip, 0.05f / bounce, 0.05f);

        lastHop = hop;
    }

    public void WalkAnim()
    {
        hop += Time.deltaTime;
        if (hop > 1) hop--;

        flip = (rb.velocity.x < 0) ? -0.05f : 0.05f;

        //Sway back and forth
        float t = hop * Mathf.PI * 2;
        spriteTransform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(t) * 0.1f);
        spriteTransform.localPosition = new Vector3(0, Mathf.Abs(Mathf.Sin(t)) * 0.5f, 0);

        if (lastHop < 0.5 && hop >= 0.5f) bounce = 1.2f;
        if (lastHop > 0.9 && hop <= 0.1f) bounce = 1.2f;
    }
}
