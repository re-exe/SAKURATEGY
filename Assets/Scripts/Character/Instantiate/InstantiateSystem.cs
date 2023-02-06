using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class InstantiateSystem : MonoBehaviour{

    [Tooltip("インスタンス")]
    public static InstantiateSystem instance = null;

    [Header("トラッキング許可")]
    public bool trackingFlag = true;

    [SerializeField]
    [Header("エリア判定")]
    private CircleCollider2D m_areaColl = null;


    private void Awake() {
        if(!instance){
            instance = this;
        }
    }

    /// <summary>
    /// キャラクターの生成 + トラッキング
    /// </summary>
    /// <param name="chara">キャラクターのインスタンス</param>
    public void CharacterInstantiate(GameObject chara){

        // 初期位置を設定
        Vector3 mouse = Input.mousePosition;
        Vector3 pos = (Vector2)Camera.main.ScreenToWorldPoint(mouse);
        GameObject m_chara = Instantiate(chara, pos, Quaternion.identity);

        // トラッキング開始
        trackingFlag = true;
        this.UpdateAsObservable()
            .Where(_=> trackingFlag)
            .Subscribe(_=>{
                Vector3 mouse = Input.mousePosition;
                m_chara.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(mouse);
            }).AddTo(this);
    }

    // TODO:キャラクターがすべて同じ場所に生成されるバグを修正する
}
