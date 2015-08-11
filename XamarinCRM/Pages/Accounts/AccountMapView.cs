﻿using System.Threading.Tasks;
using MobileCRM.Pages.Base;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using MobileCRM.Layouts;
using MobileCRM.ViewModels.Customers;

namespace MobileCRM.Pages.Accounts
{
    public class AccountMapView : BaseView
    {
        public CustomerDetailViewModel ViewModel
        {
            get { return BindingContext as CustomerDetailViewModel; }
        }

        Map map;

        public AccountMapView(CustomerDetailViewModel vm)
        {
            this.Title = "Map";
            this.Icon = "map.png";

            this.BindingContext = vm;

            ViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Account")
                    MakeMap();
            };
                    
            map = new Map()
            {
                IsShowingUser = true
            };

            MakeMap();
            var stack = new UnspacedStackLayout();

            map.VerticalOptions = LayoutOptions.FillAndExpand;
            map.HeightRequest = 100;
            map.WidthRequest = 960;

            stack.Children.Add(map);
            Content = stack;
        }

        public async void MakeMap()
        {
            Task<Pin> getPinTask = ViewModel.LoadPin();
            Pin pin = await getPinTask;

            map.Pins.Clear();
            map.Pins.Add(pin);
            
            map.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMiles(5)));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MakeMap();

            Insights.Track("Account Contact Map Page");
        }
    }
}