﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EnvisionStaking.Casper.SDK.Model.Common
{
    public class ExecutionResultRoot
    {
        public string block_hash { get; set; }
        public ExecutionResult result { get; set; }
    }  
}
