using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DescriptionManager : MonoBehaviour{

    [Tooltip("インスタンス")]
    public static DescriptionManager instance = null;

    [Tooltip("情報リスト")]
    private List<CharacterParamAsset.CharacterParam> m_paramList = new List<CharacterParamAsset.CharacterParam>();

    [SerializeField]
    [Tooltip("表示するオブジェクト")]
    private GameObject boxObject = null;

    [SerializeField]
    [Tooltip("表示するテキスト")]
    private TextMeshProUGUI text = null;

    private void Awake() {
        if(!instance){
            instance = this;
        }

        CharacterParamAsset characterParam = Resources.Load("CharacterParamAsset") as CharacterParamAsset;

        m_paramList = characterParam.characterParamList;

        boxObject.SetActive(false);
        text.SetText("");
    } 

    public void OpenDescription(int index){

        var param = m_paramList[index];
        text.SetText(param.description);
        boxObject.SetActive(true);

    }

    public void CloseDescription(){
        boxObject.SetActive(false);
        text.SetText("");
    }
}
