﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SafeObjectPool
{

    public class DefaultPolicy<T> : IPolicy<T>
    {

        public string Name { get; set; } = typeof(DefaultPolicy<T>).GetType().FullName;
        public int PoolSize { get; set; } = 0;
        public TimeSpan SyncGetTimeout { get; set; } = TimeSpan.FromSeconds(10);
        public TimeSpan IdleTimeout { get; set; } = TimeSpan.FromSeconds(50);
        public int AsyncGetCapacity { get; set; } = 10000;
        public bool IsThrowGetTimeoutException { get; set; } = true;
        public bool IsAutoDisposeWithSystem { get; set; } = true;
        public int CheckAvailableInterval { get; set; } = 5;

        public Func<T> CreateObject;
        public Action<Object<T>> OnGetObject;


        /// <summary>
        /// Creates the object using the constructor func provided
        /// </summary>
        /// <returns></returns>
        public T OnCreate()
        {
            return CreateObject();
        }

        public void OnDestroy(T obj)
        {

        }

        public void OnGet(Object<T> obj)
        {
            //Console.WriteLine("Get: " + obj);
            OnGetObject?.Invoke(obj);
        }

#if net40
#else
        public Task OnGetAsync(Object<T> obj)
        {
            //Console.WriteLine("GetAsync: " + obj);
            OnGetObject?.Invoke(obj);
            return Task.FromResult(true);
        }
#endif

        public void OnGetTimeout()
        {

        }

        public void OnReturn(Object<T> obj)
        {
            //Console.WriteLine("Return: " + obj);
        }

        public bool OnCheckAvailable(Object<T> obj)
        {
            return true;
        }

        public void OnAvailable()
        {

        }

        public void OnUnavailable()
        {

        }
    }
}