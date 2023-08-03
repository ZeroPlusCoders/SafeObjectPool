using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SafeObjectPool
{
    public interface IPolicy<T>
    {

        /// <summary>
        /// Name of the policy
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Max Capacity
        /// </summary>
        int PoolSize { get; set; }

        /// <summary>
        /// Default fetch timeout
        /// </summary>
        TimeSpan SyncGetTimeout { get; set; }

        /// <summary>
        /// Idle time, if it is exceeded when getting a value, it will be recreated
        /// </summary>
        TimeSpan IdleTimeout { get; set; }

        /// <summary>
        /// This is the queue size for async retrieval requests, less than or equal to 0 means no async enabled
        /// </summary>
        int AsyncGetCapacity { get; set; }

        /// <summary>
        /// Whether to throw an exception after getting a timeout
        /// </summary>
        bool IsThrowGetTimeoutException { get; set; }

        /// <summary>
        /// Whether to auto dispose when the parent app exits
        /// </summary>
        bool IsAutoDisposeWithSystem { get; set; }

        /// <summary>
        /// Interval in seconds for background task to check availability
        /// </summary>
        int CheckAvailableInterval { get; set; }

        /// <summary>
        /// Called when an object is created for the pool
        /// </summary>
        /// <returns>The created object</returns>
        T OnCreate();

        /// <summary>
        /// Destructor
        /// </summary>
        /// <param name="obj"></param>
        void OnDestroy(T obj);

        /// <summary>
        /// Triggered when obtaining an object from the object pool times out
        /// </summary>
        void OnGetTimeout();

        /// <summary>
        /// Triggered when the object is successfully obtained from the object pool, the object is initialized by this method
        /// </summary>
        /// <param name="obj"></param>
        void OnGet(Object<T> obj);
#if net40
#else
        /// <summary>
        /// Triggered when the object is successfully obtained from the object pool, the object is initialized by this method
        /// </summary>
        /// <param name="obj"></param>
        Task OnGetAsync(Object<T> obj);
#endif

        /// <summary>
        /// Triggered when the object is returned to the object pool
        /// </summary>
        /// <param name="obj"></param>
        void OnReturn(Object<T> obj);

        /// <summary>
        /// check availability
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool OnCheckAvailable(Object<T> obj);

        /// <summary>
        /// Event: fires when available
        /// </summary>
        void OnAvailable();

        /// <summary>
        /// Event: fires when unavailable
        /// </summary>
        void OnUnavailable();
    }
}