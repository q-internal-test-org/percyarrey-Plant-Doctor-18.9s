﻿using Plant_Doctor.Tables;
using Plant_Doctor.View;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Android.App.ActionBar;
using static Java.Util.Jar.Attributes;

namespace Plant_Doctor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        public Dashboard()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            var dppath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dppath);
            if (db.GetTableInfo("reguserdata").Count <= 0)
            {
                Welcomesignup(); 
            }
            else
            {
                var myquery = db.Table<reguserdata>().FirstOrDefault();
                if (myquery != null)
                {
                    if(myquery.fname== myquery.lname)
                    {
                        Name.Text = myquery.fname;
                    }
                    else
                    {
                        Name.Text = myquery.fname + " " + myquery.lname;
                    }
                    Email.Text = myquery.email;
                }
            }
        }
        private async void Welcomesignup()
        {
            var result = await DisplayAlert("You have No Created Account", "Would you like to Sign Up ", "Sign Up", "Cancel");
            if (result)
            {
                App.Current.MainPage = new NavigationPage(new Startup());
            }
        }
        private void Aboutfxn(object sender, EventArgs e)
        {

            Navigation.PushModalAsync(new NavigationPage(new About()));
        }
        private void Mainsignup(object sender, EventArgs e)
        {
            _ = Navigation.PushModalAsync(new Startup());
        }
        private void P_Diagnosticfxn(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new P_Diagnostic());
        }
        private void P_diagpagefxn(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new P_Diagnostic());
        }
        private async void Settingfxn(object sender, EventArgs e)
        {   
            Menuhidebtn.IsEnabled = true;
            Menuhidebtn.IsVisible = true;
            Menuadd.IsVisible = true;
            Menuadd.IsEnabled = true;
            await Menuadd.TranslateTo(0, 0, 300);
        }
        private async void Menuhidefxn(object sender, EventArgs e)
        {   
            Menuhidebtn.IsEnabled = false;
            Menuhidebtn.IsVisible = false;
            await Menuadd.TranslateTo(-200, 0, 200);
            Menuadd.IsVisible = false;
            Menuadd.IsEnabled = false;
        }
        private async void Signoutfxn(object sender, EventArgs e)
        {

            var dppath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dppath);
            if (db.GetTableInfo("reguserdata").Count <= 0)
            {
                var newresult = await DisplayAlert("Hmmmmmmmmmm", "How do you want to Sign Down when you have not Sign Up", "SignUp", "Cancel");
                if (newresult)
                {
                    _ = Navigation.PushModalAsync(new Startup());
                }
            }
            else
            {
                var result = await DisplayAlert("Warning", "Do you wish to Sign Out", "Yes", "No");
                if (result)
                {
                    App.Current.MainPage = new NavigationPage(new Startup());
                }
            }
            
        }
        private void Alertnonefxn(object sender, EventArgs e)
        {
            DisplayAlert("Alert", "Page is NOT yet available", "Ok");
        }
        private async void Editaccountfxn(object sender, EventArgs e)
        {
            var dppath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UserDatabase.db");
            var db = new SQLiteConnection(dppath);
            if (db.GetTableInfo("reguserdata").Count <= 0)
            {
                var result = await DisplayAlert("You have No Created Account", "Would you like to Sign Up ", "Sign Up", "Cancel");
                if (result)
                {
                    _ = Navigation.PushModalAsync(new Startup());
                }
            }
            else
            {
                var result = await DisplayAlert("Warning", "Would you like to save your changes", "Yes", "No");
                if (result)
                {
                    var myquery = db.Table<reguserdata>().FirstOrDefault();
                    if (myquery != null)
                    {
                        myquery.fname = Name.Text;
                        myquery.email = Email.Text;
                        var item = db.Table<reguserdata>().FirstOrDefault(x=>x.Userid==myquery.Userid);
                        if (item != null)
                            {
                                item.fname = Name.Text;
                                item.lname = Name.Text;
                                item.email = Email.Text;
                                db.Update(item);
                            _ = DisplayAlert("Alert", "Changes Saved", "Ok");
                            Console.WriteLine(db);
                        }
                    }
                }
                else
                {
                    _ = DisplayAlert("Alert", "Changes NOT Saved", "Ok");
                }
                
            }
            
        }

    }
}