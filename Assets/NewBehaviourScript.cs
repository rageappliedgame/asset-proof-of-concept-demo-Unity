using System;
using System.Collections.Generic;
using System.Linq;
using AssetPackage;
using AssetManagerPackage;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// A new behaviour script.
/// </summary>
public class NewBehaviourScript : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, ISubmitHandler
{
    #region Fields

    /// <summary>
    /// The dialogue text, to be linked with a TextField .
    /// </summary>
    public Text dialogueText;

    /// <summary>
    /// The first asset.
    /// </summary>
    static Asset asset1;

    /// <summary>
    /// The second asset.
    /// </summary>
    static Asset asset2;

    /// <summary>
    /// The third asset.
    /// </summary>
    static Logger asset3;

    /// <summary>
    /// The fourth asset.
    /// </summary>
    static Logger asset4;

    /// <summary>
    /// The fifth asset.
    /// </summary>
    static DialogueAsset asset5;

    /// <summary>
    /// The first bridge.
    /// </summary>
    static Bridge bridge1;

    /// <summary>
    /// The second bridge.
    /// </summary>
    static Bridge bridge2;

    #endregion Fields

    #region Methods

    /// <summary>
    /// Arguments to String.
    /// </summary>
    ///
    /// <param name="args"> A variable-length parameters list containing arguments. </param>
    ///
    /// <returns>
    /// A String.
    /// </returns>
    public static String ArgsToString(params object[] args)
    {
        if (args == null || args.Length == 0)
        {
            return String.Empty;
        }
        else
        {
            return String.Join(";", args.Select(p => p.ToString()).ToArray());
        }
    }

    /// <summary>
    /// Handler, called when my event.
    /// </summary>
    ///
    /// <remarks>
    /// NOTE: Only static because the console programs Main is static too.
    /// </remarks>
    ///
    /// <param name="topic"> The topic. </param>
    /// <param name="args">  A variable-length parameters list containing arguments. </param>
    public static void MyEventHandler(String topic, params object[] args)
    {
        Debug.Log(String.Format("[demo.html].{0}: [{1}]", topic, ArgsToString(args)));
    }

    /// <summary>
    /// Executes the click action.
    /// </summary>
    public void OnClick()
    {
        Debug.Log("OnClick!");
    }

    /// <summary>
    /// Executes the mouse down action.
    /// </summary>
    public void OnMouseDown()
    {
        Debug.Log("OnMouseDown!");
    }

    /// <summary>
    /// Executes the mouse enter action.
    /// </summary>
    public void OnMouseEnter()
    {
        Debug.Log("OnMouseEnter!");
    }

    /// <summary>
    /// Executes the mouse leave action.
    /// </summary>
    public void OnMouseLeave()
    {
        Debug.Log("OnMouseLeave!");
    }

    /// <summary>
    /// Executes the pointer click action.
    /// </summary>
    ///
    /// <param name="eventData"> Information describing the event. </param>
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick!");

        Test_01_Setup();

        Test_02_VersionAndDependenciesReport();

        Test_03_AssetToAssetAndBridge();

        Test_04_DataStorageAndArchive();

        Test_05_EventSubscription();

        Test_06_SanityChecks();

        Test_07_DialogueAsset(this);

