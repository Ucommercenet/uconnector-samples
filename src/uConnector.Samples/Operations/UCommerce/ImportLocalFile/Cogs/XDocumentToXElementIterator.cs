﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UConnector.Attributes;
using UConnector.Cogs;
using UConnector.Config;

namespace UConnector.Samples.Operations.UCommerce.ImportLocalFile.Cogs
{
	public class XDocumentToXElementIterator : Configurable, ICog<XDocument, IEnumerable<XElement>>
	{
        [Required]
        public string DescendendsName { get; set; }

		public IEnumerable<XElement> Execute(XDocument input)
		{
			return input.Descendants(DescendendsName);
		}
	}
}