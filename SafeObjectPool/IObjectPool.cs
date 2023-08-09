using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SafeObjectPool
{
    public interface IObjectPool<T> : IDisposable
    {
        IPolicy<T> Policy { get; }

        /// <summary>
        /// Is it available?
        /// </summary>
        bool IsAvailable { get; }

        /// <summary>
        /// Unavailable Exception
        /// </summary>
        Exception UnavailableException { get; }

        /// <summary>
        /// Unavailable Time
        /// </summary>
        DateTime? UnavailableTime { get; }

        /// <summary>
        /// Set the object pool as unavailable, subsequent Get/GetAsync will report an error, 
        /// and start the background timing check service to restore availability
        /// </summary>
        /// <param name="exception"></param>
        /// <returns>Return true when it changes from [Available] to [Unavailable]，otherwise returns false</returns>
        bool SetUnavailable(Exception exception);

        /// <summary>
        /// Stats on Objects in the object pool
        /// </summary>
        string Statistics { get; }

        /// <summary>
        /// Statistics on objects in the object pool (full)
        /// </summary>
        string StatisticsFull { get; }

        /// <summary>
        /// Get an object from the pool
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Object<T> Get(TimeSpan? timeout = null);

        /// <summary>
        /// Number of objects in the pool
        /// </summary>
        int AllCount { get; }

        /// <summary>
        /// Number of objects returned to the pool
        /// </summary>
        int FreeCount { get; }

        /// <summary>
        /// Maximum number of objects in the pool
        /// </summary>
        int MaxCount { get; }

#if net40
#else
        /// <summary>
        /// Get an object from the pool asynchronously
        /// </summary>
        /// <returns></returns>
        Task<Object<T>> GetAsync();
#endif

        /// <summary>
        /// Return object to the pool
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isReset">whether to recreate or just put in the pool</param>
        void Return(Object<T> obj, bool isReset = false);
    }
}
