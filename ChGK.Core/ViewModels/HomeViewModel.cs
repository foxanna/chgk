using System;
using System.Collections.Generic;
using System.Windows.Input;
using ChGK.Core.Services.Messenger;
using ChGK.Core.Utils;
using ChGK.Core.ViewModels.Search;
using MvvmCross.Core.ViewModels;

namespace ChGK.Core.ViewModels
{
    public class HomeViewModel : MvxViewModel
    {
        private readonly IMessagesService _messenger;

        private ICommand _showMenuItem;

        public HomeViewModel(IMessagesService messenger)
        {
            _messenger = messenger;

            MenuItems = new List<MenuItem>
            {
                new MenuItem {Name = StringResources.LastAdded, ViewModelType = typeof (LastAddedTournamentsViewModel)},
                new MenuItem {Name = StringResources.RandomQuestions, ViewModelType = typeof (RandomQuestionsViewModel)},
                new MenuItem {Name = StringResources.Search, ViewModelType = typeof (SearchParamsViewModel)},
                new MenuItem {Name = StringResources.Teams, ViewModelType = typeof (TeamsViewModel)},
                new MenuItem {Name = StringResources.AboutApp, ViewModelType = typeof (AboutViewModel)}
            };
        }

        public List<MenuItem> MenuItems { get; set; }

        public ICommand ShowMenuItem =>
            _showMenuItem ?? (_showMenuItem = new Command<MenuItem>(ShowMenu));

        public override void Start()
        {
            base.Start();

            ShowViewModel<LastAddedTournamentsViewModel>();
        }

        private void ShowMenu(MenuItem item)
        {
            _messenger.Publish(new CloseDrawerMessage(this));

            ShowViewModel(item.ViewModelType);
        }

        public class MenuItem
        {
            public string Name { get; set; }

            public Type ViewModelType { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}