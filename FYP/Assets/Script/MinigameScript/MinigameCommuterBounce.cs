using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameCommuterBounce : MonoBehaviour
{
    [SerializeField] float initialYHeight;
    [SerializeField] float heightMultiplier;
    [Header("Bounce")]
    [SerializeField] float bounce;
    float bounceVel;
    private float bounceAccel = 0.2f;
    private float bounceDamp = 0.7f;

    //Hops
    float hop;
    float lastHop;

    [SerializeField] float flip;

    [Header("Bounce variables")]
    [SerializeField] float multiplier;
    [SerializeField] float _bounce;

    private void Start()
    {
        initialYHeight = transform.localPosition.y;
    }


    private void FixedUpdate()
    {
        npcAnimation();
        WalkAnim();

    }
    public void npcAnimation()
    {
        bounceVel += (1 - bounce) * bounceAccel;
        bounce += bounceVel;
        bounceVel *= bounceDamp;
        transform.localScale = new Vector3(bounce * flip, flip/bounce, 1f);

        lastHop = hop;
    }

    public void WalkAnim()
    {
        //flip = transform.localScale.x;

        hop += Time.deltaTime * multiplier;
        if (hop > 1) hop--;

        //Sway back and forth
        float t = hop * Mathf.PI * 2;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(t) * 2f);
        transform.localPosition = new Vector3(0, Mathf.Abs(Mathf.Sin(t)) * heightMultiplier + initialYHeight, 0);

        if (lastHop < 0.5 && hop >= 0.5f) bounce = _bounce;
        if (lastHop > 0.9 && hop <= 0.1f) bounce = _bounce;
    }
}
