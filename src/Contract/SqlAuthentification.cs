﻿using System;
using System.Linq;
using System.Xml.Serialization;

namespace Remotis.Contract
{
    [XmlRootAttribute("authentification-sql")]
    public class SqlAuthentification
    {
        [XmlAttribute("username")]
        public string UserName { get; set; }
        [XmlAttribute("password")]
        public string Password { get; set; }
    }
}
