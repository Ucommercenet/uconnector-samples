﻿using UConnector.Config.Fluent.v1;
using UConnector.Extensions.Cogs.Receivers;
using UConnector.Samples.Framework;
using UConnector.Samples.Operations.UCommerce.ImportLocalFile.Cogs;

namespace UConnector.Samples.Operations.Sandbox.Operations
{
	public class OperationXmlToCsv : CustomOperation
	{
		protected override IOperation BuildOperation()
		{
			return FluentOperationBuilder.Receive<FilesFromLocalDirectory>().WithOption(x => x.Pattern = "*.xml")
								   .Debatch()
								   .Transform<WorkFileToXDocument>()
								   .Transform<XDocumentToXElementIterator>()
								   .Debatch()
								   .Transform<XElementToCsvRow>()
								   .Batch(size: 10)
								   .Send<StringIteratorToFile>()
								   .UsingRetryStrategy<SimpleRetryStrategy>()
								   .ToOperation();
		}
	}
}
