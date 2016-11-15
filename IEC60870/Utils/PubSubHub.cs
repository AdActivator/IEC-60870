﻿using System;
using System.Collections.Generic;

namespace IEC60870.Utils
{
    public class PubSubHub
    {
        internal class Handler
        {
            public Delegate Action { get; set; }
            public WeakReference Sender { get; set; }
            public Type Type { get; set; }
        }

        internal object locker = new object();
        internal Dictionary<string, List<Handler>> handlers = new Dictionary<string, List<Handler>>();

        public void Publish<T>(object sender, string topic, T data)
        {
            if (handlers.ContainsKey(topic))
            {
                var listHandler = handlers[topic];
                listHandler.ForEach(handler =>
                {
                    if (handler.Sender.IsAlive)
                    {
                        ((Action<T>)handler.Action)(data);
                    }
                    else
                    {
                        lock (locker)
                        {
                            handlers[topic].Remove(handler);
                        }
                    }
                });
            }
        }

        public void Subscribe<T>(object sender, string topic, Action<T> handler)
        {
            var item = new Handler
            {
                Action = handler,
                Sender = new WeakReference(sender),
                Type = typeof(T)
            };

            if (handlers.ContainsKey(topic))
            {
                lock (locker)
                {
                    handlers[topic].Add(item);
                }
            }
            else
            {
                lock (locker)
                {
                    handlers[topic] = new List<Handler>();
                    handlers[topic].Add(item);
                }
            }
        }
    }
}
