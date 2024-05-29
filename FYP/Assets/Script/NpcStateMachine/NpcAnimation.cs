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

    Vector3 facing;


    [SerializeField] float flip;

    [Header("Chatting")]
    public GameObject chatBubblePrefab;

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
        spriteTransform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(t) * 2f);
        spriteTransform.localPosition = new Vector3(0, Mathf.Abs(Mathf.Sin(t)) * 0.5f, 0);

        if (lastHop < 0.5 && hop >= 0.5f) bounce = _bounce;
        if (lastHop > 0.9 && hop <= 0.1f) bounce = _bounce;
    }

    public void BounceAnim(float multiplier, float _bounce, bool hasBounce, float maxHeight)
    {
        flip = Mathf.Sign(facing.x);  

        if (!hasBounce) return;
        hop += Time.deltaTime * multiplier;
        if (hop > 1) hop--;

        //Sway back and forth
        float t = hop * Mathf.PI * 2;
        spriteTransform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(t) * 5f);
        spriteTransform.localPosition = new Vector3(0, Mathf.Abs(Mathf.Sin(t)) * maxHeight, 0);

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
    public void faceTarget(Transform target)
    {
        if(target.position.x - trans.position.x > 0)
        {
            spriteTransform.localScale = Vector3.one;
        }
        else
        {
            spriteTransform.localScale = new Vector3(-1,1,1);
        }
        facing = spriteTransform.localScale;
        Debug.Log(facing+ " "+gameObject.name);
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
        state.BackToRoam(nsm.roamState);
    }

    public void Chatting(bool isChatting, float dur)
    {
        StartCoroutine(ChattingCo(isChatting, dur));
    }
    IEnumerator ChattingCo(bool isChatting, float dur)
    {
        GameObject bubble = Instantiate(chatBubblePrefab, transform.position + Vector3.up*1.5f, Quaternion.identity);
        yield return new WaitForSeconds(dur);
        Destroy(bubble);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position - Vector3.left * (Mathf.Sign(rb.velocity.x) * 4.5f), 2);
    }
}
