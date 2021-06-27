using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField]
    private GameObject poolingObjectPrefab;

    private Queue<Bullet> poolingObjectQueue = new Queue<Bullet>();
    //pooling될 Object를 저장하고 있을 Queue

    private void Awake()
    {
        Instance = this;
        Initialize(10);
    }

    private Bullet CreateNewObject() //오브젝트를 새로 만들 때 사용하는 함수
    {
        var newObj = Instantiate(poolingObjectPrefab, gameObject.transform).GetComponent<Bullet>();
        newObj.gameObject.SetActive(false);
        return newObj;
    }

    private void Initialize(int count) //이미 만들어진 오브젝트를 풀링할때 사용하는 함수
    {
        for(int i=0; i<count; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());
        }
    }

    public static Bullet GetObject()
    {
        if(Instance.poolingObjectQueue.Count>0) //빌려줄 오브젝트가 있을 때
        {
            var obj = Instance.poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject(); //빌려줄 오브젝트가 없을때 : 새 오브젝트 생성
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
