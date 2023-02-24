using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstantiateSystem : MonoBehaviour{

    [Tooltip("インスタンス")]
    public static EnemyInstantiateSystem instance = null;

    [Header("敵リスト")]
    public List<Enemys> enemyList = new List<Enemys>((int)ENEMY_ID.MAX);

    [Header("スポーン先の親")]
    public GameObject enemySpawnParent;

    [Header("敵の生成を開始する")]
    private bool EnemyInstantiateStartFlag = false;

#region クラス系

    [System.Serializable]
    public class Enemys{
        [Tooltip("パス")]
        public string path = "";

        [HideInInspector]
        [Tooltip("Prefab保存変数")]
        public GameObject prefab = null;

        [Tooltip("発生頻度")]
        public float time = 0f;

        [HideInInspector]
        [Tooltip("生成位置")]
        public Vector3 instantiatePosition = Vector3.zero;

        [Tooltip("ID")]
        public ENEMY_ID id = ENEMY_ID.MAX;
    }

    public enum ENEMY_ID{
        CRANE,
        CRYSTAL,
        SNOW_RABBIT,
        MAX
    }

    #endregion

    private void Awake() {

        if(!instance){
            instance = this;
        }
        
        enemyList.ForEach((prop) =>{
            prop.prefab = (GameObject)Resources.Load(prop.path);

            if(!prop.prefab){
                Debug.LogWarning(prop.prefab.ToString() + "のロードに失敗しました。");
            }
        });

        EnemyInstantiateStartFlag = false;
    }

    public void EnemyInstantiateStart(){
        EnemyInstantiateStartFlag = true;
        enemyList.ForEach((prop) => 
            StartCoroutine(InstantiateCol(prop.id)));
    }

    public void EnemyInstantiateStop(){
        EnemyInstantiateStartFlag = false;
        enemyList.ForEach((prop) => 
            StopCoroutine(InstantiateCol(prop.id)));
    }

    private IEnumerator InstantiateCol(ENEMY_ID id){

        while(EnemyInstantiateStartFlag){

            var enemy = enemyList[(int)id];

            yield return new WaitForSeconds(enemy.time);

            float y = Random.Range(-6f, 6f);
            enemy.instantiatePosition = new Vector3(-22f, y, 0f); 
            Instantiate(enemy.prefab, enemy.instantiatePosition, Quaternion.identity, enemySpawnParent.transform);
        }
    }
}
