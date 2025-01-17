﻿using EnvisionStaking.Casper.SDK.Model.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnvisionStaking.Casper.SDK.Model.DeployObject
{
    public class PutDeployTransferRequest : Base.Request
    {
        public PutDeployTransferRequest()
        {
            this.method = "account_put_deploy";
            this.Parameters = new PutDeployTransferParameters();
        }

        [JsonProperty("params")]
        public PutDeployTransferParameters Parameters { get; set; }
    }
}