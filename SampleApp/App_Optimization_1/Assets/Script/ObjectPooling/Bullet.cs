using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;

    public void Shoot(Vector3 dir)
    {
        direction = dir;
        Invoke("DestroyBullet", 5f);
        //Destroy(gameObject, 5f); ��� ����Ѵ�. 5�� �� �ı� ��� pool�� ��ȯ�Ѵ�.
    }
    // Start is called before the first frame update
    private void DestroyBullet()
    {
        ObjectPool.ReturnObject(this);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction);
    }
}
