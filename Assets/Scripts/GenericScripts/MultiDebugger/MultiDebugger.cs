using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Debugger{
    public class MultiDebugger : MonoBehaviour{

        [Tooltip("インスタンス")]
        public static MultiDebugger instance = null;

        [Tooltip("入力")]
        private PlayerInputScript m_input = null;
        
        private void OnEnable() { m_input.Enable(); }

        private void OnDisable() { m_input.Disable(); }

        private void Awake() {

            // インスタンス生成
            if(!instance){
                instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
            this.gameObject.SetActive(false);

            m_input = new PlayerInputScript();

            m_input.Debug.DebugWinow.performed += DebuggerWindowSwitch;
        }

        private void DebuggerWindowSwitch(InputAction.CallbackContext context){
            this.gameObject.SetActive(!this.gameObject.activeSelf);
        }
    }
}

