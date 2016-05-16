using UnityEngine;
using System.Collections;
using System.Reflection;

public class BlockType : MonoBehaviour {

    private static string blocksPath = "Blocks/";

    public enum Type {
        Grass,
        Dirt,
        Dessert,
        Snow,
        Water
    }

    public static GameObject GetBlock(string blockType) {
        PropertyInfo[] property = typeof(BlockType).GetProperties();
        foreach (PropertyInfo blockVar in property) {
            if (blockType.Equals(blockVar.Name)) {
                return ((GameObject)Resources.Load(blocksPath + "Block." + 
                    blockVar.Name, typeof(GameObject)));
            }
        }
        print("Block specified as: '" + blockType + "' not found.");
        return null;
    }

    public static GameObject Grass {
        get { 
            return ((GameObject)Resources.Load(blocksPath + "Block.Grass", typeof(GameObject)));
        }
    }

    public static GameObject Dirt {
        get { 
            return ((GameObject)Resources.Load(blocksPath + "Block.Dirt", typeof(GameObject)));
        }
    }

    public static GameObject Dessert {
        get { 
            return ((GameObject)Resources.Load(blocksPath + "Block.Dessert", typeof(GameObject)));
        }
    }

    public static GameObject Snow {
        get { 
            return ((GameObject)Resources.Load(blocksPath + "Block.Snow", typeof(GameObject)));
        }
    }

    public static GameObject Water {
        get { 
            return ((GameObject)Resources.Load(blocksPath + "Block.Water", typeof(GameObject)));
        }
    }


}

