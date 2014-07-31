using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Plugins.Messenger;
using ChGK.Core.Utils;

namespace ChGK.Core.ViewModels
{
	public class HomeViewModel : MvxViewModel
	{
		public class MenuItem
		{
			public string Name { get; set; }

			public Type ViewModelType { get; set; }

			public override string ToString ()
			{
				return Name;
			}
		}

		readonly IMvxMessenger _messenger;

		public HomeViewModel (IMvxMessenger messenger)
		{
			_messenger = messenger;

			MenuItems = new List<MenuItem> () {
				new MenuItem { Name = "Последние добавленные",  ViewModelType = typeof(LastAddedTournamentsViewModel) },
				new MenuItem { Name = "Случайные вопросы", ViewModelType = typeof(RandomQuestionsViewModel) },
				new MenuItem { Name = "О приложении", ViewModelType = typeof(AboutViewModel) },
			};
		}

		public override void Start ()
		{
			base.Start ();

//			ShowViewModel<RandomQuestionsViewModel> ();

			ShowViewModel<LastAddedTournamentsViewModel> ();
		}

		public List <MenuItem> MenuItems {
			get;
			set;
		}

		MvxCommand<MenuItem> _showMenuItem;

		public MvxCommand <MenuItem> ShowMenuItem {
			get {
				_showMenuItem = _showMenuItem ?? new MvxCommand<MenuItem> (ShowMenu);
				return _showMenuItem;
			}
		}

		void ShowMenu (MenuItem item)
		{
			_messenger.Publish (new CloseDrawerMessage (this));

			ShowViewModel (item.ViewModelType);
		}
	}
}

