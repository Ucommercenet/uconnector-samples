﻿using UConnector.Api.V1;
using UConnector.Extensions.Receivers;
using UConnector.Extensions.Senders;
using UConnector.Extensions.Transformers;

namespace UConnector.Samples.Operations.UCommerce.ImportLocalFile
{
	public class ImportLocalExcelFileOperation : Operation
	{
		protected override IOperation BuildOperation()
		{
			return FluentOperationBuilder
				.Receive<FilesFromLocalDirectory>()
					.WithOption(x => x.Pattern = "*.xslx")
				.Debatch()
				.Transform<WorkFileToStream>()
				.Transform<FromExcelStreamToDataTable>()
				.Transform<DataTableToProductList>()
				.Send<ProductListToUCommerce>()
				.ToOperation();
		}
	}
}
