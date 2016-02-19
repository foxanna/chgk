﻿using System;
using System.Collections.Generic;
using ChGK.Core.Utils;
using ChGK.Core.ViewModels.Search;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace ChGK.Core.ViewModels
{
    public class HomeViewModel : MvxViewModel
    {
        public class MenuItem
        {
            public string Name { get; set; }

            public Type ViewModelType { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        private readonly IMvxMessenger _messenger;

        public HomeViewModel(IMvxMessenger messenger)
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

        public override void Start()
        {
            base.Start();

            ShowViewModel<LastAddedTournamentsViewModel>();
        }

        public List<MenuItem> MenuItems { get; set; }

        private MvxCommand<MenuItem> _showMenuItem;

        public MvxCommand<MenuItem> ShowMenuItem
        {
            get
            {
                _showMenuItem = _showMenuItem ?? new MvxCommand<MenuItem>(ShowMenu);
                return _showMenuItem;
            }
        }

        private void ShowMenu(MenuItem item)
        {
            _messenger.Publish(new CloseDrawerMessage(this));

            ShowViewModel(item.ViewModelType);
        }
    }
}

