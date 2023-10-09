using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MsgTest : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;

    // Start is called before the first frame update
    void Awake()
    {
        button1.onClick.AddListener(OnClickButton1);
        button2.onClick.AddListener(OnClickButton2);
        button3.onClick.AddListener(OnClickButton3);
        button4.onClick.AddListener(OnClickButton4);

        PagingSystems<string>.Instance.Subscribe(GetHashCode(), "button1", (p) =>
        {
            Debug.Log(p);
        });

        PagingSystems<List<string>>.Instance.Subscribe(GetHashCode(), "button2", (p) =>
        {
            var list = p;
            for (int i = 0; i < list.Count; i++)
            {
                Debug.Log(list[i]);
            }
        });
    }

    private void OnClickButton4()
    {
        PagingSystems<string>.Instance.CancelSubscription(GetHashCode(), "button1");
    }

    private void OnClickButton3()
    {
        PagingSystems<string>.Instance.CancelAllSubscriptionsBySubscriber(GetHashCode());
        PagingSystems<List<string>>.Instance.CancelAllSubscriptionsBySubscriber(GetHashCode());
    }

    private void OnClickButton2()
    {
        PagingSystems<List<string>>.Instance.SendMessages("button2", new List<string>()
        {
            "你点击了按钮2",
            "参数是一个数组",
            "所以打印了这三条"
        });
    }

    private void OnClickButton1()
    {
        PagingSystems<string>.Instance.SendMessages("button1", "你点击了按钮1");
    }
}
