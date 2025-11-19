using ClientApp.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;

namespace ClientApp
{
    public partial class SignUp : Form
    {
        private readonly UserAuth authService;
        public SignUp(UserAuth _authService)
        {
            InitializeComponent();
            authService = _authService;
        }

        private void SignUp_Load(object sender, EventArgs e)
        {

        }

        private async void btn_signup_Click(object sender, EventArgs e)
        {
            
        }

    }
}
