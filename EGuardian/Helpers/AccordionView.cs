using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace EGuardian.Helpers
{
    public class AccordionView : ContentView
    {
        #region Private Variables
        List<AccordionSource> mDataSource;
        bool mFirstExpaned = false;
        StackLayout mMainLayout;
        #endregion

        public AccordionView()
        {
            var mMainLayout = new StackLayout();
            Content = mMainLayout;
        }

        public AccordionView(List<AccordionSource> aSource)
        {
            mDataSource = aSource;
            DataBind();
        }

        #region Properties
        public List<AccordionSource> DataSource
        {
            get { return mDataSource; }
            set { mDataSource = value; }
        }
        public bool FirstExpaned
        {
            get { return mFirstExpaned; }
            set { mFirstExpaned = value; }
        }
        #endregion

        public void DataBind()
        {
            var vMainLayout = new StackLayout() { Spacing = 0 };
            var vFirst = true;
            if (mDataSource != null)
            {
                foreach (var vSingleItem in mDataSource)
                {

                    var vHeaderButton = new AccordionButton();
                    var vAccordionContent = new ContentView()
                    {
                        Content = vSingleItem.Contenido,
                        IsVisible = false
                    };
                    if (vFirst)
                    {
                        vHeaderButton.Expand = mFirstExpaned;
                        vAccordionContent.IsVisible = mFirstExpaned;
                        vFirst = false;
                    }
                    vHeaderButton.AssosiatedContent = vAccordionContent;
                    vHeaderButton.Clicked += OnAccordionButtonClicked;
                    vHeaderButton.IsVisible = vSingleItem.Cabecera.IsVisible;
                    vMainLayout.Children.Add(
                        new Grid
                        {
                            IsVisible = vSingleItem.Cabecera.IsVisible,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            HeightRequest = 30,
                            MinimumHeightRequest = 30,
                            Children =
                            {
                                vSingleItem.Cabecera,
                                vHeaderButton
                            }
                        });
                    vMainLayout.Children.Add(vAccordionContent);

                }
            }
            mMainLayout = vMainLayout;
            Content = mMainLayout;
        }

        void OnAccordionButtonClicked(object sender, EventArgs args)
        {
            var vSenderButton = (AccordionButton)sender;
            var isVisible = vSenderButton.Expand;
            foreach (var vChildItem in mMainLayout.Children)
            {
                if (vChildItem.GetType() == typeof(ContentView)) vChildItem.IsVisible = false;
                if (vChildItem.GetType() == typeof(Grid))
                {
                    var vGrid = (Grid)vChildItem;
                    foreach (var itemGrid in vGrid.Children)
                    {
                        if (itemGrid.GetType() == typeof(AccordionButton))
                        {
                            var vButton = (AccordionButton)itemGrid;
                            vButton.Expand = false;
                        }
                    }
                }
            }


            if (isVisible)
            {
                vSenderButton.Expand = false;
                double position = 0;
                vSenderButton.AssosiatedContent.Animate("collapse",
                    x =>
                    {
                        position = vSenderButton.AssosiatedContent.Y * x;
                    }, 16, 0, Easing.SpringOut, (d, b) =>
                    {
                        System.Diagnostics.Debug.WriteLine("altura: " + vSenderButton.AssosiatedContent.Height);
                        System.Diagnostics.Debug.WriteLine("posicion: " + position);
                    });
            }
            else
            {
                vSenderButton.Expand = true;
                double position = 0;
                vSenderButton.AssosiatedContent.Animate("expand",
                    x =>
                    {
                        position = vSenderButton.AssosiatedContent.Y * x;
                    }, 16, 0, Easing.SpringOut, (d, b) =>
                    {
                        System.Diagnostics.Debug.WriteLine("altura: " + vSenderButton.AssosiatedContent.Height);
                        System.Diagnostics.Debug.WriteLine("posicion: " + position);
                    });
            }
            vSenderButton.AssosiatedContent.IsVisible = vSenderButton.Expand;
        }

    }

    public class AccordionButton : Button
    {
        #region Private Variables
        bool mExpand = false;
        #endregion
        public AccordionButton()
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            BackgroundColor = Color.Transparent;
            BorderColor = Color.Transparent;
            MinimumHeightRequest = 30;
            BorderWidth = 0;
        }
        #region Properties
        public bool Expand
        {
            get { return mExpand; }
            set { mExpand = value; }
        }
        public ContentView AssosiatedContent
        { get; set; }
        #endregion
    }

    public class AccordionSource
    {
        public StackLayout Cabecera { get; set; }
        public StackLayout Contenido { get; set; }
    }
}


