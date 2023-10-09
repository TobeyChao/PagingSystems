using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 订户
/// </summary>

public class Subscriber<T>
{
    /// <summary>
    /// 订阅者的HashCode
    /// </summary>
    private int m_HashCode;

    /// <summary>
    /// 订阅者订阅的所有服务
    /// </summary>
    private Dictionary<string, SubscriptionHandler<T>> m_SubscriptionServiceDic = new Dictionary<string, SubscriptionHandler<T>>();

    public Subscriber(int hashCode)
    {
        m_HashCode = hashCode;
    }

    /// <summary>
    /// 订阅
    /// </summary>
    /// <param name="subscriber"></param>
    /// <param name="subscriptionService">订阅的服务</param>
    /// <param name="handler">收到订阅的操作</param>
    public void Subscribe(string subscriptionService, SubscriptionHandler<T> handler)
    {
        // 如果还没有缓存过此订阅，添加此订阅服务
        if (!m_SubscriptionServiceDic.ContainsKey(subscriptionService))
        {
            m_SubscriptionServiceDic.Add(subscriptionService, null);
        }
        m_SubscriptionServiceDic[subscriptionService] = handler;
    }

    /// <summary>
    /// 取消订阅
    /// </summary>
    /// <param name="subscriptionService">订阅的服务</param>
    /// <param name="handler">收到订阅的操作</param>
    public void CancelSubscription(string subscriptionService)
    {
        if (m_SubscriptionServiceDic.ContainsKey(subscriptionService))
        {
            m_SubscriptionServiceDic[subscriptionService] = null;
            m_SubscriptionServiceDic.Remove(subscriptionService);
        }
    }

    /// <summary>
    /// 收到了订阅服务
    /// </summary>
    /// <param name="subscriptionService">订阅服务</param>
    /// <param name="param">内容</param>
    public void Receive(string subscriptionService, T param)
    {
        if (m_SubscriptionServiceDic.TryGetValue(subscriptionService, out SubscriptionHandler<T> handler))
        {
            handler?.Invoke(param);
        }
    }

    /// <summary>
    /// 或缺所有订阅
    /// </summary>
    /// <returns></returns>
    public List<string> GetAllSubscription()
    {
        return m_SubscriptionServiceDic.Keys.ToList();
    }
}