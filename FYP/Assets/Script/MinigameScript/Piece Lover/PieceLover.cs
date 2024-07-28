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
    [SerializeField] Transform piecePrefab;
    [SerializeField] float xforceOffset;
    int piecesToMatch;

    [Header("Jigsaw Slot")]
    List<Vector2Int> slotsPos = new List<Vector2Int>();
    [SerializeField] Transform slotParent;

    [Header("Slider Timer")]
    [SerializeField] SliderTimer timer;
    [SerializeField] float gameTime;
    public override void EndSequenceMethod()
    {
        Debug.Log("game");
    }

    public override void StartGame()
    {
        CreateSlots();
        CreateJigSaw();
        LeanTween.reset();
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
                jigsawPiece.GetComponent<JigSawPiece>().index = index;
                index++;

                pieces.Add(jigsawPiece);
            }
        }
        piecesToMatch = index; //Number of pieces in the jigsaw
        StartCoroutine(BreakJigSaw(pieces));
    }
    IEnumerator BreakJigSaw(List<Transform> pieces)
    {
        yield return new WaitForSeconds(2f);
        foreach(Transform piece in pieces)
        {
            piece.GetComponent<Rigidbody2D>().isKinematic = false;
            float x = Random.Range(piece.position.x - xforceOffset, piece.position.x + xforceOffset);
            Vector2 force = new Vector2(x, piece.localPosition.y);
            piece.GetComponent<Rigidbody2D>().AddForce(force.normalized*4, ForceMode2D.Impulse);
        }
        isGameActive = true;
        timer.SetTImer(gameTime, () => gameManager.OnGameOver());
    }

    public override void UpdateGame()
    {
        //Pick up jigsaw
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, lm);
            if (hit.transform.TryGetComponent(out Rigidbody2D rb))
            {
                draggingPiece = hit.transform;
                draggingPiece.LeanRotate(Vector3.zero, 0.25f).setDelay(0.25f);
                offset = draggingPiece.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);

                rb.isKinematic = true;
                rb.angularVelocity = 0;
                rb.velocity = Vector3.zero;
            }
        }
        //Let go of jigsaw
        if(draggingPiece && Input.GetMouseButtonUp(0))
        {
            draggingPiece.GetComponent<Rigidbody2D>().isKinematic = (SetBoardPiece()) ? true : false;
            draggingPiece.position += Vector3.forward;
            draggingPiece = null;
        }
        //Dragging jigsaw
        if (draggingPiece)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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

            if(index == draggingPiece.GetComponent<JigSawPiece>().index)
            {
                piecesToMatch--;
                draggingPiece.GetComponent<BoxCollider2D>().enabled = false;
                if(piecesToMatch == 0) { gameManager.OnGameOver();}
            }
        }
        return (slotsPos.Contains((Vector2Int)pos));
    }

    protected override IEnumerator InstructionCo()
    {
        yield return null;
        StartGame();
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
