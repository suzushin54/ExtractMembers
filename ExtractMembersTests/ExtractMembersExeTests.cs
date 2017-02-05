using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExtractMembers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractMembers.Tests
{
    [TestClass()]
    public class ExtractMembersExeTests
    {
        [TestMethod()]
        public void MainTest()
        {
            var sampleFile = "D:\\Shinichiro\\Documents\\Git\\sharetomato\\src\\sharetomato\\Controllers\\HomeController.cs";
            
            var em = new ExtractMembersExe();
            var rv = em.Main(sampleFile);


            Assert.Fail();
        }
    }
}