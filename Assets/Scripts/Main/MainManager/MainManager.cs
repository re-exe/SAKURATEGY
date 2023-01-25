using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// ゲームメインを管理する
/// </summary>
public class MainManager : MonoBehaviour{

    [Tooltip("現在の状態")]
    private MAIN_STATE m_nowState = MAIN_STATE.DUMMY;

    [Tooltip("直前の状態")]
    private MAIN_STATE m_preState = MAIN_STATE.DUMMY;

    public enum MAIN_STATE{
        DUMMY,
        COUNTDOWN,
        START,
        MAIN,
        RESULT
    }

    private void Awake() {
        
        // ゲームメインの更新
        this.UpdateAsObservable()
            .Where(_=> m_nowState != MAIN_STATE.DUMMY)
            .Subscribe(_=>UpdateState())
            .AddTo(this);
    }

    // 更新処理
    private void UpdateState(){
        switch(m_nowState){
            case MAIN_STATE.COUNTDOWN: break;
            case MAIN_STATE.START: break;
            case MAIN_STATE.MAIN: break;
            case MAIN_STATE.RESULT: break;
        }
    }
}
