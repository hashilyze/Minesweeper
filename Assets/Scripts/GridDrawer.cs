using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridDrawer : MonoBehaviour {
    public Vector2Int Size => m_size;

    // Allocate and draw
    public void DrawGrid(in Vector2Int size, Sprite tile) {
        Allocate(in size);
        RedrawGrid(tile);
    }
    public void DrawGrid(in Vector2Int size, System.Func<Vector2Int, Sprite> tileGenerator) {
        Allocate(in size);
        RedrawGrid(tileGenerator);
    }
    
    // Manage cell 
    public SpriteRenderer GetTile(in Vector2Int offset) => m_tiles[offset.x, offset.y];
    public SpriteRenderer GetPiece(in Vector2Int offset) => m_pieces[offset.x, offset.y];

    // Deallocate all resources
    public void Clear() {
        m_size = Vector2Int.zero;

        if (m_tileHolder != null) {
            Destroy(m_tileHolder.gameObject);
            m_tileHolder = null;
        }
        if (m_pieceHolder != null) {
            Destroy(m_pieceHolder.gameObject);
            m_pieceHolder = null;
        }

        m_tiles = null;
        m_pieces = null;
    }

    // Reuse allocated resources and draw
    public void RedrawGrid(Sprite tile) {
        for (int y = 0, yLen = m_size.y; y != yLen; ++y) {
            for (int x = 0, xLen = m_size.x; x != xLen; ++x) {
                m_tiles[x, y].sprite = tile;
                m_pieces[x, y].sprite = null;
            }
        }
    }
    public void RedrawGrid(System.Func<Vector2Int, Sprite> tileGenerator) {
        for (int y = 0, yLen = m_size.y; y != yLen; ++y) {
            for (int x = 0, xLen = m_size.x; x != xLen; ++x) {
                m_tiles[x, y].sprite = tileGenerator(new Vector2Int(x, y));
                m_pieces[x, y].sprite = null;
            }
        }
    }


    private Vector2Int m_size;
    private Transform m_tileHolder;
    private SpriteRenderer[,] m_tiles;
    private Transform m_pieceHolder;
    private SpriteRenderer[,] m_pieces;


    private void Allocate(in Vector2Int size) {
        m_size = size;

        m_tileHolder = new GameObject("Tile Holder").transform;
        m_tileHolder.transform.parent = transform;
        m_tileHolder.position = Vector2.zero;
        m_tiles = new SpriteRenderer[size.x, size.y];

        m_pieceHolder = new GameObject("Piece Holder").transform;
        m_pieceHolder.transform.parent = transform;
        m_pieceHolder.position = Vector2.zero;
        m_pieces = new SpriteRenderer[size.x, size.y];


        int objectID = 0;
        for (int y = 0, yLen = size.y; y != yLen; ++y) {
            for (int x = 0, xLen = size.x; x != xLen; ++x) {
                Vector2 objectPos = new Vector2(x - (size.x - 1) * 0.5f, y - (size.y - 1) * 0.5f);

                SpriteRenderer newTile = new GameObject($"Tile ({objectID})", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
                newTile.transform.position = objectPos;
                newTile.transform.parent = m_tileHolder;
                newTile.sortingOrder = 0;
                m_tiles[x, y] = newTile;

                SpriteRenderer newPiece = new GameObject($"Piece ({objectID})", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
                newPiece.transform.position = objectPos;
                newPiece.transform.parent = m_pieceHolder;
                newTile.sortingOrder = -1;
                m_pieces[x, y] = newPiece;

                ++objectID;
            }
        }
    }
}

public static class GridDrawer_Vector2 {
    public static void DrawGrid(this GridDrawer gridDrawer, int x, int y, Sprite tile) {
        gridDrawer.DrawGrid(new Vector2Int(x, y), tile);
    }
    public static void DrawGrid(this GridDrawer gridDrawer, int x, int y, System.Func<int, int, Sprite> tileGenerator) {
        gridDrawer.DrawGrid(new Vector2Int(x, y), (offset) => tileGenerator(offset.x, offset.y));
    }

    public static SpriteRenderer GetTile(this GridDrawer gridDrawer, int x, int y) => gridDrawer.GetTile(new Vector2Int(x, y));
    public static SpriteRenderer GetPiece(this GridDrawer gridDrawer, int x, int y) => gridDrawer.GetPiece(new Vector2Int(x, y));

    public static void RedrawGrid(this GridDrawer gridDrawer, System.Func<int, int, Sprite> tileGenerator) {
        gridDrawer.RedrawGrid((offset) => tileGenerator(offset.x, offset.y));
    }
}