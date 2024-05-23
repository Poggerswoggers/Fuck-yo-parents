using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcRoamState : NpcBaseState
{
    public bool isWalking;
    public float speed;
    public float minSpeed, maxSpeed;

    public Vector2 dir;
    [SerializeField] float walkDuration;
    float _walkDur;

    public float borderMargin = 0.5f;

    //reference
    Rigidbody2D rb;
    NpcStateManager nSm;
    [SerializeField] SnapCamera sc;

    public bool crossBoundX;
    public bool crossBoundY;

    Transform npcThis;

    [Header("Bounce Modifiers")]
    [SerializeField] float bounce;
    [SerializeField] float multiplier;

    public override void EnterState(NpcStateManager npcSm)
    {
        _walkDur = walkDuration;    

        nSm = npcSm;
        npcThis = npcSm.transform;
        InitialiseVariable();
        startWalking();
    }

    public override void UpdateState(NpcStateManager npcSm)
    {

        if (_walkDur > 0)
        {
            _walkDur -= Time.deltaTime;
        }
        else
        {
            isWalking = false;
        }
        npcSm.npcAnim.npcAnimation();
        Roaming();
    }



    void InitialiseVariable()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    void startWalking()
    {

        isWalking = true;
        float angle = Random.value * Mathf.PI * 2;
        // Calculate walking direction
        dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
            
        rb = npcThis.GetComponent<Rigidbody2D>();
    }

    void Roaming()
    {
        if(isWalking)
        {
            Vector2 targetVel = dir.normalized * speed * Time.deltaTime;
            //npcThis.Translate(targetVel);
            rb.velocity = targetVel;

            nSm.npcAnim.WalkAnim(multiplier, bounce, nSm.hasBounce);

        }
        else
        {
            nSm.npcAnim.StopWalkAnim();
            nSm.SwitchState(nSm.interactState);
        }

        if (Mathf.Abs(npcThis.position.x) > sc.CalculateBounds().x-borderMargin && !crossBoundX)
        {
            crossBoundX = true;
            //dir = new Vector2(-Mathf.Sign(transform.position.x) * Mathf.Sin(Random.Range(0.1f,1)), Random.insideUnitCircle.y);
            dir = new Vector2(-Mathf.Sign(npcThis.position.x) * Mathf.Sin(Random.Range(0, Mathf.PI / 2)), Random.insideUnitCircle.y);

            walkDuration += 1;
        }
        else if (Mathf.Abs(npcThis.position.x) < sc.CalculateBounds().x)
        {
            crossBoundX = false;
        }

        if (Mathf.Abs(npcThis.position.y) > sc.CalculateBounds().y-borderMargin && !crossBoundY)
        {
            crossBoundY = true;
            dir = new Vector2(Random.insideUnitCircle.x, -Mathf.Sign(npcThis.position.y) * Mathf.Cos(Random.Range(0, Mathf.PI / 2)));

            walkDuration += 1;
        }
        else if (Mathf.Abs(npcThis.position.y) < sc.CalculateBounds().y)
        {
            crossBoundY = false;
        }
    }
}

