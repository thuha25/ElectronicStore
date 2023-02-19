﻿using Models;
using Models.Interfaces;
using Scrypt;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModels.Commands;
using ViewModels.MyMessageBox;
using ViewModels.Services;
using ViewModels.Stores;
using ViewModels.Stores.Accounts;

namespace ViewModels;

public class ResetPasswordViewModel : ViewModelBase
{
    private readonly AccountStore _accountStore;
    private readonly VerificationStore _verificationStore;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INavigationService _loginNavigate;
    public ResetPasswordViewModel(IUnitOfWork unitOfWork,
        AccountStore accountStore,
        VerificationStore verificationStore,
        INavigationService loginNavigate)
    {
        _unitOfWork = unitOfWork;
        _accountStore = accountStore;
        _verificationStore = verificationStore;
        _loginNavigate = loginNavigate;
        if (accountStore.CurrentAccount != null) IsOldPasswordType = true;
        ResetCommand = new RelayCommand<object>(_ => reset());
    }
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
    public string? ReTypeNewPassword { get; set; }
    public bool IsOldPasswordType { get; set; } = false;
    public ICommand ResetCommand { get; }
    private void reset()
    {
        if (IsOldPasswordType)
        {
            ScryptEncoder encoder = new ScryptEncoder();
            if (encoder.Compare(OldPassword, _accountStore.CurrentAccount!.PasswordHash) == false)
            {
                ErrorNotifyViewModel.Instance!.Show("Wrong Password", "Failed");
                return;
            }
            if (NewPassword != ReTypeNewPassword || NewPassword == null)
            {
                ErrorNotifyViewModel.Instance!.Show("New Password Invalid", "Failed");
                return;
            }
        }
        else
        {
            if (NewPassword != ReTypeNewPassword || NewPassword == null)
            {
                ErrorNotifyViewModel.Instance!.Show("New Password Invalid", "Failed");
                return;
            }
        }
        _loginNavigate.Navigate();
        Task task = new Task(resetAsync);
        task.Start();
    }
    private void resetAsync()
    {
        try
        {
            InformationViewModel.Instance!.Show("Please log in your account", "Success");
            _unitOfWork.Accounts.ResetPassword(_verificationStore.Id!, NewPassword!);
        }
        catch (Exception ex)
        {
            ErrorNotifyViewModel.Instance!.Show(ex.Message);
        }
    }
}
