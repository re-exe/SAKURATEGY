using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// キャラの動きを管理する
/// </summary>
public class CharacterMove : MonoBehaviour{

    [SerializeField]
    [Tooltip("ID")]
    private TREE_ID id = TREE_ID.MAX;

    [Tooltip("キャラクターの種類ID")]
    public enum TREE_ID{
        SOMEIYOSHINO,
        KANHIZAKURA,
        KAWAZUSAKURA,
        HIGANZAKURA,
        OYAMAZAKURA,
        MAX
    }

    private void Awake() {
        
        switch(id){
            case TREE_ID.SOMEIYOSHINO: break;
            case TREE_ID.KANHIZAKURA:  break;
            case TREE_ID.KAWAZUSAKURA: break;
            case TREE_ID.HIGANZAKURA:  break;
            case TREE_ID.OYAMAZAKURA:  break;
        }
    }
}
