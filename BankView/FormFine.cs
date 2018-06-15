using BankService.BindingModels;
using BankService.ViewModel;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankView
{
    public partial class FormFine : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormFine()
        {
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxSum.Text))
            {
                MessageBox.Show("Введите сумму штрафа!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int sum = 0;
            try
            {
                sum = Convert.ToInt32(textBoxSum.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Task task = Task.Run(() => ApiClient.PutRequestData("api/Worker/DownBonus", new CalcBindingModel
            {
                WorkerId = id.Value,
                Fine = sum
            }));

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information),
            TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith((prevTask) =>
            {
                var ex = (Exception)prevTask.Exception;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }, TaskContinuationOptions.OnlyOnFaulted);

            Close();
        }

        private void FormFine_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var employee = Task.Run(() => ApiClient.GetRequestData<WorkerViewModel>("api/Worker/Get/" + id.Value)).Result;
                    textBoxFIO.Text = employee.WorkerFIO;
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
