﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace LydSemesterModtagBroadcast
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        int PostLydToList(string lyd);

        [OperationContract]
        int SetIdSted(string SetId);
        //[OperationContract]
        //int SetStedId(int stedid);
        [OperationContract]
        int usestedid();

        [OperationContract]
        IList<Lyd> GetAllLyd();

        [OperationContract]
        bool TjekStatus();

        [OperationContract]
        void Update();

        [OperationContract]
        IList<Lyd> GetAlllydSorted();

        // Test metode der henter alle lyde der har IdSted = 2
        //[OperationContract]
        //IList<Lyd> GetAlllydSted2();

        [OperationContract]
        IList<Personale> GetAllPersonale();

        // Henter den inner join tabel, så lyd viser også sted (Hardcoded i db)
        [OperationContract]
        IList<Lyd> GetAllLydMedSted();

        [OperationContract]
        IList<Lyd> GetMaxLyd();

        [OperationContract]
        IList<Personale> GetHvorHvem();

        //[OperationContract]
        //void UpdateStatus(string onOff);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    
    
}
