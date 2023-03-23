﻿using ESM.Core;
using ESM.Core.ShareServices;
using ESM.Modules.DataAccess;
using ESM.Modules.DataAccess.DTOs;
using ESM.Modules.DataAccess.Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESM.Modules.Normal.ViewModels
{
    public abstract class BaseProductViewModel<T> : BindableBase, INavigationAware
    where T : ProductDTO
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IModalService _modalService;
        protected dynamic _productDTOs;
        public dynamic ProductList { get; set; }
        public List<string> Conditions { get; } = StaticData.Conditions;
        protected string selectedCondition;
        public double MaxPrice { get; set; } = 0;
        public double TickFrequency { get; } = 500_000;
        private double currentPrice;
        public double CurrentPrice
        {
            get => currentPrice;
            set
            {
                currentPrice = value;
                priceRangeCommand();
            }
        }
        protected BaseProductViewModel(IUnitOfWork unitOfWork, IModalService modalService)
        {
            _unitOfWork = unitOfWork;
            _modalService = modalService;
            GetProductList();
            ProductDetailNavigateCommand = new(navigate);
        }
        private void GetProductList()
        {
            dynamic list = null;
            if (typeof(T).Equals(typeof(LaptopDTO)))
                list = _unitOfWork.Laptops.GetAll();
            else if (typeof(T).Equals(typeof(MonitorDTO)))
                list = _unitOfWork.Monitors.GetAll();
            else if(typeof(T).Equals(typeof(PccpuDTO)))
                list = _unitOfWork.Pccpus.GetAll();
            else if (typeof(T).Equals(typeof(PcDTO)))
                list = _unitOfWork.Pcs.GetAll();
            else if(typeof(T).Equals(typeof(SmartphoneDTO)))
                list = _unitOfWork.Smartphones.GetAll();
            else if (typeof(T).Equals(typeof(VgaDTO)))
                list = _unitOfWork.Vgas.GetAll();
            else if (typeof(T).Equals(typeof(PcharddiskDTO)))
                list = _unitOfWork.Pcharddisks.GetAll();
            if (list != null && list.Count > 0)
            {
                ProductList = list;
                _productDTOs = list;
                MaxPrice = Math.Ceiling((double)((List<T>)list).Max(x => x.SellPrice) / TickFrequency) * TickFrequency;
                CurrentPrice = MaxPrice;
            }
        }
        private void priceRangeCommand()
        {
            Action?.Invoke();
            ProductList = ((List<T>)ProductList).Where(x => (double)x.SellPrice <= CurrentPrice).ToList();
            RaisePropertyChanged(nameof(ProductList));
        }
        public string? SelectedCondition
        {
            get => selectedCondition;
            set
            {
                selectedCondition = value;
                RaisePropertyChanged(nameof(SelectedCondition));
                SelectedConditionChanged();
            }
        }
        public DelegateCommand<ProductDTO> ProductDetailNavigateCommand { get; set; }
        private void navigate(ProductDTO product)
        {
            NavigationParameters parameter = new()
            {
                {"Product" , product}
            };
            _modalService.ShowModal(ViewNames.ProductDetailView, parameter);
        }
        protected Action? Action { get; set; }
        protected void SelectedConditionChanged()
        {
            if (SelectedCondition == null) return;
            if (SelectedCondition == Conditions[0])
            {
                SelectedCondition = null;
                Action?.Invoke();
            }
            else if (SelectedCondition == Conditions[1])
            {
                ProductList = ((List<T>)ProductList)?.OrderBy(x => x.SellPrice).ToList();
            }
            else if (SelectedCondition == Conditions[2])
            {
                ProductList = ((List<T>)ProductList)?.OrderByDescending(x => x.SellPrice).ToList();
            }
            RaisePropertyChanged(nameof(ProductList));
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
           
        }
    }
}
