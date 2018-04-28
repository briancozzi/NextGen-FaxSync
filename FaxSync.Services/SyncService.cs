using FaxSync.Domain;
using FaxSync.Models;
using FaxSync.Models.FaxApi;
using FaxSync.Models.Interface;
using FaxSync.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSync.Services
{
    public class SyncService
    {
        protected AdUserService AdUserService { get; set; }
        protected DbUserService DbUserService { get; set; }
        protected HelperService HelperService { get; set; }
        protected FaxApiService FaxApiServiceReal { get; set; }
        protected FaxApiService FaxApiService { get; set; }
        protected LogService LogService { get; set; }
        public SyncService()
        {
            AdUserService = new AdUserService();
            DbUserService = new DbUserService();
            HelperService = new HelperService();
            FaxApiService = new FaxApiService();
            LogService = new LogService();
        }
    }
}