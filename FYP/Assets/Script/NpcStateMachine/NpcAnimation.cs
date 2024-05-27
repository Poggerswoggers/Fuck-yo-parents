using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAnimation : MonoBehaviour
{
    [Header("Bounce")]
    [SerializeField] float bounce;
    float bounceVel;
    public float bounceAccel;
    public float bounceDamp;

    [Header("Wander")]
    public float wanderTime;
    
    //Hops
    float hop;
    float lastHop;


    [SerializeField] float flip;

    //Reference
    Transform trans;
    Transform spriteTransform;
    Rigidbody2D rb;
    SpriteRenderer sR;
    Animator anim;
    NpcStateManager nsm;

    // Start is called before the first frame update
    void Start()
    {
        trans = transform;
        flip = transform.localScale.x;
        spriteTransform = transform.GetChild(0);
        rb = GetComponent<Rigidbody2D>();
        sR = spriteTransform.GetComponent<SpriteRenderer>();
        nsm = trans.GetComponent<NpcStateManager>();

        if(spriteTransform.GetComponent<Animator>() !=null)
        {
            anim = spriteTransform.GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (anim != null)
        {
            anim.SetBool("isWalking", nsm.isWalking);
        }
    }

    public void npcAnimation()
    {
        bounceVel += (1 - bounce) * bounceAccel;
        bounce += bounceVel;
        bounceVel *= bounceDamp;
        spriteTransform.localScale = new Vector3(bounce * flip, 1f / bounce, 1f);

        lastHop = hop;
    }

    public void WalkAnim(float multiplier, float _bounce, bool hasBounce)
    {
        flip = (rb.velocity.x < 0) ? -1f : 1f;

        if (!hasBounce) return;
        hop += Time.deltaTime * multiplier;
        if (hop > 1) hop--;

        //Sway back and forth
        float t = hop * Mathf.PI * 2;
        spriteTransform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(t) * 0.1f);
        spriteTransform.localPosition = new Vector3(0, Mathf.Abs(Mathf.Sin(t)) * 0.5f, 0);

        if (lastHop < 0.5 && hop >= 0.5f) bounce = _bounce;
        if (lastHop > 0.9 && hop <= 0.1f) bounce = _bounce;
    }

    public void StopWalkAnim()
    {
        LeanTween.reset();
        //nsm.isWalking = false;
        spriteTransform.rotation = Quaternion.identity;
        LeanTween.moveLocal(spriteTransform.gameObject, Vector3.zero, 0.2f);
        LeanTween.scale(spriteTransform.gameObject, new Vector3(Mathf.Sign(spriteTransform.localScale.x), 1, 1), 0.2f);
    }


    public void WanderAnim(NpcInteractState state)
    {
        StartCoroutine(WanderAnimCo(state));
    }
    IEnumerator WanderAnimCo(NpcInteractState state)
    {
        yield return new WaitForSeconds(wanderTime);
        sR.flipX = !sR.flipX;
        yield return new WaitForSeconds(wanderTime);
        sR.flipX = !sR.flipX;
        yield return new WaitForSeconds(wanderTime);
        state.BackToRoam();
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(trans.position - Vector3.left * (Mathf.Sign(spriteTransform.localScale.x) * 4), 3);
    }
}
