﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSync.Services.Interface
{
   public interface ISQLDbConfig
    {
        string FaxBlackListConnectionString { get; }
        string FaxLogConnectionString { get; }
    }
}
