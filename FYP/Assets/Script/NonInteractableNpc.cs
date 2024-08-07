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

    [SerializeField] float speed;
    [SerializeField] SnapCamera sc;
    [SerializeField] float borderMargin = 0.5f;
    bool crossBoundX;
    bool crossBoundY;

    //Npc walk dir
    Vector2 dir;
    float _walkDur;

    //reference
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        float angle = Random.value * Mathf.PI * 2;
        dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
    }

    private void FixedUpdate()
    {
        Roaming();
    }

    void Roaming()
    {
        Vector2 targetVel = dir.normalized * speed * Time.deltaTime;
        rb.velocity = targetVel;


        if (Mathf.Abs(transform.position.x) > MapBound.Bound.x - borderMargin && !crossBoundX)
        {
            crossBoundX = true;
            //dir = new Vector2(-Mathf.Sign(transform.position.x) * Mathf.Sin(Random.Range(0.1f,1)), Random.insideUnitCircle.y);
            dir = new Vector2(-Mathf.Sign(transform.position.x) * Mathf.Sin(Random.Range(0, Mathf.PI / 2)), Random.insideUnitCircle.y);

            _walkDur += 1;
        }
        else if (Mathf.Abs(transform.position.x) < MapBound.Bound.x)
        {
            crossBoundX = false;
        }

        if (Mathf.Abs(transform.position.y) > MapBound.Bound.y - borderMargin && !crossBoundY)
        {
            crossBoundY = true;
            dir = new Vector2(Random.insideUnitCircle.x, -Mathf.Sign(transform.position.y) * Mathf.Cos(Random.Range(0, Mathf.PI / 2)));

            _walkDur += 1;
        }
        else if (Mathf.Abs(transform.position.y) < MapBound.Bound.y)
        {
            crossBoundY = false;
        }
    }
}
