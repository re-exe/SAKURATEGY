using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CreateCharacterParamAsset")]
public class CharacterParamAsset : ScriptableObject{

    [Header("キャラクターのパラメータ")]
    public List<CharacterParam> characterParamList = new List<CharacterParam>();

    [System.Serializable]
    public class CharacterParam{
        [Header("名前")]
        public string name = "";

        [Header("モデル画像")]
        public Sprite sprite = null;

        [Header("出撃時間(s/f)")]
        public float sortieTime = 0f;

        [Header("コスト")]
        public float cost = 0f;

        [Header("HP")]
        public float hp = 0f;

        [Header("攻撃力")]
        public float attackPower = 0f;

        [Header("移動速度(s/f)")]
        public float moveSpeed = 0f;
        
        [Header("射程")]
        public float range = 0f;

        [Header("頻度")]
        public float frequency = 0f;

        [Header("説明")]
        public string description = "";
    }
}
