using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using terra.Controllers;
using terra.Models;

namespace TerraUnitTest
{
    [TestClass]
    public class TerraTest
    {
        [TestMethod]
        public void StatusNull()
        {
            Assert.IsNotNull(new StatusController().Get());
        }

        [TestMethod]
        public void StatusValues()
        {
            Assert.AreEqual(true, new StatusController().Get().Light);
        }

        [TestMethod]
        public void PostStatus()
        {
            var s = new Status
            {
                Light = true,
                Temp = 40,
                Humid = 30,
                Id = 4,
                IsDay = true,
                Timestamp = DateTime.Now
            };

            try
            {
                new StatusController().Post(s);
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }
    }
}
