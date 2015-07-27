// <copyright file="dialogueasset.cs" company="RAGE">
// Copyright (c) 2015 RAGE. All rights reserved.
// </copyright>
// <author>Veg</author>
// <date>10-4-2015</date>
// <summary>Implements the DialogueAsset class</summary>
namespace asset_proof_of_concept_demo_CSharp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// A dialogue asset.
    /// </summary>
    public class DialogueAsset : BaseAsset
    {
        #region Fields

        /// <summary>
        /// The dialogues.
        /// </summary>
        List<Dialogue> Dialogues = new List<Dialogue>();

        /// <summary>
        /// The logger.
        /// </summary>
        private Logger logger;

        /// <summary>
        /// The states.
        /// </summary>
        List<State> States = new List<State>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the asset_proof_of_concept_demo_CSharp.Logger class.
        /// </summary>
        public DialogueAsset()
            : base()
        {
            logger = AssetManager.Instance.findAssetByClass("Logger") as Logger;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Interacts.
        /// </summary>
        ///
        /// <param name="actor">    The actor. </param>
        /// <param name="player">   The player. </param>
        /// <param name="response"> The response. </param>
        ///
        /// <returns>
        /// A Dialogue.
        /// </returns>
        public Dialogue interact(String actor, String player, Int32 response)
        {
            return interact(actor, player, response.ToString());
        }

        /// <summary>
        /// Interacts.
        /// </summary>
        ///
        /// <param name="actor">  The actor. </param>
        /// <param name="player"> The player. </param>
        ///
        /// <returns>
        /// A Dialogue.
        /// </returns>
        public Dialogue interact(String actor, String player)
        {
            return interact(actor, player, null);
        }

        /// <summary>
        /// Interacts.
        /// </summary>
        ///
        /// <param name="actor">    The actor. </param>
        /// <param name="player">   The player. </param>
        /// <param name="response"> The response. </param>
        ///
        /// <returns>
        /// A Dialogue.
        /// </returns>
        public Dialogue interact(String actor, String player, String response)
        {
            Int32 state = FindStateIndex(actor, player);
            //Int32 state = index != -1 ? States[index].state : 0;

            Dialogue dialogue;

            // Dialogue responseDialog = Dialogues.First(p => p.actor.Equals(actor) && p.id.Equals(response));

            Int32 tmp;
            Boolean numeric = Int32.TryParse(response, out tmp);

            Int32 ndx = Dialogues.FindIndex(p => p.actor.Equals(actor) && p.id.Equals(response));

            if (ndx != -1)
            {
                Dialogue response_dialogue = Dialogues.Find(p => p.actor.Equals(actor) && p.id.Equals(response));

                if (numeric)
                {
                    // If its an integer response, move the dialogue state as this is a
                    // response choice
                    //
                    if (response_dialogue.isResponse)
                    {
                        DoLog("  << {0} was chosen.", response_dialogue.id);
                        state = response_dialogue.next;
                        UpdateState(actor, player, state);
                    }

                    dialogue = Dialogues.First(p => p.actor.Equals(actor) && p.id.Equals(state.ToString()));
                }
                else
                {
                    DoLog("{0} ask {1} about {2}", actor, player, response);

                    //
                    // ... otherwise this was a "what about the [item]" type of choice
                    // so we return the dialogue but don't modify the state
                    //
                    dialogue = response_dialogue;
                }
            }
            else
            {
                dialogue = Dialogues.First(p => p.actor.Equals(actor) && p.id.Equals(state.ToString()));
            }

            //126     // Process responses
            //127     //
            //128     var responses = new Array();
            //129     if (dialogue.responses) {
            //130         for (r in dialogue.responses) {
            //131             var response = Dialogue.__getDialogue(actor, dialogue.responses[r]);
            //132             responses.push({ id: response.id, text: response.text });
            //133         }
            //134     }
            //135

            //136     var dialogue_processed = {};
            //137     dialogue_processed.text = dialogue.text;
            //138     dialogue_processed.responses = responses;
            //139

            //140     //
            //141     // Move the conversation on
            //142     //

            DoLog("{0}. {1}", dialogue.id, dialogue.text);

            if (dialogue.responses != null)
            {
                foreach (Int32 rId in dialogue.responses)
                {
                    Dialogue answer = Dialogues.Find(p => p.id.Equals(rId.ToString()));

                    DoLog("  >> {0}. {1}", answer.id, answer.text);
                }
            }

            if (dialogue.next != -1)
            {
                UpdateState(actor, player, dialogue.next);
            }
            //146

            return dialogue;
        }

        /// <summary>
        /// Loads a script.
        /// </summary>
        ///
        /// <param name="actor"> The actor. </param>
        /// <param name="ns">    The namespace. </param>
        /// <param name="res">   The resource. </param>
        public void LoadScript(String actor, String ns, String res)
        {
            LoadScript(actor, GetEmbeddedResource(ns, res));
        }

        /// <summary>
        /// Loads a script.
        /// </summary>
        ///
        /// <param name="actor">  The actor. </param>
        /// <param name="script"> The script. </param>
        public void LoadScript(String actor, String script)
        {
            String[] lines = script.Split(new Char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            //! Missing is the line 'banana: I hate bananas'.

            foreach (String line in lines)
            {
                Dialogue dialogue = new Dialogue();

                dialogue.actor = actor;
                dialogue.next = -1;

                if (!Char.IsNumber(line.First()))
                {
                    dialogue.id = line.Substring(0, line.IndexOf(':'));
                    dialogue.text = line.Substring(line.IndexOf(':') + 1).Trim();
                }
                else
                {
                    Int32 start = line.IndexOf(' ') + 1;

                    dialogue.id = line.Substring(0, start - 1);
                    dialogue.actor = actor;

                    if (line.IndexOf("->") != -1)
                    {
                        dialogue.text = line.Substring(start, line.IndexOf("->") - start - 1);
                        dialogue.next = Int32.Parse(line.Substring(line.IndexOf("->") + "->".Length + 1));
                    }
                    else if (line.IndexOf("[") != -1 && line.IndexOf("]") != -1)
                    {
                        dialogue.text = line.Substring(start, line.IndexOf("[") - start - 1);

                        String responses = line.Substring(line.IndexOf('[') + 1, line.IndexOf(']') - line.IndexOf('[') - 1);

                        dialogue.responses = new List<Int32>();
                        foreach (String response in responses.Split(','))
                        {
                            dialogue.responses.Add(Int32.Parse(response.Trim()));
                        }
                    }
                }

                Dialogues.Add(dialogue);
            }
        }

        /// <summary>
        /// Executes the log operation.
        /// </summary>
        ///
        /// <param name="msg"> The message. </param>
        private void DoLog(String msg)
        {
            if (logger != null)
            {
                logger.log(msg);
            }
            else
            {
                Console.WriteLine(msg);
            }
        }

        /// <summary>
        /// Executes the log operation.
        /// </summary>
        ///
        /// <param name="msg">  The message. </param>
        /// <param name="args"> A variable-length parameters list containing arguments. </param>
        private void DoLog(String msg, params Object[] args)
        {
            DoLog(String.Format(msg, args));
        }

        /// <summary>
        /// Searches for the first state index.
        /// </summary>
        ///
        /// <param name="actor">  The actor. </param>
        /// <param name="player"> The player. </param>
        ///
        /// <returns>
        /// The found state index.
        /// </returns>
        private Int32 FindStateIndex(String actor, String player)
        {
            Int32 index = States.FindIndex(p => p.actor.Equals(actor) && p.player.Equals(player));

            if (index == -1)
            {
                States.Add(new State()
                {
                    actor = actor,
                    player = player,
                    state = 0
                });
            }

            return (index != -1) ? States[index].state : 0;
        }

        /// <summary>
        /// Updates the state.
        /// </summary>
        ///
        /// <param name="actor">  The actor. </param>
        /// <param name="player"> The player. </param>
        /// <param name="state">  The state. </param>
        private void UpdateState(String actor, String player, Int32 state)
        {
            Int32 index = States.FindIndex(p => p.actor.Equals(actor) && p.player.Equals(player));

            if (index == -1)
            {
                // New State
                States.Add(new State()
                {
                    actor = actor,
                    player = player,
                    state = state
                });
            }
            else
            {
                // Update State
                States[index] = new State()
                {
                    actor = actor,
                    player = player,
                    state = state
                };
            }
        }

        #endregion Methods

        #region Nested Types

        /// <summary>
        /// A dialogue.
        /// </summary>
        public struct Dialogue
        {
            #region Fields

            /// <summary>
            /// The actor.
            /// </summary>
            public String actor;

            /// <summary>
            /// The identifier.
            /// </summary>
            public String id;

            /// <summary>
            /// The next.
            /// </summary>
            public Int32 next;

            /// <summary>
            /// The responses.
            /// </summary>
            public List<Int32> responses;

            /// <summary>
            /// The text.
            /// </summary>
            public String text;

            #endregion Fields

            #region Properties

            /// <summary>
            /// Gets a value indicating whether this object is response.
            /// </summary>
            ///
            /// <value>
            /// true if this object is response, false if not.
            /// </value>
            public Boolean isResponse
            {
                get
                {
                    Int32 tmp;
                    return Int32.TryParse(id, out tmp);
                }
            }

            #endregion Properties
        }

        /// <summary>
        /// A state.
        /// </summary>
        public struct State
        {
            #region Fields

            /// <summary>
            /// The actor.
            /// </summary>
            public String actor;

            /// <summary>
            /// The player.
            /// </summary>
            public String player;

            /// <summary>
            /// The state.
            /// </summary>
            public Int32 state;

            #endregion Fields
        }

        #endregion Nested Types
    }
}