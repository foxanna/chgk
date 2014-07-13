using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using Android.Support.V4.View;
using Android.Support.V4.App;
using ChGK.Core.ViewModels;
using Cirrious.MvvmCross.Droid.Fragging;

namespace ChGK.Droid.Views
{
	[Android.App.Activity (Label = "")]			
	public class QuestionsView : MvxFragmentActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.QuestionsView);

			var viewPager = FindViewById<ViewPager> (Resource.Id.viewPager);
			viewPager.Adapter = new QuestionsPagerAdapter (SupportFragmentManager, ((QuestionsViewModel) ViewModel).Questions);
			viewPager.PageSelected += (sender, e) => ActionBar.Title = viewPager.Adapter.GetPageTitle (e.Position);
			viewPager.CurrentItem = ((QuestionsViewModel) ViewModel).Index;
		}
	}

	class QuestionsPagerAdapter : FragmentStatePagerAdapter
	{
		List<QuestionViewModel> _questions;

		public QuestionsPagerAdapter (FragmentManager fm, List<QuestionViewModel> questions) : base (fm)
		{
			_questions = questions;
		}

		public override int Count {
			get {
				return _questions.Count;
			}
		}

		public override Fragment GetItem (int position)
		{
			var fragment = new QuestionView ();
			fragment.ViewModel = _questions [position];
			return fragment;
		}

		public override Java.Lang.ICharSequence GetPageTitleFormatted (int position)
		{
			return new Java.Lang.String ("Вопрос " + (position + 1));
		}
	}
}