        Test_08_Settings(this);
    }

    /// <summary>
    /// Executes the pointer down action.
    /// </summary>
    ///
    /// <param name="eventData"> Information describing the event. </param>
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown!");
    }

    /// <summary>
    /// Called when Enter Key is pressed!
    /// </summary>
    /// <param name="eventDat">Event dat.</param>
    public void OnSubmit(BaseEventData eventDat)
    {
        Debug.Log("OnSubmit!");
    }

    /// <summary>
    /// Tests 01 setup.
    /// </summary>
    private static void Test_01_Setup()
    {
        if (bridge1 == null)
        {
            bridge1 = new Bridge("global bridge: ");
            Debug.Log("Bridge1 Created");
        }

        if (bridge2 == null)
        {
            bridge2 = new Bridge();
            Debug.Log("Bridge2 Created");
        }

        //! Add assets and automatically create the Asset Manager.
        //
        asset1 = new Asset();
        asset2 = new Asset();
        asset3 = new Logger();
        asset4 = new Logger();
        asset5 = new DialogueAsset();

        bridge2.Prefix = "private bridge: ";

        // For Unity3D we need a bridge as Console.WriteLine is not supported and we have to use Debug.log() instead!
        asset3.Bridge = bridge2;

        //! Unity3D - These will not log until a bridge is attached.
        //! Default behavior is Console.WriteLine that compiles, but does not show up in the Console Window.
        asset3.log("Asset1: " + asset1.Class + ", " + asset1.Id);
        asset3.log("Asset2: " + asset2.Class + ", " + asset2.Id);
        asset3.log("Asset3: " + asset3.Class + ", " + asset3.Id);
        asset3.log("Asset4: " + asset4.Class + ", " + asset4.Id);
        asset3.log("Asset5: " + asset5.Class + ", " + asset5.Id);
    }

    /// <summary>
    /// Tests 02 version and dependencies report.
    /// </summary>
    private static void Test_02_VersionAndDependenciesReport()
    {
        //! See https://msdn.microsoft.com/en-us/library/system.version(v=vs.110).aspx

        //! major.minor[.build[.revision]]
        //!
        //! The components are used by convention as follows:
        //!
        //! Major:    Assemblies with the same name but different major versions are not interchangeable.
        //!           A higher version number might indicate a major rewrite of a product where backward
        //!           compatibility cannot be assumed.
        //! Minor:    If the name and major version number on two assemblies are the same,
        //!           but the minor version number is different, this indicates significant enhancement with
        //!           the intention of backward compatibility.
        //!           This higher minor version number might indicate a point
        //!           release of a product or a fully backward-compatible new version of a product.
        //! Build:    A difference in build number represents a recompilation of the same source.
        //!           Different build numbers might be used when the processor, platform, or compiler changes.
        //! Revision: Assemblies with the same name, major, and minor version numbers but different revisions
        //!           are intended to be fully interchangeable. A higher revision number might be used in a
        //!           build that fixes a security hole in a previously released assembly.
        //!
        //! For two versions to be equal, the major, minor, build, and revision numbers of the first Version
        //! object must be identical to those of the second Version object. If the build or revision number
        //! of a Version object is undefined, that Version object is considered to be earlier than a Version
        //! object whose build or revision number is equal to zero. The following example illustrates this
        //! by comparing three Version objects that have undefined version components.

        // 12.0.0.0
        // Major.Minor.Build.Revision.
        //Console.WriteLine("{0}", Assembly.GetCallingAssembly().GetName().Version);

        // CLR Version 4.0.30319.34209
        //Version ver = Environment.Version;
        //Debug.Print("CLR Version {0}", ver.ToString());

        //XDocument versionXml = asset1.VersionAndDependencies();
        //Console.WriteLine(versionXml.ToString());

        //IEnumerable<XElement> dependencies = versionXml.XPathSelectElements("version/dependencies/depends");
        //foreach (XElement dependency in dependencies)
        //{
        //    Console.WriteLine("Depends {0}", dependency.Value);
        //}
        Debug.Log(String.Empty);
        Debug.Log(String.Format("Asset {0} v{1}", asset1.Class, asset1.Version));
        foreach (KeyValuePair<String, String> dependency in asset1.Dependencies)
        {
            Debug.Log(String.Format("Depends on {0} v{1}", dependency.Key, dependency.Value));
        }
        Debug.Log(String.Empty);

        Debug.Log(AssetManager.Instance.VersionAndDependenciesReport);

        Debug.Log(String.Format("Version: v{0}", asset1.Version));

        Debug.Log(String.Empty);
    }

    /// <summary>
    /// Tests 03 asset to asset and bridge.
    /// </summary>
    private static void Test_03_AssetToAssetAndBridge()
    {
        // Use the new Logger directly.
        //
        asset3.log("LogByLogger: " + asset3.Class + ", " + asset3.Id);

        // Test if asset1 can find the Logger (asset3) thru the AssetManager.
        //
        asset1.publicMethod("Hello World (console.log)");

        //! TODO Implement replacing method behavior.
        //

        // Replace the both Logger's log method by a native version supplied by the Game Engine.
        //
        AssetManager.Instance.Bridge = bridge1;

        // Check the results for both Loggers are alike.
        //
        asset1.publicMethod("Hello Different World (Game Engine Logging)");

        // Replace the 1st Logger's log method by a native version supplied by the Game Engine.
        //
        asset2.Bridge = bridge2;

        // Check the results for both Loggers differ (one message goes to the console, the other shows as an alert).
        //
        asset1.publicMethod("Hello Different World (Game Engine Logging)");
    }

    /// <summary>
    /// Tests 04 data storage and archive.
    /// </summary>
    private static void Test_04_DataStorageAndArchive()
    {
        asset3.log("----[assetmanager.bridge]-----");
        asset2.doStore();   // Create Hello1.txt and Hello2.txt
        foreach (String fn in asset2.doList()) // List
        {
            asset3.log(String.Format("{0}={1}", fn, asset2.doLoad(fn)));
        }
        asset2.doRemove();  // Remove Hello1.txt

        foreach (String fn in asset2.doList()) // List
        {
            asset3.log(String.Format("{0}={1}", fn, asset2.doLoad(fn)));
        }
        asset2.doArchive(); // Move Hello2.txt

        asset3.log("----[default]-----");

        //! Reset/Remove Both Bridges.
        //
        asset2.Bridge = null;

        AssetManager.Instance.Bridge = null;

        foreach (String fn in asset2.doList()) // List
        {
            asset3.log(String.Format("{0}={1}", fn, asset2.doLoad(fn)));
        }
        asset2.doStore();

        asset3.log("----[private.bridge]-----");

        asset2.Bridge = bridge2;

        asset2.doStore();

        foreach (String fn in asset2.doList()) // List
        {
            asset3.log(String.Format("{0}={1}", fn, asset2.doLoad(fn)));
        }

        asset3.log("----[default]-----");

        asset2.Bridge = null;

        foreach (String fn in asset2.doList()) // List
        {
            asset3.log(String.Format("{0}={1}", fn, asset2.doLoad(fn)));
        }
    }

    /// <summary>
    /// Tests 05 event subscription.
    /// </summary>
    private static void Test_05_EventSubscription()
    {
        //! Event Subscription.
        //
        // Define an event, subscribe to it and fire the event.
        //
        pubsubz.define("EventSystem.Msg");

        //! Using a method.
        //
        {
            String eventId = pubsubz.subscribe("EventSystem.Msg", MyEventHandler);

            pubsubz.publish("EventSystem.Msg", "hello", "from", "demo.html!");

            pubsubz.unsubscribe(eventId);
        }

        //! Using delegate.
        //
        {
            pubsubz.TopicEvent te = (topic, args) =>
            {
                Console.WriteLine("[demo.html].{0}: [{1}] (delegate)", topic, ArgsToString(args));
            };

            String eventId = pubsubz.subscribe("EventSystem.Msg", te);

            pubsubz.publish("EventSystem.Msg", 1, 2, Math.PI);

            pubsubz.unsubscribe(eventId);
        }

        //! Using anonymous delegate.
        //
        {
            String eventId = pubsubz.subscribe("EventSystem.Msg", (topic, args) =>
            {
                Console.WriteLine("[demo.html].{0}: [{1}] (anonymous delegate)", topic, ArgsToString(args));
            });

            pubsubz.publish("EventSystem.Msg", "hello", "from", "demo.html!");

            pubsubz.unsubscribe(eventId);
        }
    }

    /// <summary>
    /// Tests 06 sanity checks.
    /// </summary>
    private static void Test_06_SanityChecks()
    {
        //! Check if id and class can still be changed (shouldn't).
        //
        //asset4.Id = "XYY1Z";
        //asset4.Class = "test";
        //asset4.log("Asset4: " + asset4.Class + ", " + asset4.Id);

        //! Test if we can re-register without creating new stuff in the register (i.e. get the existing unique id returned).
        //
        Console.WriteLine("Trying to re-register: {0}", AssetManager.Instance.registerAssetInstance(asset4, asset4.Class));
    }

    /// <summary>
    /// Tests 07 dialogue asset.
    /// </summary>
    private static void Test_07_DialogueAsset(NewBehaviourScript scriptResult)
    {
        const string script =
@"0 Hi! Welcome to RAGE! Is this your first visit? [1,2]
1 Yes, I just arrived-> 3
2 No, I've been here before -> 4
3 Awesome!Have fun! -> 5
4 Hey, welcome back -> 5
5 Do you want to be my friend?[6, 7, 8, 9]
6 Yes-> 10
7 No-> 11
8 Maybe-> 12
9 Not sure -> 12
10 Great! -> 13
11 Awwww-> 13
12 Please! -> 5
13 Oh hi -> 13
banana: I hate bananas
";

        //! DialogAsset.
        //
        asset5.ParseScript("me", script);

        // Interacting using ask/tell
        scriptResult.dialogueText.text = asset5.interact("me", "player", "banana").text;

        //Text go = (Text)GameObject.Find("/Text");

        // Interacting using branches
        //
        scriptResult.dialogueText.text = asset5.interact("me", "player").text;
        scriptResult.dialogueText.text = asset5.interact("me", "player", 2).text; //Answer id 2

        scriptResult.dialogueText.text = asset5.interact("me", "player").text;
        scriptResult.dialogueText.text = asset5.interact("me", "player", 6).text; //Answer id 6

        scriptResult.dialogueText.text = asset5.interact("me", "player").text;
    }

    private static void Test_08_Settings(NewBehaviourScript script)
    {
        //script.dialogueText.text = asset1.DefaultSettings.Count.ToString();//["NewKey0"].;

        //! Log Default Settings
        Debug.Log(asset1.SettingsToXml());

        //! Log Default Settings
        asset2.Bridge = bridge1;
        Debug.Log(asset2.SettingsToXml());

        //! Save App Default Settings if not present (and Settings is not null).
        asset2.SaveDefaultSettings(false);

        //! Load App Default Settings if present (and Settings is not null).
        asset2.LoadDefaultSettings();
        Debug.Log(asset2.SettingsToXml());

        //! Try Saving an Asset with No Settings (null)
        if (asset3.hasSettings)
        {
            asset3.SaveDefaultSettings(false);

            Debug.Log(asset3.SettingsToXml());
        }

        //! Save Runtime Settings
        asset2.SaveSettings("runtime-settings.xml");

        //! Load Runtime Settings.
        asset1.Bridge = bridge1;
        asset1.LoadSettings("runtime-settings.xml");

        Debug.Log(asset1.SettingsToXml());
    }

    // Use this for initialization
    void Start()
    {
        if (AssetManager.Instance != null)
        {
            Debug.Log("Asset Manager Created");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //
    }

    #endregion Methods
}