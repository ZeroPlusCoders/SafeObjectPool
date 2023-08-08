using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SafeObjectPool
{

    public class Object<T> : IDisposable
    {
        public static Object<T> InitWith(IObjectPool<T> pool, int id, T value)
        {
            return new Object<T>
            {
                Pool = pool,
                Id = id,
                Value = value,
                LastGetThreadId = Thread.CurrentThread.ManagedThreadId,
                LastGetTime = DateTime.Now
            };
        }

        /// <summary>
        /// Pool that owns this object
        /// </summary>
        public IObjectPool<T> Pool { get; internal set; }

        /// <summary>
        /// unique identifier in the object pool
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// The object we're pooling
        /// </summary>
        public T Value { get; internal set; }

        internal long _getTimes;

        /// <summary>
        /// Total number of fetches
        /// </summary>
        public long GetTimes => _getTimes;

        /// Last time fetched
        public DateTime LastGetTime { get; internal set; }

        /// <summary>
        /// Last time returned
        /// </summary>
        public DateTime LastReturnTime { get; internal set; }

        /// <summary>
        /// Creation time
        /// </summary>
        public DateTime CreateTime { get; internal set; } = DateTime.Now;

        /// <summary>
        /// The thread ID when last fetched
        /// </summary>
        public int LastGetThreadId { get; internal set; }

        /// <summary>
        /// The thread ID when last returned
        /// </summary>
        public int LastReturnThreadId { get; internal set; }

        public override string ToString()
        {
            return $"{this.Value}, Times: {this.GetTimes}, ThreadId(R/G): {this.LastReturnThreadId}/{this.LastGetThreadId}, Time(R/G): {this.LastReturnTime.ToString("yyyy-MM-dd HH:mm:ss:ms")}/{this.LastGetTime.ToString("yyyy-MM-dd HH:mm:ss:ms")}";
        }

        /// <summary>
        /// Reset value - destroys and recreates the object
        /// </summary>
        public void ResetValue()
        {
            if (this.Value != null)
            {
                try { this.Pool.Policy.OnDestroy(this.Value); } catch { }
                try { (this.Value as IDisposable)?.Dispose(); } catch { }
            }
            T value = default(T);
            try { value = this.Pool.Policy.OnCreate(); } catch { }
            this.Value = value;
            this.LastReturnTime = DateTime.Now;
        }

        internal bool _isReturned = false;
        
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Pool?.Return(this);
        }
    }
}