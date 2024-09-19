using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTestProject1
{
  
    public class UnitTest1
    { 
        [Fact]
        [TestMethod]
        public void TestMethod1()
        {
            var x = new apartmentController();
           Assert.IsInstanceOfType<List<apartment>>();
        }
    }
}
