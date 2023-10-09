using System.Collections.Generic;

public delegate void SubscriptionHandler<T>(T param);

/// <summary>
/// 寻呼系统
/// </summary>
public class PagingSystems<T> : TSingleton<PagingSystems<T>>
{
    // 订阅者们
    private Dictionary<int, Subscriber<T>> m_SubscriberDic = new Dictionary<int, Subscriber<T>>();

    // 根据订阅服务为订阅者分类
    private Dictionary<string, HashSet<int>> m_SubscribersByServiceDic = new Dictionary<string, HashSet<int>>();

    /// <summary>
    /// 订阅
    /// </summary>
    /// <param name="subscriber">订阅者哈希值</param>
    /// <param name="subscriptionService">订阅的服务</param>
    /// <param name="handler">收到订阅的操作</param>
    public void Subscribe(int subscriber, string subscriptionService, SubscriptionHandler<T> handler)
    {
        // 增加订阅者
        if (!m_SubscriberDic.ContainsKey(subscriber))
        {
            m_SubscriberDic.Add(subscriber, new Subscriber<T>(subscriber));
        }
        m_SubscriberDic[subscriber].Subscribe(subscriptionService, handler);
        // 增加分类
        if (!m_SubscribersByServiceDic.ContainsKey(subscriptionService))
        {
            m_SubscribersByServiceDic.Add(subscriptionService, new HashSet<int>());
        }
        m_SubscribersByServiceDic[subscriptionService].Add(subscriber);
    }

    /// <summary>
    /// 取消订阅
    /// </summary>
    /// <param name="subscriptionService">订阅的服务</param>
    /// <param name="handler">收到订阅的操作</param>
    public void CancelSubscription(int subscriber, string subscriptionService)
    {
        // 确保订阅者存在
        if (m_SubscriberDic.ContainsKey(subscriber))
        {
            m_SubscriberDic[subscriber].CancelSubscription(subscriptionService);
        }
        // 分类中去除此订阅者
        if (m_SubscribersByServiceDic.ContainsKey(subscriptionService))
        {
            m_SubscribersByServiceDic[subscriptionService].Remove(subscriber);
        }
    }

    /// <summary>
    /// 取消某订阅者所有订阅
    /// </summary>
    public void CancelAllSubscriptionsBySubscriber(int subscriber)
    {
        // 确保订阅者存在
        if (m_SubscriberDic.ContainsKey(subscriber))
        {
            var list = m_SubscriberDic[subscriber].GetAllSubscription();
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    CancelSubscription(subscriber, list[i]);
                }
            }
        }
    }

    /// <summary>
    /// 发送订阅消息
    /// 注意，不要在回调函数中企图取消订阅，会导致
    /// InvalidOperationException: Collection was modified; enumeration operation may not execute.
    /// </summary>
    /// <param name="subscriptionService">服务</param>
    /// <param name="param">消息内容</param>
    public void SendMessages(string subscriptionService, T param)
    {
        if (m_SubscribersByServiceDic.ContainsKey(subscriptionService))
        {
            foreach (var subscriber in m_SubscribersByServiceDic[subscriptionService])
            {
                if (m_SubscriberDic.ContainsKey(subscriber))
                {
                    m_SubscriberDic[subscriber].Receive(subscriptionService, param);
                }
            }
        }
    }
}