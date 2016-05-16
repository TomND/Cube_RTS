using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemBlock : ItemPickable {

    public BlockType.Type blockType;
    public string BlockName;

    void Start() {

        itemType = ItemType.Block;

    }
}