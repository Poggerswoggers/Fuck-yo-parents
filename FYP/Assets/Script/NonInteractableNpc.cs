using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonInteractableNpc:MonoBehaviour
{
    public enum states
    {
        roam,
        Queue,
        Leave,
        BoardTrain,
    }
    public states currentState;

    [SerializeField] NoninteractableScriptable Noninteractablefields;

    bool crossBoundX;
    bool crossBoundY;

    //Npc walk dir
    Vector2 dir;

    //reference
    Rigidbody2D rb;
    NpcAnimation npcAnim;
    private void Start()
    {
        npcAnim = gameObject.GetComponent<NpcAnimation>();
        rb = GetComponent<Rigidbody2D>();
        float angle = Random.value * Mathf.PI * 2;
        dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
    }

    private void FixedUpdate()
    {
        Roaming();
        npcAnim.npcAnimation();
        npcAnim.WalkAnim(1.1f, 1.1f, true);
    }

    void Roaming()
    {
        Vector2 targetVel = dir.normalized * Noninteractablefields.speed * Time.deltaTime;
        rb.velocity = targetVel;


        if (Mathf.Abs(transform.position.x) > MapBound.UpperBound.x - Noninteractablefields.borderMargin && !crossBoundX)
        {
            crossBoundX = true;
            //dir = new Vector2(-Mathf.Sign(transform.position.x) * Mathf.Sin(Random.Range(0.1f,1)), Random.insideUnitCircle.y);
            dir = new Vector2(-Mathf.Sign(transform.position.x) * Mathf.Sin(Random.Range(0, Mathf.PI / 2)), Random.insideUnitCircle.y);
        }
        else if (Mathf.Abs(transform.position.x) < MapBound.UpperBound.x)
        {
            crossBoundX = false;
        }

        if (Mathf.Abs(transform.position.y) > MapBound.UpperBound.y - Noninteractablefields.borderMargin && !crossBoundY)
        {
            crossBoundY = true;
            dir = new Vector2(Random.insideUnitCircle.x, -Mathf.Sign(transform.position.y) * Mathf.Cos(Random.Range(0, Mathf.PI / 2)));
        }
        else if (Mathf.Abs(transform.position.y) < MapBound.UpperBound.y)
        {
            crossBoundY = false;
        }
    }
}
