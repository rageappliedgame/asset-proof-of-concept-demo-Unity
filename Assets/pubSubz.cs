// <copyright file="pubSubz.cs" company="RAGE">
// Copyright (c) 2015 RAGE. All rights reserved.
// </copyright>
// <author>Veg</author>
// <date>10-4-2015</date>
// <summary>Implements the pub subz class</summary>
namespace asset_proof_of_concept_demo_CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class pubsubz
    {
        #region Fields

        private static Int32 subUid = 0;
        private static Dictionary<String, Dictionary<String, TopicEvent>> topics = new Dictionary<String, Dictionary<String, TopicEvent>>();

        #endregion Fields

        #region Delegates

        public delegate void TopicEvent(String topic, params object[] args);

        #endregion Delegates

        #region Methods

        public static Boolean define(String topic)
        {
            if (!topics.Keys.Contains(topic))
            {
                topics.Add(topic, new Dictionary<String, TopicEvent>());

                return true;
            }

            return false;
        }

        public static Boolean publish(String topic, params object[] args)
        {
            if (!topics.Keys.Contains(topic))
            {
                return false;
            }

            foreach (KeyValuePair<String, TopicEvent> func in topics[topic])
            {
                func.Value(topic, args);
            }

            return true;
        }

        public static String subscribe(String topic, TopicEvent func)
        {
            if (!topics.Keys.Contains(topic))
            {
                topics.Add(topic, new Dictionary<String, TopicEvent>());
            }

            String token = (++subUid).ToString();

            topics[topic].Add(token, func);

            return token;
        }

        public static Boolean unsubscribe(String token)
        {
            foreach (String topic in topics.Keys)
            {
                Dictionary<String, TopicEvent> subscribers = topics[topic];

                foreach (String subscriber in subscribers.Keys)
                {
                    if (token.Equals(subscriber))
                    {
                        subscribers.Remove(subscriber);

                        return true;
                    }
                }
            }

            return false;
        }

        #endregion Methods
    }
}