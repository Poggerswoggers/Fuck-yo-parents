using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAnimation : MonoBehaviour
{
    [Header("Bounce")]
    [SerializeField] float bounce;
    [Range(1,2)] [SerializeField] float maxBounce;
    float bounceVel;
    public float bounceAccel;
    public float bounceDamp;

    [Header("Debug")]
    [SerializeField] float hop;
    [SerializeField] float lastHop;


    float flip;

    //Reference
    Transform spriteTransform;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        flip = transform.localScale.x;
        
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

        if (lastHop < 0.5 && hop >= 0.5f) bounce = maxBounce;
        if (lastHop > 0.9 && hop <= 0.1f) bounce = maxBounce;
    }

    public void BounceAnim(float multiplier, float _bounce)
    {
        hop += Time.deltaTime * multiplier;
        if (hop > 1) hop--;

        flip = (rb.velocity.x < 0) ? -0.05f : 0.05f;

        //Sway back and forth
        float t = hop * Mathf.PI * 2;
        spriteTransform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(t) * 0.1f);
        spriteTransform.localPosition = new Vector3(0, Mathf.Abs(Mathf.Sin(t)) * 0.5f, 0);

        if (lastHop < 0.5 && hop >= 0.5f) bounce = _bounce;
        if (lastHop > 0.9 && hop <= 0.1f) bounce = _bounce;
    }

    public void StopWalkAnim()
    {
        spriteTransform.rotation = Quaternion.identity;
        spriteTransform.localPosition = Vector3.zero;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position - Vector3.left * (Mathf.Sign(spriteTransform.localScale.x) * 4), 3);
    }
}
