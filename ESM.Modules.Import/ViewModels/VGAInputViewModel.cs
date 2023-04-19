﻿using ESM.Core.ShareServices;
using ESM.Modules.DataAccess;
using ESM.Modules.DataAccess.Infrastructure;
using ESM.Modules.DataAccess.Models;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ESM.Modules.Import.ViewModels
{
    public class VGAInputViewModel : BaseProductInputViewModel<Vga>
    {
        public VGAInputViewModel(IUnitOfWork unitOfWork, IOpenDialogService openDialogService, IModalService modalService) : base(unitOfWork, openDialogService, modalService)
        {
        }
        public string Header => "VGA";
        private string chip;
        public string Chip
        {
            get => chip?.Trim();
            set => SetProperty(ref chip, value);
        }
        private string chipset;
        public string Chipset
        {
            get => chipset?.Trim();
            set => SetProperty(ref chipset, value);
        }
        private string vram;
        public string Vram
        {
            get => vram?.Trim();
            set => SetProperty(ref vram, value);
        }
        private string gen;
        public string Gen
        {
            get => gen?.Trim();
            set => SetProperty(ref gen, value);
        }
        private string series;
        public string Series
        {
            get => series?.Trim();
            set => SetProperty(ref series, value);
        }
        protected override async void addCommand()
        {
            if (SelectedWorkType == "THÊM")
            {
                if (Id == null || Id.Length != 9 || !Id.StartsWith(DAStaticData.IdPrefix[ProductType.VGA]) || !Id.All(x => char.IsDigit(x)))
                {
                    _modalService.ShowModal(ModalType.Error, "Sai định dạng ID", "Cảnh báo");
                    return;
                }
                //make the two calls to IsProductExistInBill and IsIdExist concurrently
                var task1 = _unitOfWork.Bills.IsProductExistInBill(Id);
                var task2 = _unitOfWork.Vgas.IsIdExist(Id);

                await Task.WhenAll(task1, task2);

                bool exist = task1.Result || task2.Result;

                if (ProductList.Any(x => x.Id == Id) || exist)
                {
                    _modalService.ShowModal(ModalType.Error, "ID đã tồn tại", "Cảnh báo");
                    return;
                }
                if (Company == null || Unit == null || Name == null ||
                   Chip == null || Chipset == null || Vram == null || Gen == null)
                {
                    _modalService.ShowModal(ModalType.Error, "Nhập tất cả thông tin cần thiết", "Cảnh báo");
                    return;
                }
                Vga vgaDTO = GetPc();
                ProductList.Add(vgaDTO);
                NotInDatabase.Add(Id);
                clearCommand();
            }
            else if (SelectedWorkType == "SỬA")
            {
                if (Company == null || Unit == null || Name == null ||
                   Chip == null || Chipset == null || Vram == null || Gen == null)
                {
                    _modalService.ShowModal(ModalType.Error, "Nhập tất cả thông tin cần thiết", "Cảnh báo");
                    return;
                }
                Vga vgaDTO = GetPc();
                vgaDTO.Remain = Remain;
                var res = await _unitOfWork.Vgas.Update(vgaDTO);
                if ((bool)res)
                    _modalService.ShowModal(ModalType.Information, "Cập nhật thành công", "Thông báo");
                else _modalService.ShowModal(ModalType.Error, "Có lỗi xảy ra", "Thông báo");
                // Clear
                Empty();
                var list = await _unitOfWork.Vgas.GetAll();
                ProductList = new(list);
            }
        }
        protected override void clearCommand()
        {
            if (SelectedWorkType == "THÊM")
            {
                Empty();
            }
            else if (SelectedWorkType == "SỬA")
            {
                Price = 0; Discount = null;
                if (Product != null) findCommand(Product);
            }
        }

        protected override async void CurrentWorkTypeChanged()
        {
            if (SelectedWorkType == "THÊM")
            {
                ProductList = new();
            }
            else
            {
                var list = await _unitOfWork.Vgas.GetAll();
                ProductList = new(list);
            }
            clearCommand();
        }

        protected override async void deleteCommand(ProductDTO productDTO)
        {
            if (SelectedWorkType == "THÊM")
            {
                if (NotInDatabase.Contains(productDTO.Id))
                {
                    var p = ProductList.Single(x => x.Id == productDTO.Id);
                    ProductList.Remove(p);
                    NotInDatabase.Remove(productDTO.Id);
                }
            }
            else
            {
                MetroWindow metroWindow = (MetroWindow)Application.Current.MainWindow;
                if (metroWindow.ShowModalMessageExternal("Cảnh báo", "Bạn có chắc chắn xóa?", MessageDialogStyle.AffirmativeAndNegative) == MessageDialogResult.Affirmative)
                {
                    await _unitOfWork.Vgas.Delete(productDTO.Id);
                    var list = await _unitOfWork.Vgas.GetAll();
                    ProductList = new(list);
                }
            }
        }

        protected override void findCommand(ProductDTO productDTO)
        {
            Product = (Vga)productDTO;
            Id = Product.Id;
            Company = Product.Company;
            Unit = Product.Unit;
            Series = Product.Series;
            Name = Product.Name;
            Price = Product.Price;
            Discount = Product.Discount;
            DetailPath = Product.DetailPath;
            AvatarPath = Product.AvatarPath;
            ImagePath = Product.ImagePath;
            Chip = Product.Chip;
            Chipset = Product.Chipset;
            Vram = Product.Vram;
            Gen = Product.Gen;
            RaisePropertyChanged(nameof(IsDefault));
        }

        protected override async Task saveCommand()
        {
            var res = await _unitOfWork.Vgas.AddList(ProductList);
            if ((bool)res)
            {
                _modalService.ShowModal(ModalType.Information, "Đã lưu", "Thông báo");
                ProductList = new();
            }
            else _modalService.ShowModal(ModalType.Error, "Lưu không thành công", "Lỗi");
            await Task.CompletedTask;
        }

        protected override void editCommand(ProductDTO productDTO)
        {
            if (SelectedWorkType == "THÊM")
            {
                ProductList.Remove((Vga)productDTO);
                NotInDatabase.Remove(productDTO.Id);
            }
            findCommand(productDTO);
        }
        private Vga GetPc()
        {
            return new()
            {
                Id = Id,
                Company = Company,
                Unit = Unit,
                Series = Series,
                Name = Name,
                Price = Price,
                Discount = Discount,
                DetailPath = DetailPath,
                AvatarPath = AvatarPath,
                ImagePath = ImagePath,
                Chip = Chip,
                Chipset = Chipset,
                Gen = Gen,
                Vram = Vram,
                Remain = 0,
            };
        }
        private void Empty()
        {
            Id = null;
            Company = null;
            Unit = null;
            Series = null;
            Name = null;
            Price = 0;
            Discount = 0;
            DetailPath = null;
            AvatarPath = null;
            ImagePath = null;
            Chip = null;
            Chipset = null;
            Gen = null;
            Vram = null;
            Remain = 0;
            RaisePropertyChanged(nameof(IsDefault));
        }
    }
}
