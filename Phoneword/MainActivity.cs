using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Phoneword
{
    [Activity(Label = "Phone Word", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        EditText phoneNumberText = null;
        Button translateButton = null;
        Button callButton = null;
        string translatedNumber = string.Empty;

        /// <summary>
        ///     set our view from the "main" layout resource
        /// </summary>
        /// <param name="bundle">bundle</param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            InstanciarAtributosDeClasse();
            OnClickTranslateButton();
            OnClickCallButton();
        }

        /// <summary>
        ///     Instanciar os atributos da MainActivity
        /// </summary>
        private void InstanciarAtributosDeClasse()
        {
            ObterControlesDeInterfaceComUsuario();
            callButton.Enabled = false;
            translatedNumber = Core.PhonewordTranslator.ToNumber(phoneNumberText.Text);
        }

        /// <summary>
        ///     Get our UI controls from the loaded layout
        /// </summary>
        private void ObterControlesDeInterfaceComUsuario()
        {
            EditText phoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
            Button translateButton = FindViewById<Button>(Resource.Id.TranslateButton);
            Button callButton = FindViewById<Button>(Resource.Id.CallButton);
        }

        /// <summary>
        ///     Ação ao clicar no botão Translate
        /// </summary>
        private void OnClickTranslateButton()
        {
            translateButton.Click += (object sender, EventArgs e) =>
            {
                if (String.IsNullOrWhiteSpace(translatedNumber))
                {
                    callButton.Text = "Call";
                    callButton.Enabled = false;
                }
                else
                {
                    callButton.Text = "Call " + translatedNumber;
                    callButton.Enabled = true;
                }
            };
        }

        /// <summary>
        ///     On "Call" button click, try to dial phone number.
        /// </summary>
        private void OnClickCallButton()
        {
            callButton.Click += (object sender, EventArgs e) =>
            {
                RealizarChamadaTelefonica();
            };
        }

        /// <summary>
        ///     Try to dial phone number.
        ///     Create intent to dial phone.
        ///     Show the alert dialog to the user and wait for response.
        /// </summary>
        protected void RealizarChamadaTelefonica()
        {
            var callDialog = new AlertDialog.Builder(this);
            callDialog.SetMessage("Call " + translatedNumber + "?");
            callDialog.SetNeutralButton("Call", delegate
            {
                var callIntent = new Intent(Intent.ActionCall);
                callIntent.SetData(Android.Net.Uri.Parse("tel:" + translatedNumber));
                StartActivity(callIntent);
            });
            callDialog.SetNegativeButton("Cancel", delegate { });
            callDialog.Show();
        }
    }
}

