using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;

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
        ///     Criação da Activity inicial do app
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
            VerificarBotaoCall();
            VerificarBotaoCallHistory();
        }

        /// <summary>
        ///     Obter os controles de interface com o usuário a partir do layout carregado
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
        ///     Traduz o telefone inserido pelo usuário e verifica o botão Call
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
        ///     Ação ao clicar no botão Call
        ///     Chamada ao método RealizarChamadaTelefonica()
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
        ///     Inicializar a CallHistoryActivity
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
        ///     Limpar os campos e verificar o botão Call
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
        ///     Tenta realizar uma ligação telefônica
        ///     Cria uma Intenta para realizar a ligação
        ///     Exibe uma tela de alerta para o usuário e espera uma resposta
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
        ///     Adiciona um telefone ao histórico de ligações
        /// </summary>
        /// <param name="phoneNumber">Número do telefone</param>
        private void AdicionarTelefoneNoHistorico(string phoneNumber)
        {
            if(!string.IsNullOrWhiteSpace(phoneNumber))
                phoneNumbers.Add(phoneNumber);            
        }

        /// <summary>
        ///     Habilita o botão Call History caso existam telefones no histórico de ligações
        /// </summary>
        private void VerificarBotaoCallHistory()
        {
            if (phoneNumbers.Count == 0)
                callHistoryButton.Enabled = false;
            else
                callHistoryButton.Enabled = true;
        }

        /// <summary>
        ///     Habilita o botão Call caso um telefone fornecido pelo usuário tenha sido traduzido
        ///     Atualiza o texto do botão Call
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