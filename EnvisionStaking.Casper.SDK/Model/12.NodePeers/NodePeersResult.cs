﻿using EnvisionStaking.Casper.SDK.Model.Common;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnvisionStaking.Casper.SDK.Model.NodePeers
{
    public class NodePeersResult : Base.Result
    {
        public ResultBody result { get; set; }
    }

    public class ResultBody
    {
        public string api_version { get; set; }
        public List<Peer> peers { get; set; }
    }   
}



