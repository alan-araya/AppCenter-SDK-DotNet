// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows.Data.Xml.Dom;

namespace Microsoft.AppCenter.Test.UWP
{
    [TestClass]
    public class UWPPushTest
    {
        /// <summary>
        /// Verify ParseLaunchString works when launch string is null
        /// </summary>
        [TestMethod]
        public void ParseLaunchStringWhenStringIsNull()
        {
            var result = Push.Push.ParseLaunchString(null);
            Assert.IsNull(result);
        }

        /// <summary>
        /// Verify ParseLaunchString works when launch string is empty
        /// </summary>
        [TestMethod]
        public void ParseLaunchStringWhenStringIsEmpty()
        {
            var result = Push.Push.ParseLaunchString(string.Empty);
            Assert.IsNull(result);
        }

        /// <summary>
        /// Verify ParseLaunchString works when launch string is not valid Json
        /// </summary>
        [TestMethod]
        public void ParseLaunchStringWhenStringContainsInvalidJson()
        {
            var actualResult = Push.Push.ParseLaunchString("{\"mobile_center\":{\"key1\":\"value1\",\"key2\":\"value2\"");
            Assert.IsNull(actualResult);
        }

        /// <summary>
        /// Verify ParseLaunchString works when launch string doesn't contain "mobile_center"
        /// </summary>
        [TestMethod]
        public void ParseLaunchStringWhenStringDoesNotContainAppCenter()
        {
            var actualResult = Push.Push.ParseLaunchString("{\"foobar\":{\"key1\":\"value1\",\"key2\":\"value2\"}}");
            Assert.IsNull(actualResult);
        }

        /// <summary>
        /// Verify ParseLaunchString works with new key
        /// </summary>
        [TestMethod]
        public void ParseLaunchStringAppCenter()
        {
            var actualResult = Push.Push.ParseLaunchString("{\"appCenter\":{\"key1\":\"value1\",\"key2\":\"value2\"}}");
            var expectedResult = new Dictionary<string, string>
            {
                ["key1"] = "value1",
                ["key2"] = "value2"
            };

            Assert.AreEqual(2, actualResult.Count);
            Assert.AreEqual(expectedResult["key1"], actualResult["key1"]);
            Assert.AreEqual(expectedResult["key2"], actualResult["key2"]);
        }

        /// <summary>
        /// Verify ParseLaunchString works with old key
        /// </summary>
        [TestMethod]
        public void ParseLaunchStringMobileCenter()
        {
            var actualResult = Push.Push.ParseLaunchString("{\"mobile_center\":{\"key1\":\"value1\",\"key2\":\"value2\"}}");
            var expectedResult = new Dictionary<string, string>
            {
                ["key1"] = "value1",
                ["key2"] = "value2"
            };

            Assert.AreEqual(2, actualResult.Count);
            Assert.AreEqual(expectedResult["key1"], actualResult["key1"]);
            Assert.AreEqual(expectedResult["key2"], actualResult["key2"]);
        }

        /// <summary>
        /// Verify ParseAppCenterPush works when custom data is null
        /// </summary>
        [TestMethod]
        public void ParseAppCenterPushWhenCustomDataIsNull()
        {
            var xmlContent = new XmlDocument();
            xmlContent.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><toast><visual><binding template=\"ToastImageAndText02\"><text id=\"1\">test-title</text><text id=\"2\">hello world</text></binding></visual></toast>");
            var actualResult = Push.Push.ParseAppCenterPush(xmlContent);
            Assert.IsNull(actualResult);
        }

        /// <summary>
        /// Verify ParseAppCenterPush works with new key
        /// </summary>
        [TestMethod]
        public void ParseAppCenterPushAppCenter()
        {
            var xmlContent = new XmlDocument();

            xmlContent.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><toast launch=\"{&quot;appCenter&quot;:{&quot;key1&quot;:&quot;value1&quot;,&quot;key2&quot;:&quot;value2&quot;}}\"><visual><binding template=\"ToastImageAndText02\"><text id=\"1\">test-title</text><text id=\"2\">hello world</text></binding></visual></toast>");
            var actualResult = Push.Push.ParseAppCenterPush(xmlContent);

            var expectedResult = new Push.PushNotificationReceivedEventArgs
            {
                Title = "test-title",
                Message = "hello world",
                CustomData = new Dictionary<string, string>
                {
                    ["key1"] = "value1",
                    ["key2"] = "value2"
                }
            };

            Assert.AreEqual(expectedResult.Title, actualResult.Title);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
            Assert.AreEqual(expectedResult.CustomData["key1"], actualResult.CustomData["key1"]);
            Assert.AreEqual(expectedResult.CustomData["key2"], actualResult.CustomData["key2"]);
        }

        /// <summary>
        /// Verify ParseAppCenterPush works with old key
        /// </summary>
        [TestMethod]
        public void ParseAppCenterPushMobileCenter()
        {
            var xmlContent = new XmlDocument();

            xmlContent.LoadXml("<?xml version=\"1.0\" encoding=\"utf-16\"?><toast launch=\"{&quot;mobile_center&quot;:{&quot;key1&quot;:&quot;value1&quot;,&quot;key2&quot;:&quot;value2&quot;}}\"><visual><binding template=\"ToastImageAndText02\"><text id=\"1\">test-title</text><text id=\"2\">hello world</text></binding></visual></toast>");
            var actualResult = Push.Push.ParseAppCenterPush(xmlContent);

            var expectedResult = new Push.PushNotificationReceivedEventArgs
            {
                Title = "test-title",
                Message = "hello world",
                CustomData = new Dictionary<string, string>
                {
                    ["key1"] = "value1",
                    ["key2"] = "value2"
                }
            };

            Assert.AreEqual(expectedResult.Title, actualResult.Title);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
            Assert.AreEqual(expectedResult.CustomData["key1"], actualResult.CustomData["key1"]);
            Assert.AreEqual(expectedResult.CustomData["key2"], actualResult.CustomData["key2"]);
        }
    }
}
