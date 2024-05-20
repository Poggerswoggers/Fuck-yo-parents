using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcBehaviour : MonoBehaviour
{
    public enum States { Roaming, Talk }
    public States currentState = States.Roaming;

    public bool isWalking;
    public float speed;
    public Vector2 dir;

    Vector3 refVel = Vector3.zero;

    Rigidbody2D rb;

    public SnapCamera sc;

    public bool crossBoundX;
    public bool crossBoundY;

    public float talkRad;

    [SerializeField] LayerMask lm;
    private void Start()
    {
     

        startWalking();
        rb = GetComponent<Rigidbody2D>();
        sc.GetComponent<SnapCamera>();
    }

    void startWalking()
    {

        isWalking = true;
        speed = Random.Range(2, 5);

        changeDir();
    }

    private void Update()
    {
        switch (currentState)
        {
            case States.Roaming:
                Roaming();
                break;
            case States.Talk:
                Talk();
                break;

            default:
                Debug.LogWarning("enu defaulted");
                break;
        }

    }

    void Roaming()
    {
        if (!isWalking) return;
        Vector2 targetVel = dir.normalized * speed;
        rb.velocity = targetVel;

        if (Mathf.Abs(transform.position.x) > sc.CalculateBounds().x && !crossBoundX)
        {
            crossBoundX = true;
            //dir = new Vector2(-Mathf.Sign(transform.position.x) * Mathf.Sin(Random.Range(0.1f,1)), Random.insideUnitCircle.y);
            dir = new Vector2(-Mathf.Sign(transform.position.x) * Mathf.Sin(Random.Range(0, Mathf.PI / 2)), Random.insideUnitCircle.y);

        }
        else if (Mathf.Abs(transform.position.x) < sc.CalculateBounds().x)
        {
            crossBoundX = false;
        }

        if (Mathf.Abs(transform.position.y) > sc.CalculateBounds().y && !crossBoundY)
        {
            crossBoundY = true;
            dir = new Vector2(Random.insideUnitCircle.x, -Mathf.Sign(transform.position.y) * Mathf.Cos(Random.Range(0, Mathf.PI / 2)));

        }
        else if (Mathf.Abs(transform.position.y) < sc.CalculateBounds().y)
        {
            crossBoundY = false;
        }
    }

    void Talk()
    {
        rb.velocity = Vector2.zero;


        RaycastHit2D hit = Physics2D.CircleCast(transform.position, talkRad, Vector2.zero);
        //transform.position = LeanMove(hit.collider.gameObject,2);

    }

    void changeDir()
    {
        float angle = Random.value * Mathf.PI * 2;
        // Calculate walking direction
        dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
    }
}
