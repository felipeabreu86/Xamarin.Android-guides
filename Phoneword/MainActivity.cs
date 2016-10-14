using System;
using System.Collections.Generic;
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
        Button callHistoryButton = null;
        Button resetButton = null;
        string translatedNumber = string.Empty;
        static readonly List<string> phoneNumbers = new List<string>();

        /// <summary>
        ///     set our view from the "main" layout resource
        /// </summary>
        /// <param name="bundle">bundle</param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            InicializarAtributosDaClasse();
            OnClickTranslateButton();
            OnClickCallButton();
            OnClickCallHistoryButton();
            OnClickResetButton();
        }

        /// <summary>
        ///     Inicializar os atributos da classe MainActivity
        /// </summary>
        private void InicializarAtributosDaClasse()
        {
            ObterControlesDeInterfaceComUsuario();
            callButton.Enabled = false;
            VerificarBotaoCallHistory();
        }

        /// <summary>
        ///     Get our UI controls from the loaded layout
        /// </summary>
        private void ObterControlesDeInterfaceComUsuario()
        {
            phoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
            translateButton = FindViewById<Button>(Resource.Id.TranslateButton);
            callButton = FindViewById<Button>(Resource.Id.CallButton);
            callHistoryButton = FindViewById<Button>(Resource.Id.CallHistoryButton);
            resetButton = FindViewById<Button>(Resource.Id.ResetButton);
        }

        /// <summary>
        ///     Ação ao clicar no botão Translate
        /// </summary>
        private void OnClickTranslateButton()
        {
            translateButton.Click += (object sender, EventArgs e) =>
            {
                translatedNumber = Core.PhonewordTranslator.ToNumber(phoneNumberText.Text);
                VerificarBotaoCall();
            };
        }

        /// <summary>
        ///     On "Call" button click, try to dial phone number.
        /// </summary>
        private void OnClickCallButton()
        {
            callButton.Click += (object sender, EventArgs e) =>
            {
                RealizarChamadaTelefonica(translatedNumber);
            };
        }

        /// <summary>
        ///     Ação ao clicar no botão Call History
        /// </summary>
        private void OnClickCallHistoryButton()
        {
            callHistoryButton.Click += (object sender, EventArgs e) =>
            {
                var intent = new Intent(this, typeof(CallHistoryActivity));
                intent.PutStringArrayListExtra("phone_numbers", phoneNumbers);
                StartActivity(intent);
            };
        }

        /// <summary>
        ///     Ação ao clicar no botão Reset
        /// </summary>
        private void OnClickResetButton()
        {
            resetButton.Click += (object sender, EventArgs e) =>
            {
                phoneNumberText.Text = string.Empty;
                translatedNumber = string.Empty;
                VerificarBotaoCall();
            };
        }

        /// <summary>
        ///     Try to dial phone number.
        ///     Create intent to dial phone.
        ///     Show the alert dialog to the user and wait for response.
        /// </summary>
        /// <param name="phoneNumber">Número do telefone</param>
        protected void RealizarChamadaTelefonica(string phoneNumber)
        {
            var callDialog = new AlertDialog.Builder(this);
            callDialog.SetMessage("Call " + phoneNumber + "?");
            callDialog.SetNeutralButton("Call", delegate
            {
                AdicionarTelefoneNoHistorico(phoneNumber);
                VerificarBotaoCallHistory();
                var callIntent = new Intent(Intent.ActionCall);
                callIntent.SetData(Android.Net.Uri.Parse("tel:" + phoneNumber));
                StartActivity(callIntent);
            });
            callDialog.SetNegativeButton("Cancel", delegate { });
            callDialog.Show();
        }

        /// <summary>
        ///     Adiciona Telefone no histórico de ligações.
        /// </summary>
        /// <param name="phoneNumber">Número do telefone</param>
        private void AdicionarTelefoneNoHistorico(string phoneNumber)
        {
            if(!string.IsNullOrWhiteSpace(phoneNumber))
                phoneNumbers.Add(phoneNumber);            
        }

        /// <summary>
        ///     Habilita o botão Call History caso existam telefones no histórico.
        /// </summary>
        private void VerificarBotaoCallHistory()
        {
            if (phoneNumbers.Count == 0)
                callHistoryButton.Enabled = false;
            else
                callHistoryButton.Enabled = true;
        }

        /// <summary>
        ///     Habilita o botão Call caso tenha sido fornecido um telefone.
        ///     Atualiza o texto do botão Call.
        /// </summary>
        private void VerificarBotaoCall()
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
        }
    }
}