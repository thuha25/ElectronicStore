﻿using Models.DTOs;
using System.Collections.Generic;
using System.Linq;
using ViewModels.Services;
using ViewModels.Stores;
using ViewModels.Stores.PCCPUAttributes;
using Models.Interfaces;

namespace ViewModels.ProductViewModels;
public class PCCPUViewModel : ProductViewModel<PccpuDTO>
{
    public HashSet<PccpuCompany> CompanyList { get; set; }
    public HashSet<PccpuNeed> NeedList { get; set; }
    public HashSet<PccpuSocket> SocketList { get; set; }
    public HashSet<PccpuSeries> SeriesList { get; set; }
    public PCCPUViewModel(IUnitOfWork unitOfWork,
        ProductDetailStore productDetailStore,
        INavigationService productDetailNavigate)
        : base(unitOfWork, productDetailStore, productDetailNavigate)
    {
        var list = _unitOfWork.Pccpus.GetAll();
        if (list != null) ProductList = new(_productDTOs = list!);
        Action += OnIsCheckedChanged;
        getCompanyList();
        getSocketList();
        getNeedList();
        getSeriesList();
    }
    private void OnIsCheckedChanged()
    {
        List<string> ListCompany = new();
        List<string> ListSocket = new();
        List<string> ListNeed = new();
        List<string> ListSeries = new();
        ProductList = new();
        foreach (var e in CompanyList)
            if (e.IsChecked) ListCompany.Add(e.Name);
        foreach (var e in SocketList)
            if (e.IsChecked) ListSocket.Add(e.Name);
        foreach (var e in NeedList)
            if (e.IsChecked) ListNeed.Add(e.Name);
        foreach (var e in SeriesList)
            if (e.IsChecked) ListSeries.Add(e.Name);
        if (ListCompany.Count != 0) ProductList = _productDTOs!.Where(x => ListCompany.Contains(x.Company)).ToList();
        else ProductList = (List<PccpuDTO>?)_productDTOs;
        if (ListSocket.Count != 0) ProductList = ProductList!.Where(x => ListSocket.Contains(x.Socket)).ToList();
        if (ListNeed.Count != 0) ProductList = ProductList!.Where(x => ListNeed.Contains(x.Need)).ToList();
        if (ListSeries.Count != 0) ProductList = ProductList!.Where(x => ListSeries.Contains(x.Series)).ToList();
        OnPropertyChanged(nameof(ProductList));
    }
    private void getCompanyList()
    {
        if (_productDTOs == null) return;
        CompanyList = new();
        foreach (var pccpu in _productDTOs)
        {
            PccpuCompany pccpuCompany = new() { Name = pccpu.Company };
            pccpuCompany.CurrentStoreChanged += OnIsCheckedChanged;
            CompanyList.Add(pccpuCompany);
        }
    }
    private void getSocketList()
    {
        if (_productDTOs == null) return;
        SocketList = new();
        foreach (var pccpu in _productDTOs)
        {
            PccpuSocket pccpusocket = new() { Name = pccpu.Socket };
            pccpusocket.CurrentStoreChanged += OnIsCheckedChanged;
            SocketList.Add(pccpusocket);
        }
    }
    private void getNeedList()
    {
        if (_productDTOs == null) return;
        NeedList = new();
        foreach (var pccpu in _productDTOs)
        {
            if (pccpu.Need == null) continue;
            PccpuNeed pccpuNeed = new() { Name = pccpu.Need };
            pccpuNeed.CurrentStoreChanged += OnIsCheckedChanged;
            NeedList.Add(pccpuNeed);
        }
    }
    private void getSeriesList()
    {
        if (_productDTOs == null) return;
        SeriesList = new();
        foreach (var pccpu in _productDTOs)
        {
            if (pccpu.Series == null) continue;
            PccpuSeries pccpuSeries = new() { Name = pccpu.Series };
            pccpuSeries.CurrentStoreChanged += OnIsCheckedChanged;
            SeriesList.Add(pccpuSeries);
        }
    }
}
