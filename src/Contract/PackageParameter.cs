﻿using System;
using System.Linq;
using System.Xml.Serialization;

namespace Remotis.Contract
{
    [XmlRootAttribute("parameter")]
    class PackageParameter
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlText]
        public object Value { get; set; }
    }
}
