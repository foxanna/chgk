using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V13.App;
using Android.Support.V4.View;
using Android.Views;
using ChGK.Core.ViewModels;
using Java.Lang;

namespace ChGK.Droid.Views
{
    [Activity(Label = "Вопрос 1", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class QuestionsView : MenuItemIndependentView
    {
        private ViewPager _viewPager;

        protected override int LayoutId => Resource.Layout.QuestionsView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _viewPager = FindViewById<ViewPager>(Resource.Id.viewPager);
            _viewPager.Adapter = new QuestionsPagerAdapter(FragmentManager, ((QuestionsViewModel) ViewModel).Questions);
            _viewPager.PageSelected += (sender, e) => ActionBar.Title = _viewPager.Adapter.GetPageTitle(e.Position);
            _viewPager.CurrentItem = ((QuestionsViewModel) ViewModel).Index;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.questions, menu);
            return true;
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.enter_results:
                {
                    (ViewModel as QuestionsViewModel)?.Questions?[_viewPager.CurrentItem]?.EnterResults();
                    return true;
                }
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }

    internal class QuestionsPagerAdapter : FragmentStatePagerAdapter
    {
        private readonly List<QuestionViewModel> _questions;

        public QuestionsPagerAdapter(FragmentManager fm, List<QuestionViewModel> questions) : base(fm)
        {
            _questions = questions;
        }

        public override int Count => _questions.Count;

        public override Fragment GetItem(int position)
        {
            var fragment = new QuestionView
            {
                RetainInstance = true,
                ViewModel = _questions[position]
            };
            return fragment;
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new String("Вопрос " + (position + 1));
        }
    }
}