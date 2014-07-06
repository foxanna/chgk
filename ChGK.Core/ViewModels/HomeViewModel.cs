using System;
using System.Collections.Generic;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;

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
				//return string.Format ("[MenuItem: Name={0}, ViewModelType={1}]", Name, ViewModelType);
				return Name;
			}
		}

		//	"Случайные вопросы"
		//			RandomQuestions,
		//
		//			SomeOther
		//		}

		public HomeViewModel ()
		{
			MenuItems = new List<MenuItem> () {
				new MenuItem { Name = "Случайные вопросы", ViewModelType = typeof(RandomQuestionsViewModel) },
				new MenuItem { Name = "Последние добавленные",  ViewModelType = typeof(LastAddedTournamentsViewModel) },
			};
		}

		public List <MenuItem> MenuItems {
			get;
			set;
		}

		private MvxCommand<MenuItem> _showMenuItem;

		public MvxCommand <MenuItem> ShowMenuItem {
			get {
				_showMenuItem = _showMenuItem ?? new MvxCommand<MenuItem> (ShowMenu);
				return _showMenuItem;
			}
		}

		private void ShowMenu (MenuItem item)
		{
			ShowViewModel (item.ViewModelType);
		}
	}
}

