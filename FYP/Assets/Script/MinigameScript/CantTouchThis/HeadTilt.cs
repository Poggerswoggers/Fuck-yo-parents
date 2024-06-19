using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HeadTilt : BaseMiniGameClass
{   
    [Header("MiniGame Settings")]
    [SerializeField] float forceAmount = 10f;
    private Rigidbody2D rb;

    [SerializeField] float fallThreshold = 15f;
    [SerializeField] float maxFallingTorque;
    [SerializeField] float minFallingTorque;

    private float rotationZ;
    private float fallTorque;

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


    protected override IEnumerator InstructionCo()
    {
        instructionPanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        instructionPanel.SetActive(false);

        StartGame();
    }

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
        if(isGameOver && isGameActive)
        {           
            rb.bodyType = RigidbodyType2D.Static;
        }
        else
        {            
            if (Input.GetMouseButtonDown(0))
            {
                rb.AddTorque(forceAmount * CheckMouseSide());
                awakeTime = _awakeTime;
                awake = true;
                Debug.Log("Clicked");

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
        Debug.Log("wdw");
        rb.AddTorque(forceAmount * Mathf.Sign(rotationZ)*30);
    }


    public override void EndSequenceMethod()
    {
        Debug.Log("Game Over");
        sr.sprite = EyeOpen;

        //LeanTween.reset();
        //LeanTween.rotateZ(gameObject, 0, 0.3f);
    }
}
