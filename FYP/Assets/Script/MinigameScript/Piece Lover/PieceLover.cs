using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceLover : BaseMiniGameClass
{
    [SerializeField] LayerMask lm;

    Transform draggingPiece = null;
    Vector3 offset;

    [Header("JigsawTexture")]
    [SerializeField] Texture2D jigsawTexture;
    [SerializeField] int cellCount;

    [Header("Jigsaw pieces")]
    [SerializeField] Transform piecePrefab;
    [SerializeField] List<Transform> pieces;

    [Header("Jigsaw Slot")]
    [SerializeField] List<Vector2Int> slots;
    [SerializeField] Transform slotParent;
    public override void EndSequenceMethod()
    {
        
    }

    public override void StartGame()
    {
        isGameActive = true;
        CreateJigSaw();
    }

    //Slice the base jigsaw into even squares
    void CreateJigSaw()
    {
        int sliceWidth = jigsawTexture.width / cellCount;
        int sliceHeight = jigsawTexture.height / cellCount;

        for (int x=0; x<cellCount; x++)
        {
            for(int y=0; y<cellCount; y++)
            {
                var rect = new Rect(sliceWidth*x, sliceHeight*y, sliceWidth, sliceHeight);  //get the rect
                // Create the sprite
                Sprite sprite = Sprite.Create(jigsawTexture, rect, Vector2.one * 0.5f, sliceWidth);     //Create the sprite
                Transform jigsawPiece = Instantiate(piecePrefab);               //Instaniate prefab
                jigsawPiece.GetComponent<SpriteRenderer>().sprite = sprite;     //Set the prefab sprite to created sprite

                pieces.Add(jigsawPiece);
            }
        }

        foreach(Transform slot in slotParent)
        {
            slots.Add(slot.GetComponent<JigSawPiece>().GetPos());
        }
    }

    public override void UpdateGame()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, lm);
            if (hit.collider!=null)
            {
                draggingPiece = hit.transform;
                offset = draggingPiece.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                offset += Vector3.back;
                draggingPiece.GetComponent<Rigidbody2D>().isKinematic = true;
            }
        }

        if(draggingPiece && Input.GetMouseButtonUp(0)){
            draggingPiece.GetComponent<Rigidbody2D>().isKinematic = false;
            SetBoardPiece();
            draggingPiece.position += Vector3.forward;
            draggingPiece = null;
        }

        if (draggingPiece)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos += offset;
            draggingPiece.position = mousePos;
        }
    }

    void SetBoardPiece()
    {
        Vector3Int pos = Vector3Int.RoundToInt(draggingPiece.position)/2*2;
        if (slots.Contains((Vector2Int)pos))
        {
            int index = slots.IndexOf((Vector2Int)pos);
            draggingPiece.transform.position = pos;
            draggingPiece.GetComponent<Rigidbody2D>().isKinematic = true;

            //Add tile check and disabling placing tile on an occupied tile
        }
    }

    protected override IEnumerator InstructionCo()
    {
        yield return null;
        StartGame();
    }
}
