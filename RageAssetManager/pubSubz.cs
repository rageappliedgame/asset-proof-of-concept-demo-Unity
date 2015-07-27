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

    /// <summary>
    /// A pubsubz.
    /// </summary>
    public static class pubsubz
    {
        #region Fields

        /// <summary>
        /// The sub UID.
        /// </summary>
        private static Int32 subUid = 0;

        /// <summary>
        /// The topics.
        /// </summary>
        private static Dictionary<String, Dictionary<String, TopicEvent>> topics = new Dictionary<String, Dictionary<String, TopicEvent>>();

        #endregion Fields

        #region Delegates

        /// <summary>
        /// Topic event.
        /// </summary>
        ///
        /// <param name="topic"> The topic. </param>
        /// <param name="args">  A variable-length parameters list containing arguments. </param>
        public delegate void TopicEvent(String topic, params object[] args);

        #endregion Delegates

        #region Methods

        /// <summary>
        /// Defines.
        /// </summary>
        ///
        /// <param name="topic"> The topic. </param>
        ///
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public static Boolean define(String topic)
        {
            if (!topics.Keys.Contains(topic))
            {
                topics.Add(topic, new Dictionary<String, TopicEvent>());

                return true;
            }

            return false;
        }

        /// <summary>
        /// Publishes.
        /// </summary>
        ///
        /// <param name="topic"> The topic. </param>
        /// <param name="args">  A variable-length parameters list containing arguments. </param>
        ///
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
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

        /// <summary>
        /// Subscribes.
        /// </summary>
        ///
        /// <param name="topic"> The topic. </param>
        /// <param name="func">  The function. </param>
        ///
        /// <returns>
        /// A String.
        /// </returns>
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

        /// <summary>
        /// Unsubscribes.
        /// </summary>
        ///
        /// <param name="token"> The token. </param>
        ///
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
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