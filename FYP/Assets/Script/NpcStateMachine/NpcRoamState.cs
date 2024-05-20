using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcRoamState : NpcBaseState
{
    public bool isWalking;
    public float speed;
    public float minSpeed, maxSpeed;
    public Vector2 dir;


    Rigidbody2D rb;
    Transform spriteTransform;
    public SnapCamera sc;

    public bool crossBoundX;
    public bool crossBoundY;

    public float talkRad;

    public float bounce;
    public float bounceVel;
    public float bounceAccel;
    public float bounceDamp;
    public float hop;
    public float lastHop;
    public float flip;

    Transform npcThis;

    public override void EnterState(NpcStateManager npcSm)
    {
        npcThis = npcSm.transform;
        InitialiseVariable();
        startWalking();
    }

    public override void UpdateState(NpcStateManager npcSm)
    {
        Roaming();


        if(isWalking)
        {
            WalkAnim();
        }
        else
        {

        }
        npcAnimation();

    }



    void InitialiseVariable()
    {
        spriteTransform = npcThis.GetChild(0);
        speed = Random.Range(minSpeed, maxSpeed);
    }

    void startWalking()
    {
        hop = Random.value;

        isWalking = true;
        float angle = Random.value * Mathf.PI * 2;
        // Calculate walking direction
        dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));

        rb = npcThis.GetComponent<Rigidbody2D>();
    }

    void Roaming()
    {
        if (isWalking)
        {
            Vector2 targetVel = dir.normalized * speed * Time.deltaTime;
            rb.velocity = targetVel;
        }

        if (Mathf.Abs(npcThis.position.x) > sc.CalculateBounds().x && !crossBoundX)
        {
            crossBoundX = true;
            //dir = new Vector2(-Mathf.Sign(transform.position.x) * Mathf.Sin(Random.Range(0.1f,1)), Random.insideUnitCircle.y);
            dir = new Vector2(-Mathf.Sign(npcThis.position.x) * Mathf.Sin(Random.Range(0, Mathf.PI / 2)), Random.insideUnitCircle.y);

        }
        else if (Mathf.Abs(npcThis.position.x) < sc.CalculateBounds().x)
        {
            crossBoundX = false;
        }

        if (Mathf.Abs(npcThis.position.y) > sc.CalculateBounds().y && !crossBoundY)
        {
            crossBoundY = true;
            dir = new Vector2(Random.insideUnitCircle.x, -Mathf.Sign(npcThis.position.y) * Mathf.Cos(Random.Range(0, Mathf.PI / 2)));

        }
        else if (Mathf.Abs(npcThis.position.y) < sc.CalculateBounds().y)
        {
            crossBoundY = false;
        }
    }


    void npcAnimation()
    {
        bounceVel += (1 - bounce) * bounceAccel;
        bounce += bounceVel;
        bounceVel *= bounceDamp;
        spriteTransform.localScale = new Vector3(bounce * flip, 0.05f / bounce, 0.05f);

        lastHop = hop;
    }

    void WalkAnim()
    {
        hop += 3 / (40 / 0.05f);
        if (hop > 1) hop--;

        flip = (rb.velocity.x < 0) ? -0.05f : 0.05f;

        //Sway back and forth
        float t = hop * Mathf.PI * 2;
        spriteTransform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(t) * 0.1f);
        spriteTransform.localPosition = new Vector3(0, Mathf.Abs(Mathf.Sin(t)) * 0.2f, 0);

        if (lastHop < 0.5 && hop >= 0.5f) bounce = 1.2f;
        if (lastHop > 0.9 && hop <= 0.1f) bounce = 1.2f;
    }
}

