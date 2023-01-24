using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Debugger{
    public class DeviceInfo : MonoBehaviour{

        [SerializeField]
        [Tooltip("テキストリスト")]
        private List<TextMeshProUGUI> textList = new List<TextMeshProUGUI>();

        private void Awake() {

            textList[0].SetText("OS : " + SystemInfo.operatingSystem);
            textList[1].SetText("CPU : " + SystemInfo.processorType + "/" + SystemInfo.processorCount + "cores");
            textList[2].SetText("GPU : " + SystemInfo.graphicsDeviceName + "/" + SystemInfo.graphicsMemorySize + "MB" + "/" + SystemInfo.graphicsDeviceType);
            textList[3].SetText("RAM : " + SystemInfo.systemMemorySize + ".0MB" + "/" + "GC : " + System.GC.CollectionCount(0));
            textList[4].SetText("Resolution : " + Screen.currentResolution.width + " x " + Screen.currentResolution.height + "/" + "RefreshRate : " + Screen.currentResolution.refreshRate + "Hz");
        }
    }
}

