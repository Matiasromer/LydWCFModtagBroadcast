using Microsoft.VisualStudio.TestTools.UnitTesting;
using LydSemesterModtagBroadcast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LydSemesterModtagBroadcast.Tests
{
    [TestClass()]
    public class Service1Tests
    {
        [TestMethod()]
        public void SetIdStedTest()
        {
            Service1 service = new Service1();

            service.SetIdSted(3);

            //Assert.Fail();
        }
        //[TestMethod()]
        //public void GetAllLydTest()
        //{
        //    Service1 service = new Service1();

        //    service.GetAllLyd();

        //    Assert.AreEqual();
        //}
    }
}