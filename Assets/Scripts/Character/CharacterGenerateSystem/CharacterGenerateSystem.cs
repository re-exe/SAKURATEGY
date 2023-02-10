using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CharacterGenerateSystem : MonoBehaviour{

    [Tooltip("Input")]
    private PlayerInputScript m_input = null;

    [Header("ボタンリスト")]
    public List<Characters> characterList = new List<Characters>((int)TREE_ID.MAX);

    [Space(10)]
    [Header("情報")]
    [Tooltip("情報リスト")]
    private List<CharacterParamAsset.CharacterParam> m_paramList = new List<CharacterParamAsset.CharacterParam>();

    [SerializeField]
    [Tooltip("表示する枠")]
    private GameObject boxObj = null;

    [SerializeField]
    [Tooltip("表示するテキスト")]
    private TextMeshProUGUI text = null;

#region クラス

    [System.Serializable]
    public class Characters{

        [HideInInspector]
        [Tooltip("Prefab保存変数")]
        public GameObject prefab = null;

        [Tooltip("オブジェクト")]
        public GameObject obj = null;

        [Tooltip("パス")]
        public string path = "";

        [HideInInspector]
        [Tooltip("イベントトリガー")]
        public EventTrigger trigger = null;

        [Tooltip("ID")]
        public TREE_ID id = TREE_ID.MAX;
    }

    [Tooltip("ID")]
    public enum TREE_ID{
        SOMEIYOSHINO,
        KANHIZAKURA,
        KAWAZUSAKURA,
        HIGANZAKURA,
        OYAMAZAKURA,
        MAX
    }

#endregion

    private void OnEnable() { m_input.Enable(); }

    private void OnDisable() { m_input.Disable(); }

    private void Awake() {

        m_input = new PlayerInputScript();

        CharacterParamAsset characterParam = Resources.Load("CharacterParamAsset") as CharacterParamAsset;
        m_paramList = characterParam.characterParamList;
        boxObj.SetActive(false);
        text.SetText("");

        characterList.ForEach((prop) =>{

            prop.trigger = prop.obj.GetComponent<EventTrigger>();

            prop.prefab = (GameObject)Resources.Load(prop.path);

            if(!prop.prefab){
                Debug.LogWarning("Failed to load " + prop.id + ". Please check the path.");
                return;
            }

            // D&Dイベントの登録
            EventTrigger.Entry enter = new EventTrigger.Entry();
            enter.eventID = EventTriggerType.PointerEnter;
            enter.callback.AddListener((e) => PointerEnter(prop.id));
            prop.trigger.triggers.Add(enter);

            EventTrigger.Entry exit = new EventTrigger.Entry();
            exit.eventID = EventTriggerType.PointerExit;
            exit.callback.AddListener((e) => PointerExit());
            prop.trigger.triggers.Add(exit);

            EventTrigger.Entry down = new EventTrigger.Entry();
            down.eventID = EventTriggerType.PointerDown;
            down.callback.AddListener((e) => PointerDown(prop.prefab));
            prop.trigger.triggers.Add(down);

            EventTrigger.Entry up = new EventTrigger.Entry();
            up.eventID = EventTriggerType.PointerUp;
            up.callback.AddListener((e) => PointerUp());
            prop.trigger.triggers.Add(up);
        });
    }

    public void PointerEnter(TREE_ID id){

        var param = m_paramList[(int)id];
        text.SetText(param.description);
        boxObj.SetActive(true);
        
    }

    public void PointerExit(){
        boxObj.SetActive(false);
        text.SetText("");
    }

    public void PointerDown(GameObject obj){
        InstantiateSystem.instance.CharacterInstantiate(obj);
    }

    public void PointerUp(){
        InstantiateSystem.instance.InstantiateNewPosition();
        InstantiateSystem.instance.DestroyCharacterCash();
    }
}
