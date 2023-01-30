using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterGenerate : MonoBehaviour{

    [SerializeField]
    [Tooltip("キャラクターのPrefab名")]
    private string charaName = "";

    [Tooltip("Prefab保存変数")]
    private GameObject m_character = null;

    private void Awake() {
        
        // Prefabのロード
        m_character = (GameObject)Resources.Load("Prefabs/Character/" + charaName);

        if(!m_character){
            Debug.LogError(charaName + "の生成ができませんでした。　パスを確認してください。");
        }

        // EventTriggerの取得
        EventTrigger trigger = GetComponent<EventTrigger>();

        // EventTriggerに関数を登録
        // カーソルが接触したとき
        EventTrigger.Entry enter = new EventTrigger.Entry();
        enter.eventID = EventTriggerType.PointerEnter;
        enter.callback.AddListener((e) => PointerEnter());
        trigger.triggers.Add(enter);

        // 接触から離れたとき
        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener((e) => PointerExit());
        trigger.triggers.Add(exit);

        //オブジェクトを押下したとき
        EventTrigger.Entry down = new EventTrigger.Entry();
        down.eventID = EventTriggerType.PointerDown;
        down.callback.AddListener((e) => PointerDown());
        trigger.triggers.Add(down);

        // 押下状態から離したとき
        EventTrigger.Entry up = new EventTrigger.Entry();
        up.eventID = EventTriggerType.PointerUp;
        up.callback.AddListener((e) => PointerUp());
        trigger.triggers.Add(up);
    }

    /// <summary>
    /// カーソルが接触したとき
    /// </summary>
    public void PointerEnter(){
        Debug.Log(this.gameObject.name.ToString() + "に接触した");
    }

    /// <summary>
    /// 接触から離れたとき
    /// </summary>
    public void PointerExit(){
        Debug.Log(this.gameObject.name.ToString() + "から離れた");
    }

    /// <summary>
    /// オブジェクトを押下したとき
    /// </summary>
    public void PointerDown(){
        Debug.Log(this.gameObject.name.ToString() + " : 押下(ON)");
    }

    /// <summary>
    /// 押下状態から離したとき
    /// </summary>
    public void PointerUp(){
        Debug.Log(this.gameObject.name.ToString() + " : 押下(OFF)");
    }
}
