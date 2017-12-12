using Microsoft.VisualStudio.TestTools.UnitTesting;
using LydSemesterModtagBroadcast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LydSemesterModtagBroadcastTests
{
    [TestClass()]
    public class Service1Tests
    {
        [TestMethod()]
        public void TjekStatusTestTrue()
        {
            Service1 service = new Service1();

            service.TjekStatus();

            Assert.IsTrue(service.TjekStatus());
        }

        [TestMethod()]
        public void TjekStatusTestFalse()
        {
            Service1 service = new Service1();

            service.TjekStatus();

            Assert.IsFalse(service.TjekStatus());
        }

        [TestMethod()]
        public void SetIdStedTest()
        {


            Service1 service = new Service1();

            service.SetIdSted("1");


        }


        [TestMethod()]
        public void PostLydToListTest()
        {
            Service1 service2 = new Service1();

            service2.PostLydToList("70");


        }

        [TestMethod()]
        public void Updat2Test()
        {
            bool value = true;

            Service1 service3 = new Service1();

            service3.Update();

            Assert.IsTrue(value);


        }



    }



   
      
    
}