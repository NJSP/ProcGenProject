using System.Collections.Generic;
using UnityEngine;

public class BSPNode
{
    public RectInt Rect;
    public BSPNode Left;
    public BSPNode Right;
    public RectInt Room;
    public RoomType RoomType { get; set; }
    public BSPNode(RectInt rect)
    {
        Rect = rect;
    }
}