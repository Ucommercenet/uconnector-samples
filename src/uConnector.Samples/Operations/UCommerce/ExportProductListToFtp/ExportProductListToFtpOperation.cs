﻿using UConnector.Api.V1;
using UConnector.Filesystem;
using UConnector.Samples.Operations.UCommerce.ExportProductListToFtp.Receiver;
using UConnector.Samples.Operations.UCommerce.ExportProductListToFtp.Transformers;
using UConnector.UCommerce;

namespace UConnector.Samples.Operations.UCommerce.ExportProductListToFtp
{
    public class ExportProductListToFtpOperation : Operation
    {
        protected override IOperation BuildOperation()
        {
            return FluentOperationBuilder
				.Receive<ProductListFromUCommerce>()
                .Transform<ProductListToDataTable>()
				.Transform<FromDataTableToExcelStream>()
				.Transform<StreamToWorkfileWithTimestampName>()
					.WithOption(a => a.Extension = ".xlsx")
                .Batch()
                .Send<FtpFilesAdapter>().ToOperation();
        }
    }
}