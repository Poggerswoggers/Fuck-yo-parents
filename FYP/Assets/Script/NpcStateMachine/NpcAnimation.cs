using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAnimation : MonoBehaviour
{
    [Header("Bounce")]
    private float bounce;
    float bounceVel;
    const float bounceAccel = 0.2f;
    const float bounceDamp = 0.7f;

    [Header("Wander")]
    public float wanderTime;
    
    //Hops
    float hop;
    float lastHop;

    Vector3 facing;
    [SerializeField] float flip;


    //Reference
    Transform trans;
    Transform spriteTransform;
    Rigidbody2D rb;
    SpriteRenderer sR;
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
        spriteTransform.localPosition = new Vector3(0, Mathf.Abs(Mathf.Sin(t)) * 0.3f, 0);

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
        //LeanTween.reset();
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

    public void Chatting(float dur)
    {
        StartCoroutine(ChattingCo(dur));
    }
    IEnumerator ChattingCo(float dur)
    {
        transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(dur);
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public IEnumerator Frustrated(float dur)
    {
        spriteTransform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(dur);
        spriteTransform.GetChild(0).gameObject.SetActive(false);
    }
}
