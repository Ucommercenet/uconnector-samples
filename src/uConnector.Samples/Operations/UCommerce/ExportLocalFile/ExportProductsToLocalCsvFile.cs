﻿using UConnector.Api.V1;
using UConnector.IO;
using UConnector.UCommerce;

namespace UConnector.Samples.Operations.UCommerce.ExportLocalFile
{
	public class ExportProductsToLocalCsvFile : Operation
	{
		protected override IOperation BuildOperation()
		{
			return FluentOperationBuilder
				.Receive<ProductListFromUCommerce>()
				.Transform<ProductListToDataTable>()
				.Transform<FromDataTableToCsvStream>()
				.Transform<StreamToWorkfileWithTimestampName>()
					.WithOption(a => a.Extension = ".csv")
				.Batch(size: 1)
				.Send<FilesToLocalDirectory>()
					.WithOption(x => x.Overwrite = true)
				.ToOperation();
		}
	}
}
