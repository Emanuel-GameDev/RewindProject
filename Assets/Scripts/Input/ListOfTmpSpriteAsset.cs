using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

[CreateAssetMenu(fileName = "List of Sprite Asset", menuName = "List of Sprite Asset",order = 0)]
public class ListOfTmpSpriteAsset : ScriptableObject
{
    public List<TMP_SpriteAsset> spriteAssets;
}
