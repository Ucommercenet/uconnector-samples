﻿using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Web.Mvc;
using UCommerce.EntitiesV2;
using UConnector.Config;
using UConnector.Extensions.Cogs.Senders;
using UConnector.Extensions.Cogs.Transformers;
using UConnector.Extensions.Cogs.TwoWayCogs;
using UConnector.Extensions.Model;
using UConnector.MvcApplication.Cogs.Models;
using UConnector.MvcApplication.Cogs.Transformers;
using UConnector.Extensions;
using UConnector.MvcApplication.Models;
using UConnector.Validatation;

namespace UConnector.MvcApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly OperationValidater _OperationValidater;

        public HomeController()
        {
            _OperationValidater = ObjectFactory.Resolve<OperationValidater>();
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            var model = new IndexModel
                            {
                                //ProductCatalogs = ProductCatalog.All().ToList(),
                                ProductCatalogGroups = ProductCatalogGroup.All().ToList(),
                                //Categories = Category.All().ToList()
                            };

            return View("Index", model);
        }

        public ActionResult Download(int? id, string typeName, DownloadAs? type)
        {
            var typeInfo = new TypeInfo
                               {
                                   Id = id.GetValueOrDefault(0),
                                   TypeName = typeName ?? "",
                                   Type = type.GetValueOrDefault(DownloadAs.Excel),
                               };

            WorkFile output = null;

            var builder = OperationBuilder.Create()
                .Decision<IfTypeInfoTypeIsExcelDecision>(
                    OperationBuilder.Create()
                        .Cog<TypeInfoToProductListCog>()
                        .Cog<ProductListToDataTableCog>()
                        .Cog<ExcelCog>()
                        .Cog<NamingCog>().WithOption(a => a.Extension = ".xlsx")
                        .Send<InvokeMethodSender<WorkFile>>().WithOption(x => x.Method = (value) => output = value).
                        GetOperation().FirstStep)
                        .Cog<TypeInfoToProductListCog>()
                        .Cog<ProductListToDataTableCog>()
                        .Cog<CsvCog>()
                        .Cog<NamingCog>().WithOption(a => a.Extension = ".csv")
                        .Send<InvokeMethodSender<WorkFile>>().WithOption(x => x.Method = (value) => output = value);

            var operation = builder.GetOperation();
            var runner = new OperationEngine();
            runner.Execute(operation, typeInfo);
            output.Stream.Flush();
            output.Stream.Position = 0;
            return File(output.Stream, MediaTypeNames.Application.Octet, output.Name);
        }
    }
}