using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

/// <summary>
/// 準備画面管理クラス
/// </summary>
public class PreparationManager : MonoBehaviour{

    [Header("UI")]
    [SerializeField]
    [Tooltip("キャラクターのボタンリスト")]
    private List<CherryTreeButton> cherryTreeButtonList = new List<CherryTreeButton>((int)TREE_ID.MAX);

    [Space(10)]
    [SerializeField]
    [Tooltip("ポップアップ要素")]
    private PopupElement popupElement = new PopupElement();

    [Space(10)]
    [SerializeField]
    [Tooltip("スタートボタン")]
    private Button battleStartButton = null;

    [SerializeField]
    [Tooltip("スタートチェック画面")]
    private GameObject startCheckCanvas = null;

    [Space(10)]
    [SerializeField]
    [Tooltip("ポップアップ画面")]
    private GameObject popupObject = null;

    [SerializeField]
    [Tooltip("戻るボタン")]
    private Button popupBackButton = null;

    [Tooltip("パラメータ保管リスト")]
    private List<CharacterParamAsset.CharacterParam> characterParamList = new List<CharacterParamAsset.CharacterParam>();

    [System.Serializable]
    [Tooltip("キャラクターのボタン")]
    public class CherryTreeButton{
        [Tooltip("名前")]
        public string name = "";

        [Tooltip("ボタン")]
        public Button button = null;

        [Tooltip("ID")]
        public TREE_ID id = TREE_ID.MAX;
    }

    [System.Serializable]
    [Tooltip("ポップアップ要素")]
    public class PopupElement{
        [Tooltip("名前")]
        public TextMeshProUGUI name = null;

        [Tooltip("モデルの画像")]
        public Image image = null;

        [Tooltip("出撃時間")]
        public TextMeshProUGUI sortieTime = null;

        [Tooltip("コスト")]
        public TextMeshProUGUI cost = null;

        [Tooltip("HP")]
        public TextMeshProUGUI hp = null;

        [Tooltip("攻撃力")]
        public TextMeshProUGUI attackPower = null;

        [Tooltip("移動速度")]
        public TextMeshProUGUI moveSpeed = null;
        
        [Tooltip("射程")]
        public TextMeshProUGUI range = null;

        [Tooltip("頻度")]
        public TextMeshProUGUI frequency = null;

        [Tooltip("説明")]
        public TextMeshProUGUI description = null;
    }
    
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
        // パラメータをロード
        CharacterParamAsset characterParam = Resources.Load("CharacterParamAsset") as CharacterParamAsset;

        // リストを保管
        characterParamList = characterParam.characterParamList;

        // ポップアップ画面非表示
        popupObject.SetActive(false);

        // 確認画面非表示
        startCheckCanvas.SetActive(false);

        cherryTreeButtonList.ForEach((prop) =>{
            prop.button.OnClickAsObservable()
                .Subscribe(_=>{
                    SetButtonInteractable(false);
                    SetCharacterElements(prop.id);
                    popupObject.SetActive(true);
                }).AddTo(this);
        });

        popupBackButton.OnClickAsObservable()
            .Where(_=> popupObject.activeSelf)
            .Subscribe(_=>{
                SetButtonInteractable(true);
                popupObject.SetActive(false);
            }).AddTo(this);

        battleStartButton.OnClickAsObservable()
            .Where(_=> !startCheckCanvas.activeSelf)
            .Subscribe(_=> startCheckCanvas.SetActive(true))
            .AddTo(this);
    }

    /// <summary>
    /// ボタンの挙動を管理する
    /// </summary>
    /// <param name="setActive">true:入力を許可/false:入力を禁止</param>
    private void SetButtonInteractable(bool setActive){
        cherryTreeButtonList.ForEach((prop) =>{
            prop.button.interactable = setActive;
            battleStartButton.interactable = setActive;
        });
    }

    /// <summary>
    /// リストからポップアップ画面に反映させる関数
    /// </summary>
    /// <param name="id"></param>
    private void SetCharacterElements(TREE_ID id){

        var param = characterParamList[(int)id];
        popupElement.name.SetText(param.name);
        popupElement.image.sprite = param.sprite;
        popupElement.sortieTime.SetText(param.sortieTime.ToString() + "(s/f)");
        popupElement.cost.SetText(param.cost.ToString());
        popupElement.hp.SetText(param.hp.ToString());
        popupElement.attackPower.SetText(param.attackPower.ToString());
        popupElement.moveSpeed.SetText(param.moveSpeed.ToString() + "(s/f)");
        popupElement.range.SetText(param.range.ToString());
        popupElement.frequency.SetText(param.frequency.ToString());
        popupElement.description.SetText(param.description);
    }
}
