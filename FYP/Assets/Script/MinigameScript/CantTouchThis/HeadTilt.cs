using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HeadTilt : BaseMiniGameClass
{
    [Header("Strike")]
    [SerializeField] int Strikes;
    
    [Header("MiniGame Settings")]
    [SerializeField] float forceAmount = 10f;
    private Rigidbody2D rb;

    [SerializeField] float fallThreshold = 15f;
    [SerializeField] float maxFallingTorque;
    [SerializeField] float minFallingTorque;

    private float rotationZ;
    private float fallTorque;
    private bool reset;

    [SerializeField] float maxLeanAngle;
    [SerializeField] int startTorque;
    int[] dir;

    [Header("Sprites")]
    //HeadReference
    [SerializeField] Sprite Eyeclosed;
    [SerializeField] Sprite EyeOpen;
    bool awake;
    [SerializeField] float awakeTime;
    float _awakeTime;

    SpriteRenderer sr;

    //Game Time
    [SerializeField] SliderTimer slider;
    [SerializeField] float gameTIme;


    public override void StartGame()
    {
        _awakeTime = awakeTime;

        sr = GetComponentInChildren<SpriteRenderer>();
        sr.sprite = Eyeclosed;

        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        dir = new int[] {-startTorque, startTorque };
        int i = UnityEngine.Random.Range(0, dir.Length);
        rb.AddTorque(dir[i]);

        slider.SetTImer(gameTIme, () => gameManager.OnGameOver());

        isGameActive = true;
    }

    public override void UpdateGame()
    {
        if (!isGameActive) return;
        if (Input.GetMouseButtonDown(0))
        {
            rb.AddTorque(forceAmount * CheckMouseSide());
            awakeTime = _awakeTime;
            awake = true;

            minFallingTorque *= 1.02f;
        }
        if (awakeTime > 0 && awake)
        {
            sr.sprite = EyeOpen;
            awakeTime -= Time.deltaTime;
        }
        else
        {
            awake = false;
            sr.sprite = Eyeclosed;
        }
             
        //rb.AddTorque(-Mathf.Sign(transform.rotation.eulerAngles.z) * fallAmount*0.05f);
    }

    void ApplyFallingBehavior(float rotationZ)
    {
        // Calculate the falling torque based on the rotation angle
        fallTorque = Mathf.Lerp(minFallingTorque, maxFallingTorque, (Mathf.Abs(rotationZ) - fallThreshold) / (maxLeanAngle - fallThreshold));
        rb.AddTorque(Mathf.Sign(rotationZ) * fallTorque);
    }

    private void FixedUpdate()
    {
        if (reset)
        {
            rb.AddTorque(-Mathf.Sign(rotationZ) * fallTorque * 10f);
            if (Mathf.Abs(rotationZ) <= 0.5) { rb.bodyType = RigidbodyType2D.Static; }
        }

        if (!isGameActive) return;
        rotationZ = transform.rotation.eulerAngles.z;
        if (rotationZ > 180)
            rotationZ -= 360;

        if (Mathf.Abs(rotationZ) > fallThreshold)
        {
            ApplyFallingBehavior(rotationZ);
        }
    }

    float CheckMouseSide()
    {
        return (Input.mousePosition.x > Screen.width / 2) ? 1 : -1;
    }

    public void HitPassenger()
    {

        Strikes++;
        rb.AddTorque(forceAmount * Mathf.Sign(rotationZ)*30);
    }


    public override void EndSequenceMethod()
    {
        isGameActive = false;
        reset = true;
        sr.sprite = EyeOpen;
        StartCoroutine(EndSequenceCo());
    }
    IEnumerator EndSequenceCo()
    {
        yield return new WaitForSeconds(2f);
        UnloadedAndUpdateScore(2000 - Strikes*100);
    }
}
