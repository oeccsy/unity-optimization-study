# [Object Pooling]

������Ʈ�� ������ �ı��� ���ſ� �۾��̴�.  

Ư�� �ı��� ������Ʈ�� ���� ���  

Garbage Collection���� ���� ������ ����� ġ�����̴�.  

���� �̸� �����ϱ� ���� ������Ʈ Ǯ���̶�� ����� ����Ѵ�.  

## [Object Pooling ����]

1. ������Ʈ�� ������ ������Ʈ Ǯ�� �����.
1. ������Ʈ�� �ʿ��ϸ� ������Ʈ Ǯ���� ������ ����Ѵ�.
    * ������Ʈ ������ Ǯ�� ������Ʈ�� ������ �ʿ��� ��쿡 �����Ѵ�.
1. ������Ʈ ����� ������ �ٷ� �ı����� �ʰ� ������Ʈ Ǯ�� �ݳ��Ѵ�.
    * ������Ʈ �ı��� �ش� ������Ʈ�� ������ �ʿ� �������� �Ǹ� �� �� �ı��Ѵ�.  
      
<br>
�̷��� ������Ʈ�� ��Ȱ���Ͽ� ������ �ı��� �ּ�ȭ �Ѵ�.

---  

1. ������Ʈ�� ������ ������Ʈ Ǯ�� �����.
```C#
    //ObjectPool.cs

    public static ObjectPool Instance;

    private Queue<Bullet> poolingObjectQueue = new Queue<Bullet>();
    private GameObject poolingObjectPrefab;

    private void Initialize(int count) //�̹� ������� ������Ʈ�� Ǯ���Ҷ� ����ϴ� �Լ�
    {
        for(int i=0; i<count; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());
        }
    }

    private void Awake()
    {
        Instance = this;
        Initialize(10);
    }
```
* Bullet�� �ν��Ͻ����� ť�� �̿��Ͽ� �����ϵ��� �Ѵ�.
* ObjectPool�� �ν��Ͻ��� Instance ��� static ������ �����Ͽ� �̿��Ѵ�.

2. ������Ʈ�� �ʿ��ϸ� ������Ʈ Ǯ���� ������ ����Ѵ�.
    * ������Ʈ ������ Ǯ�� ������Ʈ�� ������ �ʿ��� ��쿡 �����Ѵ�.
```C#
    //ObjectPool.cs

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

    private Bullet CreateNewObject() //������Ʈ�� ���� ���� �� ����ϴ� �Լ�
    {
        var newObj = Instantiate(poolingObjectPrefab, gameObject.transform).GetComponent<Bullet>();
        newObj.gameObject.SetActive(false);
        return newObj;
    }
```
* ���⼭ Instance�� ObjectPool�� �ν��Ͻ��� static �����̴�.
* �ٸ� ��ũ��Ʈ���� Bullet�� �ν��Ͻ��� �����ϰ� ���� ��� �ش� static �Լ��� ȣ���ϵ��� �Ѵ�.


3. ������Ʈ ����� ������ �ٷ� �ı����� �ʰ� ������Ʈ Ǯ�� �ݳ��Ѵ�.

```C#
    //ObjectPool.cs
    public static void ReturnObject(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue.Enqueue(bullet);
    }
```
* static �Լ� �̱� ������ transform�� �ƴ� Instance.transform�� ���ڷ� �����Ѵ�.  
  ���⼭ Instance�� ObjectPool�� �ν��Ͻ��� static �����̴�.
* �ٸ� ��ũ��Ʈ���� Bullet�� �ν��Ͻ��� �����ϰ� ���� ��� �ش� statid�Լ��� ȣ���Ͽ� ��ȯ�ϵ��� �Ѵ�.

### [���� �м�]
Window -> Analysis -> Profiler �׸񿡼� ���ɺм��� Ȯ���� �� �ִ�.

![Image](../SampleApp/App_Optimization_1/Picture/Profiler1.png)

<br>

������Ʈ�� ������ �ı��۾��� �����ϸ�

�߰��� �������� Ƣ�� �κ��� ����µ�, GarbageCollector�� ������ ���� ������ Ȯ���� �� �ִ�.

![Image2](../SampleApp/App_Optimization_1/Picture/Profiler2.png)

<br>

�̶� �ִ�� ����� �� �ִ� Bullet�� �ν��Ͻ����� ������ ��  
������Ʈ Ǯ���� �̿��Ͽ� Bullet�� �ν��Ͻ��� �����ϸ� GarbageCollector �κп� ���ǹ��� ��ȭ�� Ȯ���� �� �ִ�.
* GarbageCollector.CollectIncremental (8.00ms) -> (3.41ms)
* GC.Collect (2.97ms) -> (1.40ms)

![Image3](../SampleApp/App_Optimization_1/Picture/Profiler3.png)
![Image3](../SampleApp/App_Optimization_1/Picture/Profiler4.png)


### [���� ��ũ]

* https://www.youtube.com/watch?v=xiojw0lHzro&t=437s
