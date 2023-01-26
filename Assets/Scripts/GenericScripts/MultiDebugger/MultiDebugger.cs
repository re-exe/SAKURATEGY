using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiDebugger : MonoBehaviour{

    [Tooltip("入力")]
    private PlayerInputScript m_input = null;

    [Tooltip("インスタンス")]
    private static MultiDebugger instance = null;

    [Tooltip("子のオブジェクト")]
    private GameObject m_childrenObject = null;

    private void Start() {
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }

        // 色々取得
        m_input = new PlayerInputScript();
        m_input.Enable();
        m_childrenObject = this.transform.GetChild(0).gameObject;

        if(m_childrenObject == null){
            m_childrenObject = this.transform.Find("DebugCanvas").gameObject;
        }

        // 初期は非表示
        m_childrenObject.SetActive(false);

        m_input.UI.Debug.performed += _ => {
            m_childrenObject.SetActive(!m_childrenObject.activeSelf);
        };
    }
}
