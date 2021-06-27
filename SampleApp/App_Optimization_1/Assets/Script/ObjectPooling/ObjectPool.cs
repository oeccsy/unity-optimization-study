using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField]
    private GameObject poolingObjectPrefab;

    private Queue<Bullet> poolingObjectQueue = new Queue<Bullet>();
    //pooling�� Object�� �����ϰ� ���� Queue

    private void Awake()
    {
        Instance = this;
        Initialize(10);
    }

    private Bullet CreateNewObject() //������Ʈ�� ���� ���� �� ����ϴ� �Լ�
    {
        var newObj = Instantiate(poolingObjectPrefab, gameObject.transform).GetComponent<Bullet>();
        newObj.gameObject.SetActive(false);
        return newObj;
    }

    private void Initialize(int count) //�̹� ������� ������Ʈ�� Ǯ���Ҷ� ����ϴ� �Լ�
    {
        for(int i=0; i<count; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());
        }
    }

    public static Bullet GetObject()
    {
        if(Instance.poolingObjectQueue.Count>0) //������ ������Ʈ�� ���� ��
        {
            var obj = Instance.poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject(); //������ ������Ʈ�� ������ : �� ������Ʈ ����
            newObj.transform.SetParent(null);
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }

    public static void ReturnObject(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue.Enqueue(bullet);
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
