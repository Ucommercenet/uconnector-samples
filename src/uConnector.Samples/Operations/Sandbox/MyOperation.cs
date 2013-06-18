﻿using UConnector.Api.V1;
using UConnector.Filesystem;
using UConnector.Helpers;
using UConnector.Samples.Transformers;

namespace UConnector.Samples.Operations.Sandbox
{
	public class MyOperation : Operation
	{
		protected override IOperation BuildOperation()
		{
			return FluentOperationBuilder
				.Receive<FilesFromLocalDirectory>()			// receives IEnumerable<WorkFile>
					.WithOption(x => x.Pattern = "*.xml")
				.Debatch()									// IEnumerable<WorkFile> to WorkFile
				.Transform<WorkFileToXDocument>()			// WorkFile to XDocument
				.Transform<XDocumentToXElements>()			// XDocument to IEnumerable<XElement>
					.WithOption(x => x.DescendendsName = "author")
				.Debatch()									// IEnumerable<XElement> to XElement
				.Transform<XElementToValue>()				// XElement to string
				.Batch()									// string to IEnumerable<string>
				.Transform<DistinctFilter<string>>()		// IEnumerable<string> to IEnumerable<string>
				.Send<StringsToFileInLocalDirectory>()						// sends IEnumerable<string> to a file.
				.ToOperation();
		}
	}
}
