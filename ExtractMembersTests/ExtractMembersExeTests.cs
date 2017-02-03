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
            var sampleFile = "D:\\TFS\\VBMigrationCollection\\001_Service\\10_CoolCatツール\\10_SFL提供\\10_PJWeb\\10_標準MSI\\CoolCatSetup_0000201606300100_Class\\Class\\CoolCatDataOO4OODAC\\Source\\OraDatabase.vb";
            
            var em = new ExtractMembersExe();
            var rv = em.Main(sampleFile);


            Assert.Fail();
        }
    }
}