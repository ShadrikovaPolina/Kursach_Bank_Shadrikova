using System;
using System.Windows.Forms;

namespace BankView
{
    public partial class FormAuthorization : Form
    {
        public FormAuthorization()
        {
            InitializeComponent();
        }

        private void buttonAuthorization_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxUserName.Text))
            {
                MessageBox.Show("Заполните поле логин", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPassword.Text))
            {
                MessageBox.Show("Заполните поле пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                ApiClient.Login(textBoxUserName.Text, textBoxPassword.Text);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxPassword.Clear();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonRegistration_Click(object sender, EventArgs e)
        {
            var form = new FormRegistration();
            form.ShowDialog();
        }
    }
}
