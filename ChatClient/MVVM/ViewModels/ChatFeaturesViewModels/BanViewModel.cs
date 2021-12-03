using ChatClient.Commands.AuthenticationCommands;
using ChatClient.MVVM.ViewModels.BaseViewModels;
using ChatClient.Services.BaseConfiguration;
using SharedItems.Models;
using SharedItems.Models.StatusModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace ChatClient.MVVM.ViewModels.ChatFeaturesModels
{
    public class BanViewModel : ChatViewModelBase
    {
        public BanViewModel(ChatBaseConfiguration baseConfiguration,
                BanStatusModel banStatus, UserModel currentUser) : base(baseConfiguration)
        {
            _banStatus = banStatus;
            _currentUser = currentUser;

            ReconnectionCommand = new ReconnectionCommand(this, currentUser);

            bool isPermanent = CheckStartupData(banStatus);

            if (!isPermanent)
            {
                RemainingTime = BanStatus.EndTime.Subtract(DateTime.Now).ToString(@"hh\:mm\:ss");

                _timer = new DispatcherTimer();
                StartClock();
            }
        }

        private readonly DispatcherTimer _timer;
        private BanStatusModel _banStatus;
        private UserModel _currentUser;

        private string _remainingTime;
        private bool _isEnabled;

        public BanStatusModel BanStatus 
        {
            get => _banStatus;
            set
            {
                _banStatus = value;
                OnPropertyChanged();
            }
        }

        public string RemainingTime
        {
            get => _remainingTime;
            set
            {
                _remainingTime = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool CheckStartupData(BanStatusModel model)
        {
            if (model.IsPermanent)
            {
                RemainingTime = "Forever";
            }

            return model.IsPermanent;
        }

        private void StartClock()
        {
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int checkTime = DateTime.Compare(DateTime.Now, BanStatus.EndTime);

            if (checkTime < 0)
            {
                RemainingTime = BanStatus.EndTime.Subtract(DateTime.Now).ToString(@"hh\:mm\:ss");
            }
            else
            {
                _timer.Tick -= Timer_Tick;
                RemainingTime = new TimeSpan(0).ToString(@"hh\:mm\:ss");
                IsEnabled = true;
                BanStatus.IsBanned = false;
            }
        }

        protected async override Task Reconnect()
        {
            await BaseConfiguration.ChatService.AuthorizationModel.Reconnect(_currentUser.UserProfile.Username);
        }
    }
}
