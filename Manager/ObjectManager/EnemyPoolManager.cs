using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class EnemyPoolManager : MonoBehaviour
{
    [System.Serializable]
    private class ObjectInfo
    {
        // ������Ʈ �̸�
        public string objectName;
        // ������Ʈ Ǯ���� ������ ������Ʈ
        public GameObject prefab;
        // ��� �̸� ���� �س�������
        public int count;
    }


    public static EnemyPoolManager instance;

    // ������ƮǮ �Ŵ��� �غ� �Ϸ�ǥ��
    public bool IsReady { get; private set; }

    [SerializeField]
    private ObjectInfo[] objectInfos = null;

    // ������ ������Ʈ�� key�������� ���� ����
    private string objectName;

    // ������ƮǮ���� ������ ��ųʸ�
    private Dictionary<string, IObjectPool<GameObject>> ojbectPoolDic = new Dictionary<string, IObjectPool<GameObject>>();

    // ������ƮǮ���� ������Ʈ�� ���� �����Ҷ� ����� ��ųʸ�
    private Dictionary<string, GameObject> ObjectDic = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;


        }
        else
            Destroy(this.gameObject);

        Init();
    }


    private void Init()
    {
        IsReady = false;

        for (int idx = 0; idx < objectInfos.Length; idx++)
        {
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
            OnDestroyPoolObject, true, objectInfos[idx].count, objectInfos[idx].count);

            if (ObjectDic.ContainsKey(objectInfos[idx].objectName))
            {
                Debug.LogFormat("{0} �̹� ��ϵ� ������Ʈ�Դϴ�.", objectInfos[idx].objectName);
                return;
            }

            ObjectDic.Add(objectInfos[idx].objectName, objectInfos[idx].prefab);
            ojbectPoolDic.Add(objectInfos[idx].objectName, pool);

            // �̸� ������Ʈ ���� �س���
            for (int i = 0; i < objectInfos[idx].count; i++)
            {
                objectName = objectInfos[idx].objectName;
                PoolAble poolAbleObject = CreatePooledItem().GetComponent<PoolAble>();
                poolAbleObject.Pool.Release(poolAbleObject.gameObject);
            }
        }

        Debug.Log("������ƮǮ�� �غ� �Ϸ�");
        IsReady = true;

    }

    // ����
    private GameObject CreatePooledItem()
    {
        GameObject poolObject = Instantiate(ObjectDic[objectName]);
        poolObject.GetComponent<PoolAble>().Pool = ojbectPoolDic[objectName];

        return poolObject;
    }

    // �뿩
    private void OnTakeFromPool(GameObject poolObject)
    {
        poolObject.SetActive(true);
    }

    // ��ȯ
    private void OnReturnedToPool(GameObject poolObject)
    {
        poolObject.SetActive(false);
    }

    // ����
    private void OnDestroyPoolObject(GameObject poolObject)
    {
        Destroy(poolObject);
    }

    public GameObject GetObject(string objectName)
    {
        this.objectName = objectName;

        if (ObjectDic.ContainsKey(objectName) == false)
        {
            Debug.LogFormat("{0} ������ƮǮ�� ��ϵ��� ���� ������Ʈ�Դϴ�.", objectName);
            return null;
        }

        return ojbectPoolDic[objectName].Get();
    }
}