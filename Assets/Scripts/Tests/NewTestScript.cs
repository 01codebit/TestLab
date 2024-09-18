using System.Collections;
using System.Collections.Generic;
using System.Web;
using NUnit.Framework;
using TestLab.EventChannel;
using UnityEngine;
using UnityEngine.TestTools;

public class NewTestScript
{
    // A Test behaves as an ordinary method
    [Test]
    public void NewTestScriptSimplePasses()
    {
        // Use the Assert class to test conditions
        string _endpoint = "todos";
        Dictionary<string, string> _parameters = new Dictionary<string, string>()
        {
            {"userId","1"}, 
            {"completed","false"}
        };
        var result = HttpUtils.GetRequestUri(_endpoint, _parameters);
        Assert.Equals(result, "zipiti");
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
