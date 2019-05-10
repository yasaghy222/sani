using AutoMapper;
using DataLayer;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ViewModel.Areas;
using System.Linq;
using ViewModel.Areas;
using ViewModel.General;
using Utility;
using System.ComponentModel.DataAnnotations;
using System;
using Sani.Controllers;

namespace Sani
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ControllerBuilder.Current.SetControllerFactory(new NinjectController());


            Mapper.Initialize(
                config =>
                {
                    #region general
                    config.CreateMap<tblDocuments, Common.ViewTblDoc>();

                    config.CreateMap<Factor.ViewTblFactor, Factor.ViewOrder>()
                          .ForMember(source => source.registerDate, dest => dest.MapFrom(x => x.registerDate.GetPersianDate(2)))
                          .ForMember(source => source.customerName, dest => dest.MapFrom(x => x.tblCustomer.name))
                          .ForMember(source => source.orderTitles, dest => dest.MapFrom(x => x.tblAssignToFactor.GetOrderTitles()))
                          .ForMember(source => source.forceName, dest => dest.MapFrom(x => x.tblForce.name))
                          .ForMember(source => source.byWho, dest => dest.MapFrom(x => Functions.GetByWho(x.elctricShopId, x.opratorId)))
                          .ForMember(source => source.branchName, dest => dest.MapFrom(x => x.tblBranch.title))
                          .ForMember(source => source.status, dest => dest.MapFrom(x => (EnumExtensions.GetEnumValue<Common.FactorStatus>(x.status)).GetAttribute<DisplayAttribute>().Name));

                    config.CreateMap<Factor.FactorVariable, tblFactor>()
                          .ForMember(source => source.bCode, dest => dest.MapFrom(x => Guid.Empty))
                          .ForMember(source => source.tblAssignToFactor, dest => dest.MapFrom(x => new tblAssignToFactor()));

                    config.CreateMap<tblAssignToFactor, Factor.ViewFactorItem>()
                         .ForMember(source => source.title, dest => dest.MapFrom(x => x.tblService.title));

                    config.CreateMap<tblFactor, Factor.FactorPreview>()
                        .ForMember(source => source.customerName, dest => dest.MapFrom(x => x.tblCustomer.name))
                        .ForMember(source => source.customerPhoneNumber, dest => dest.MapFrom(x => x.tblCustomer.phoneNumber))
                        .ForMember(source => source.customerPusheId, dest => dest.MapFrom(x => x.tblCustomer.pusheId))
                        .ForMember(source => source.address, dest => dest.MapFrom(x => x.tblCustomerAddress.address))
                        .ForMember(source => source.lat, dest => dest.MapFrom(x => x.tblCustomerAddress.lat))
                        .ForMember(source => source.lng, dest => dest.MapFrom(x => x.tblCustomerAddress.lng));
                    #endregion

                    #region electric shop
                    config.CreateMap<EShop.ViewTblEShop, EShop.ViewEShop>()
                                  .ForMember(source => source.branchName, dest => dest.MapFrom(x => x.tblBranch.title))
                                  .ForMember(source => source.creditor, dest => dest.MapFrom(x => x.creditor.SetCama()));
                    #endregion

                    #region force
                    config.CreateMap<Force.ViewTblForce, Force.ViewForce>()
                          .ForMember(source => source.branchName, dest => dest.MapFrom(x => x.tblBranch.title))
                          .ForMember(source => source.debtor, dest => dest.MapFrom(x => x.debtor.SetCama()))
                          .ForMember(source => source.creditor, dest => dest.MapFrom(x => x.creditor.SetCama()))
                          .ForMember(source => source.status, dest => dest.MapFrom(x => (EnumExtensions.GetEnumValue<Common.ForceStatus>(x.status)).GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name));
                    #endregion

                    #region customer
                    config.CreateMap<tblCustomer, Customer.OrderCustomer>()
                          .ForMember(source => source.bCode, dest => dest.MapFrom(x => x.locationId.GetBCode()));

                    config.CreateMap<tblCustomerAddress, Customer.OrderAddress>()
                          .ForMember(source => source.cityName, dest => dest.MapFrom(x => x.tblState_City.title))
                          .ForMember(source => source.isSelected, dest => dest.MapFrom(x => false));

                    config.CreateMap<Customer.OAddressVar, tblCustomerAddress>()
                          .ForMember(source => source.id, dest => dest.MapFrom(x => Functions.GenerateNewId()))
                          .ForMember(source => source.lat, dest => dest.MapFrom(x => double.Parse(x.lat)))
                          .ForMember(source => source.lng, dest => dest.MapFrom(x => double.Parse(x.lng)));
                    #endregion
                }
            );
        }
    }
}
