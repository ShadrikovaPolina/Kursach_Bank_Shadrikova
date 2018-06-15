using BankService.BindingModels;
using BankService.ViewModel;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankView
{
    public partial class FormBonus : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormBonus()
        {
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxSum.Text))
            {
                MessageBox.Show("Введите сумму бонуса!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            Task task = Task.Run(() => ApiClient.PutRequestData("api/Worker/UpBonus", new CalcBindingModel
            {
                WorkerId = id.Value,
                Bonus = sum
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormBonus_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var worker = Task.Run(() => ApiClient.GetRequestData<WorkerViewModel>("api/Worker/Get/" + id.Value)).Result;
                    textBoxFIO.Text = worker.WorkerFIO;
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
    }
}
