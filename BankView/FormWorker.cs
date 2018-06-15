using BankService.BindingModels;
using BankService.ViewModel;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankView
{
    public partial class FormWorker : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormWorker()
        {
            InitializeComponent();
        }

        private void FormWorker_Load(object sender, EventArgs e)
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFIO.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string fio = textBoxFIO.Text;
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => ApiClient.PutRequestData("api/Worker/UpdElement", new WorkerBindingModel
                {
                    Id = id.Value,
                    WorkerFIO = fio,
                }));
            }
            else
            {
                task = Task.Run(() => ApiClient.PostRequestData("api/Worker/AddElement", new WorkerBindingModel
                {
                    WorkerFIO = fio
                }));
            }
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
    }
}
