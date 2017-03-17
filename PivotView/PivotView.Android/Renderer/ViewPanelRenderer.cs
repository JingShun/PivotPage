using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PivotView.Droid;
using static Android.Views.View;
using Android.Support.V4.View;
using Java.Lang;
using System.Collections;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;
using PivotPagePortable;

[assembly: ExportRenderer(typeof(ViewPanel), typeof(ViewPanelRenderer))]
namespace PivotView.Droid
{
    public class ViewPanelRenderer:ViewRenderer<ViewPanel, ViewPager>
    {
        private ViewPanel _viewPanel;
        private ViewPager _viewPager;
        protected override void OnElementChanged(ElementChangedEventArgs<ViewPanel> e)
        {
            base.OnElementChanged(e);
            _viewPanel = this.Element;
            if (this.Control == null)
            {
                var viewpager = new ViewPager(this.Context);
                viewpager.Adapter = new CustomPagerAdapter(this.Context, this.Element);
                viewpager.PageSelected += Viewpager_PageSelected;
                
                this.SetNativeControl(viewpager);
                _viewPager = viewpager;
                _viewPanel.Select = Select;
            }
        }
        /// <summary>
        /// ������������ViewPager����ʾ��
        /// </summary>
        /// <param name="index">��������0��ʼ</param>
        public void Select(int index)
        {
            _viewPager.SetCurrentItem(index, true);
        }
        /// <summary>
        /// ViewPager����ʾ����ͼ�����仯��֪ͨViewPannel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Viewpager_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            if (_viewPanel == null)
                return;
            _viewPanel.CurrentIndex = e.Position;
            _viewPanel.OnSelectChanged();
        }
    }

    /// <summary>
    /// ViewPager��Ӧ��Adapter
    /// </summary>
    public class CustomPagerAdapter : PagerAdapter
    {
        private ViewPanel _customViewPage;
        private Context _context;
        private IList _views = new List<Xamarin.Forms.View>();
        public CustomPagerAdapter(Context context, ViewPanel customViewPage)
        {
            _customViewPage = customViewPage;
            _views = customViewPage.Children ;
            _context = context;
        }
        public override int Count
        {
            get
            {
                return _views.Count;
            }
        }

        public override Java.Lang.Object InstantiateItem(Android.Views.View container, int position)
        {
            var viewPager = container.JavaCast<ViewPager>();
            var view = _views[position] as Xamarin.Forms.View;
            view.Parent = _customViewPage;
            if (Platform.GetRenderer(view) == null)
                Platform.SetRenderer(view, Platform.CreateRenderer(view));
            var renderer = Platform.GetRenderer(view);
            var viewGroup = renderer.ViewGroup;
            viewPager.AddView(viewGroup);
            return viewGroup;
        }

        public override bool IsViewFromObject(Android.Views.View view, Java.Lang.Object objectValue)
        {
            return view == objectValue;
        }
        public override void DestroyItem(Android.Views.View container, int position, Java.Lang.Object view)
        {
            var viewPager = container.JavaCast<ViewPager>();
            viewPager.RemoveView(view as Android.Views.View);
        }
        
    }
}