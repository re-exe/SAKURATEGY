using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using UnityEngine.SceneManagement;
using SceneChanger;

/// <summary>
/// ゲームメインを管理する
/// </summary>
public class MainManager : MonoBehaviour{

    [Tooltip("Instance")]
    public static MainManager instance = null;

    [Header("ゲーム更新許可")]
    public bool gameUpdatePermit = false;

    [Tooltip("現在の状態")]
    private MAIN_STATE m_nowState = MAIN_STATE.DUMMY;

    [Tooltip("直前の状態")]
    private MAIN_STATE m_preState = MAIN_STATE.DUMMY;

    [Tooltip("初期化フラグ")]
    private bool initFlag = false;

    /*-------------------------------------------------------------------------------*/

    [Space(10)]
    [Header("消費リソース系")]

    [Tooltip("温暖ゲージ")]
    public Image warmGauge = null;

    [Tooltip("敵ゲージ")]
    public Image enemyGauge = null;

    [Tooltip("神木ゲージ")]
    public Image treeGauge = null;

    /*-------------------------------------------------------------------------------*/

    [Space(10)]
    [Header("ゲームスタート画面")]

    [SerializeField]
    [Tooltip("画面")]
    private GameObject startPanel = null;

    [SerializeField]
    [Tooltip("テキスト")]
    private CanvasGroup startText = null;
    


    /*-------------------------------------------------------------------------------*/

    [Space(10)]
    [Header("ゲームオーバー画面")]
    [SerializeField]
    [Tooltip("ゲームオーバー画面")]
    private GameObject gameOverWindow = null;

    [SerializeField]
    [Tooltip("終了ボタン")]
    private Button exitButton = null;

    [SerializeField]
    [Tooltip("コンティニューボタン")]
    private Button continueButton = null;

    [SerializeField]
    [Tooltip("ゲームクリア音")]
    private AudioClip gameClearSound = null;

    [SerializeField]
    [Tooltip("ゲームオーバー音")]
    private AudioClip gameOverSound = null;

    [SerializeField]
    [Tooltip("ソース")]
    private AudioSource source = null;

    /*-------------------------------------------------------------------------------*/

    [Space(10)]
    [Header("ゲームクリア画面")]
    [SerializeField]
    [Tooltip("ゲームクリア画面")]
    private GameObject gameClearWindow = null;

    [SerializeField]
    [Tooltip("戻るボタン")]
    private Button backButton = null;

    /*-------------------------------------------------------------------------------*/
    

    public enum MAIN_STATE{
        DUMMY,
        COUNTDOWN,
        START,
        MAIN,
        GameClear,
        GameOver
    }

    private void Awake() {

        // インスタンス生成
        if(!instance)
            instance = this;

        startPanel.SetActive(true);
        
        // ゲームメインの更新
        this.UpdateAsObservable()
            .Where(_=> gameUpdatePermit)
            .Subscribe(_=>UpdateState())
            .AddTo(this);

        ChangeState(MAIN_STATE.COUNTDOWN);
        gameUpdatePermit = true;
        

        // ゲージ系を0に設定
        warmGauge.fillAmount = 0f;
        enemyGauge.fillAmount = 0f;
        treeGauge.fillAmount = 1f;

        startText.alpha = 0f;

        gameClearWindow.SetActive(false);
        gameOverWindow.SetActive(false);

        exitButton.OnClickAsObservable()
            .Subscribe(_=>{
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
            }).AddTo(this);

        continueButton.OnClickAsObservable()
            .Subscribe(_=> {
                SceneManager.LoadScene("Preparation");
            }).AddTo(this);

        backButton.OnClickAsObservable()
            .Subscribe(_=> {
                FadeSceneChanger.Instance.ChangeSceneWithFade("Title", 2f, 2f);
            }).AddTo(this);
    }

    // 更新処理
    private void UpdateState(){
        switch(m_nowState){
            case MAIN_STATE.COUNTDOWN: CountDown(); break;
            case MAIN_STATE.START: GameStart(); break;
            case MAIN_STATE.MAIN: MainUpdate(); break;
            case MAIN_STATE.GameClear: GameClear(); break;
            case MAIN_STATE.GameOver: GameOver(); break;
        }
    }

    // 状態遷移
    private void ChangeState(MAIN_STATE state){
        m_preState = m_nowState;
        m_nowState = state;
        initFlag = true;
        Debug.Log("NowState : " + m_nowState.ToString());
    }

    private void CountDown(){
        if(initFlag){
            initFlag = false;

            enemyGauge.DOFillAmount(1f, 3f)
                .SetAutoKill(true)
                .SetEase(Ease.Linear)
                .Play()
                .OnComplete(() => ChangeState(MAIN_STATE.START));
            
        }
    }

    private void GameStart(){
        if(initFlag){
            initFlag = false;

            startText.DOFade(1f, 0.8f)
                .SetEase(Ease.Flash, 11)
                .SetAutoKill(true)
                .Play()
                .OnComplete(() =>{

                    startPanel.transform.DOLocalMoveY(-720f, 2f)
                        .SetEase(Ease.OutQuart)
                        .SetAutoKill(true)
                        .Play()
                        .OnComplete(() =>{
                             EnemyInstantiateSystem.instance.EnemyInstantiateStart();
                            ChangeState(MAIN_STATE.MAIN);
                            startPanel.SetActive(false);
                        });
                });
        }
    }

    private void MainUpdate(){
        if(initFlag){
            initFlag = false;

            StartCoroutine(warmGaugeAdditionCol());
        }

        if(treeGauge.fillAmount == 0f){
            Time.timeScale = 0f;
            ChangeState(MAIN_STATE.GameOver);
        }

        if(enemyGauge.fillAmount == 0f){
            Time.timeScale = 0f;
            ChangeState(MAIN_STATE.GameClear);
        }
    }

    private void GameClear(){
        if(initFlag){
            initFlag = false;
        }

        source.clip = gameClearSound;
        source.Play();
        gameClearWindow.SetActive(true);
    }

    private void GameOver(){
        if(initFlag){
            initFlag = false;
        }

        source.clip = gameOverSound;
        source.Play();
        gameOverWindow.SetActive(true);
    }

    /// <summary>
    /// 温暖ゲージの加算処理(非同期)
    /// </summary>
    /// <returns></returns>
    private IEnumerator warmGaugeAdditionCol(){
        while(gameUpdatePermit){

            // 0.5秒ごとに1%増加させる
            yield return new WaitForSeconds(0.5f);
            warmGauge.fillAmount += 0.01f;

            if(warmGauge.fillAmount == 1f){
                warmGauge.fillAmount = 1f;
            }
        }
    }
}
