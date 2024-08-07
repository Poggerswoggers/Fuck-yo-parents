using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NpcRoamState : NpcBaseState
{

    
    public float speed { get; set;}
    [SerializeField] float minSpeed, maxSpeed;

    //Npc walk dir
    Vector2 dir;

    //Walk duration
    [SerializeField] float walkDuration;
    float _walkDur;

    [SerializeField] float borderMargin = 0.5f;

    //Roam cross border bound check
    bool crossBoundX;
    bool crossBoundY;

    [Header("Bounce Modifiers")]
    [SerializeField] float bounce;
    [SerializeField] float multiplier;

    public override void EnterState(NpcStateManager npcSm)
    {
        _walkDur = walkDuration;    

       nSm = npcSm;
       npcThis = npcSm.transform;

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
            nSm.isWalking = false;
        }
        npcSm.npcAnim.npcAnimation();
        Roaming();
    }

    public override void ExitState(NpcStateManager npcSm)
    {
        rb.velocity = Vector2.zero;
        nSm.npcAnim.StopWalkAnim();
    }

    void startWalking()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        nSm.isWalking = true;
        float angle = Random.value * Mathf.PI * 2;
        // Calculate walking direction
        dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
            
        rb = npcThis.GetComponent<Rigidbody2D>();
    }

    void Roaming()
    {
        if(nSm.isWalking)
        {
            Vector2 targetVel = dir.normalized * speed * Time.deltaTime;
            //npcThis.Translate(targetVel);
            rb.velocity = targetVel;

            nSm.npcAnim.WalkAnim(multiplier, bounce, nSm.hasBounce);

        }
        else
        {
            int odds = Random.Range(0, 2);
            if(odds == 0)
            {
                nSm.SwitchState(nSm.interactState);
            }
            else
            {
                _walkDur = walkDuration;
                startWalking();
            }
        }
          
       
        if (Mathf.Abs(npcThis.position.x) > MapBound.Bound.x-borderMargin && !crossBoundX)
        {
            crossBoundX = true;
            //dir = new Vector2(-Mathf.Sign(transform.position.x) * Mathf.Sin(Random.Range(0.1f,1)), Random.insideUnitCircle.y);
            dir = new Vector2(-Mathf.Sign(npcThis.position.x) * Mathf.Sin(Random.Range(0, Mathf.PI / 2)), Random.insideUnitCircle.y);

            _walkDur += 1;
        }
        else if (Mathf.Abs(npcThis.position.x) < MapBound.Bound.x)
        {
            crossBoundX = false;
        }

        if (Mathf.Abs(npcThis.position.y) > MapBound.Bound.y-borderMargin && !crossBoundY)
        {
            crossBoundY = true;
            dir = new Vector2(Random.insideUnitCircle.x, -Mathf.Sign(npcThis.position.y) * Mathf.Cos(Random.Range(0, Mathf.PI / 2)));

            _walkDur += 1;
        }
        else if (Mathf.Abs(npcThis.position.y) < MapBound.Bound.y)
        {
            crossBoundY = false;
        }
    }

    public override void OnCollisionEnter2D(NpcStateManager npcSm, Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if(Mathf.Abs(dir.x) > 0.1){
                dir = new Vector2(-Mathf.Sign(dir.x) * Mathf.Sin(Random.Range(0, Mathf.PI / 2)), Random.insideUnitCircle.y);
            }
            if (Mathf.Abs(dir.y) > 0.1){
                dir = new Vector2(Random.insideUnitCircle.x, -Mathf.Sign(dir.y) * Mathf.Cos(Random.Range(0, Mathf.PI / 2)));
            }
        }
    }
}

