using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceLover : BaseMiniGameClass
{
    [SerializeField] LayerMask lm;

    Transform draggingPiece = null;
    Vector3 offset;

    [SerializeField] Transform dropPoint;

    [Header("JigsawTexture")]
    [SerializeField] Texture2D jigsawTexture;
    [SerializeField] int cellCount;

    [Header("Jigsaw pieces")]
    [SerializeField] float jigsawFixedTime;
    [SerializeField] Transform piecePrefab;
    private float xforceOffset = 0.2f;
    [SerializeField] float splitForce;
    int piecesToMatch;

    [Header("Jigsaw Slot")]
    List<Vector2Int> slotsPos = new List<Vector2Int>();
    [SerializeField] Transform slotParent;

    [Header("Slider Timer")]
    [SerializeField] SliderTimer timer;
    [SerializeField] float gameTime;

    [Header("Camera")]
    [SerializeField] Camera minigameCam;

    [Header("Particle")]
    [SerializeField] GameObject snapParticle;

    public override void EndSequenceMethod()
    {
        score = 2000 - 500 * piecesToMatch;
        UnloadedAndUpdateScore(score);
    }

    private void Awake()
    {
        CreateSlots();
    }

    public override void StartGame()
    {
        LeanTween.reset();
        CreateJigSaw();
    }

    void CreateSlots()
    {
        foreach (Transform slot in slotParent)
        {
            Vector3Int pos = Vector3Int.FloorToInt(slot.position);
            slotsPos.Add((Vector2Int)pos);
        }
    }

    //Slice the base jigsaw into even squares
    void CreateJigSaw()
    {
        List<Transform> pieces = new List<Transform>();

        int sliceWidth = jigsawTexture.width / cellCount;   //Get the size of each jigsaw piece after slicing
        int sliceHeight = jigsawTexture.height / cellCount;
        int index = 0;      //To set jigsaw piece index

        for (int x=0; x<cellCount; x++)
        {
            for(int y=0; y<cellCount; y++)
            {
                var rect = new Rect(sliceWidth*x, sliceHeight*y, sliceWidth, sliceHeight);  //get the rect
                // Create the sprite
                Sprite sprite = Sprite.Create(jigsawTexture, rect, Vector2.one * 0.5f, sliceWidth);     //Create the sprite
                Transform jigsawPiece = Instantiate(piecePrefab, transform.parent);               //Instaniate prefab
                jigsawPiece.position = (Vector3Int)slotsPos[index];
                jigsawPiece.GetComponent<SpriteRenderer>().sprite = sprite;     //Set the prefab sprite to created sprite               
                jigsawPiece.GetComponent<JigSawPiece>().Index = index;
                index++;

                pieces.Add(jigsawPiece);
            }
        }
        piecesToMatch = index; //Number of pieces in the jigsaw
        StartCoroutine(BreakJigSaw(pieces));
    }
    IEnumerator BreakJigSaw(List<Transform> pieces)
    {
        yield return new WaitForSeconds(jigsawFixedTime);
        foreach(Transform piece in pieces)
        {
            piece.GetComponent<Rigidbody2D>().isKinematic = false;
            float x = Random.Range(piece.position.x - xforceOffset, piece.position.x + xforceOffset);
            Vector2 force = new Vector2(x, piece.localPosition.y);
            piece.GetComponent<Rigidbody2D>().AddForce(force.normalized*splitForce, ForceMode2D.Impulse);
        }
        isGameActive = true;
        timer.SetTImer(gameTime, () => gameManager.OnGameOver());
    }

    public override void UpdateGame()
    {
        //Pick up jigsaw
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = minigameCam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, lm);
            if (!hit.collider) return;
            if (hit.transform.TryGetComponent(out Rigidbody2D rb))
            {
                draggingPiece = hit.transform;
                draggingPiece.LeanRotate(Vector3.zero, 0.25f).setDelay(0.25f);
                offset = draggingPiece.position - minigameCam.ScreenToWorldPoint(Input.mousePosition);

                rb.isKinematic = true;
                rb.angularVelocity = 0;
                rb.velocity = Vector3.zero;
            }
        }
        //Let go of jigsaw
        if(draggingPiece && Input.GetMouseButtonUp(0))
        {
            draggingPiece.GetComponent<Rigidbody2D>().isKinematic = (SetBoardPiece());
            draggingPiece.position += Vector3.forward;
            draggingPiece = null;
        }
        //Dragging jigsaw
        if (draggingPiece)
        {
            Vector3 mousePos = minigameCam.ScreenToWorldPoint(Input.mousePosition);
            mousePos += offset;
            draggingPiece.position = mousePos;
        }
    }

    bool SetBoardPiece()
    {
        Vector3Int pos = Vector3Int.RoundToInt(draggingPiece.position)/2*2;       

        if (slotsPos.Contains((Vector2Int)pos))
        {
            int index = slotsPos.IndexOf((Vector2Int)pos);
            draggingPiece.transform.position = pos;
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.attachJig);
            }

            if (index == draggingPiece.GetComponent<JigSawPiece>().Index)
            {
                piecesToMatch--;
                draggingPiece.GetComponent<BoxCollider2D>().enabled = false;
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.PlaySFX(AudioManager.instance.correctJig);
                }
                //Set particle
                GameObject particle = Instantiate(snapParticle);
                particle.transform.position = (Vector3Int)slotsPos[index];  //It self destructs
                if(piecesToMatch == 0) { gameManager.OnGameOver();}
            }
        }
        return (slotsPos.Contains((Vector2Int)pos));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<JigSawPiece>())
        {
            collision.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            float x = Random.Range(-dropPoint.lossyScale.x / 2, dropPoint.lossyScale.x / 2);
            collision.transform.position = new Vector2(x, dropPoint.position.y);
        }
    }
}
