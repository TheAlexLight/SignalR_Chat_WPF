﻿using ChatClient.ViewModels;
using SharedItems.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.Commands.CustomViewsCommands
{
    public class ChangeUserSettingsCommand : CommandBase
    {
        private readonly ChatViewModel _viewModel;

        public ChangeUserSettingsCommand(ChatViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            //object[] values = parameter as object[];

            if (parameter is ChangeSettingsType settingsType)
            {
                //if (values.Length != 2)
                //{
                    _viewModel.UserSettingsType = settingsType;

                    if (settingsType == ChangeSettingsType.None)
                    {
                        _viewModel.NeedToClearPassword = true;
                        _viewModel.UsernameSettingsField = string.Empty;;
                        _viewModel.EmailSettingsField = string.Empty;
                        _viewModel.Password = string.Empty;
                        _viewModel.PasswordConfirm = string.Empty;
                    }
                //}
            }
        }
    }
}
