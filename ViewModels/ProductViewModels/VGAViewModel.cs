﻿using Models.DTOs;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ViewModels.Commands;
using ViewModels.Services;
using ViewModels.Stores;
using ViewModels.Stores.VGAAttributes;
using Models.Interfaces;

namespace ViewModels.ProductViewModels;
public class VGAViewModel : ProductViewModel<VgaDTO>
{
    public HashSet<VgaCompany> CompanyList { get; set; }
    public HashSet<VgaChip> ChipList { get; set; }
    public HashSet<VgaChipset> ChipsetList { get; set; }
    public HashSet<VgaNeed> NeedList { get; set; }
    public HashSet<VgaSeries> SeriesList { get; set; }
    public HashSet<VgaVram> VramList { get; set; }
    public HashSet<VgaGen> GenList { get; set; }
    
    public VGAViewModel(IUnitOfWork unitOfWork,
        ProductDetailStore productDetailStore,
        INavigationService productDetailNavigate)
        : base(unitOfWork, productDetailStore, productDetailNavigate)
    {
        var list = _unitOfWork.Vgas.GetAll();
        if (list != null) ProductList = new(_productDTOs = list!);
        Action += OnIsCheckedChanged;
        getCompanyList();
        getChipList();
        getChipsetList();
        getNeedList();
        getVramList();
        getSeriesList();
        getGenList();
    }
    private void OnIsCheckedChanged()
    {
        List<string> ListCompany = new();
        List<string> ListChip = new();
        List<string> ListChipset = new();
        List<string> ListNeed = new();
        List<string> ListVram = new();
        List<string> ListSeries = new();
        List<string> ListGen = new();
        ProductList = new();
        foreach (var e in CompanyList)
            if (e.IsChecked) ListCompany.Add(e.Name);
        foreach (var e in ChipList)
            if (e.IsChecked) ListChip.Add(e.Name);
        foreach (var e in ChipsetList)
            if (e.IsChecked) ListChipset.Add(e.Name);
        foreach (var e in NeedList)
            if (e.IsChecked) ListNeed.Add(e.Name);
        foreach (var e in VramList)
            if (e.IsChecked) ListVram.Add(e.Name);
        foreach (var e in SeriesList)
            if (e.IsChecked) ListSeries.Add(e.Name);
        foreach (var e in GenList)
            if (e.IsChecked) ListGen.Add(e.Name);
        if (ListCompany.Count != 0) ProductList = _productDTOs!.Where(x => ListCompany.Contains(x.Company)).ToList();
        else ProductList = (List<VgaDTO>?)_productDTOs;
        if (ListChip.Count != 0) ProductList = ProductList!.Where(x => ListChip.Contains(x.Chip)).ToList();
        if (ListChipset.Count != 0) ProductList = ProductList!.Where(x => ListChipset.Contains(x.Chipset)).ToList();
        if (ListNeed.Count != 0) ProductList = ProductList!.Where(x => ListNeed.Contains(x.Need)).ToList();
        if (ListVram.Count != 0) ProductList = ProductList!.Where(x => ListVram.Contains(x.Vram)).ToList();
        if (ListSeries.Count != 0) ProductList = ProductList!.Where(x => ListSeries.Contains(x.Series)).ToList();
        if (ListGen.Count != 0) ProductList = ProductList!.Where(x => ListGen.Contains(x.Gen)).ToList();
        OnPropertyChanged(nameof(ProductList));
    }
    private void getCompanyList()
    {
        if (_productDTOs == null) return;
        CompanyList = new();
        foreach (var vga in _productDTOs)
        {
            VgaCompany vgaCompany = new() { Name = vga.Company };
            vgaCompany.CurrentStoreChanged += OnIsCheckedChanged;
            CompanyList.Add(vgaCompany);
        }
    }
    private void getChipList()
    {
        if (_productDTOs == null) return;
        ChipList = new();
        foreach (var vga in _productDTOs)
        {
            VgaChip vgaCPU = new() { Name = vga.Chip };
            vgaCPU.CurrentStoreChanged += OnIsCheckedChanged;
            ChipList.Add(vgaCPU);
        }
    }
    private void getChipsetList()
    {
        if (_productDTOs == null) return;
        ChipsetList = new();
        foreach (var vga in _productDTOs)
        {
            VgaChipset vgaGraphic = new() { Name = vga.Chipset };
            vgaGraphic.CurrentStoreChanged += OnIsCheckedChanged;
            ChipsetList.Add(vgaGraphic);
        }
    }
    private void getNeedList()
    {
        if (_productDTOs == null) return;
        NeedList = new();
        foreach (var vga in _productDTOs)
        {
            if (vga.Need == null) continue;
            VgaNeed vgaNeed = new() { Name = vga.Need };
            vgaNeed.CurrentStoreChanged += OnIsCheckedChanged;
            NeedList.Add(vgaNeed);
        }
    }
    private void getVramList()
    {
        if (_productDTOs == null) return;
        VramList = new();
        foreach (var vga in _productDTOs)
        {
            VgaVram vgaVram = new() { Name = vga.Vram };
            vgaVram.CurrentStoreChanged += OnIsCheckedChanged;
            VramList.Add(vgaVram);
        }
    }
    private void getSeriesList()
    {
        if (_productDTOs == null) return;
        SeriesList = new();
        foreach (var vga in _productDTOs)
        {
            if (vga.Series == null) continue;
            VgaSeries vgaSeries = new() { Name = vga.Series };
            vgaSeries.CurrentStoreChanged += OnIsCheckedChanged;
            SeriesList.Add(vgaSeries);
        }
    }
    private void getGenList()
    {
        if (_productDTOs == null) return;
        GenList = new();
        foreach (var vga in _productDTOs)
        {
            VgaGen vgaGen = new() { Name = vga.Gen };
            vgaGen.CurrentStoreChanged += OnIsCheckedChanged;
            GenList.Add(vgaGen);
        }
    }
}
