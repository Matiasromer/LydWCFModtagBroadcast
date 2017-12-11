using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LydSemesterModtagBroadcast
{
    [DataContract]
    public class Personale
    {
        

        [DataMember]

        public string Navn;

        [DataMember]
        public int Telf;

        [DataMember]
        public string Email;

        [DataMember]
        public string Sted;
    }
}