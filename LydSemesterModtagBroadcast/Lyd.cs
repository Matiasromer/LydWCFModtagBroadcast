using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LydSemesterModtagBroadcast
{
    [DataContract]
    public class Lyd
    {

        [DataMember]
        public DateTime Date;

        [DataMember]
        public string Lyde;

        [DataMember]
        public int Id;
    }
}